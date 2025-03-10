using AutoMapper;
using LG.Application.Commons.Bases;
using LG.Application.Dtos.User.Request;
using LG.Application.Dtos.User.Response;
using LG.Application.Interfaces;
using LG.Application.Validators.User;
using LG.Domain.Commons.Bases.Request;
using LG.Domain.Commons.Bases.Response;
using LG.Domain.Entities;
using LG.Domain.Repositories;
using LG.Utilities.Static;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using BC = BCrypt.Net.BCrypt;

namespace LG.Application.Services
{
    public class UserApplication : IUserApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserValidator _userValidator;
        private readonly IConfiguration _configuration;

        public UserApplication(IUnitOfWork unitOfWork, IMapper mapper, UserValidator userValidator, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userValidator = userValidator;
            _configuration = configuration;
        }

        public async Task<BaseResponse<BaseEntityResponse<UserResponseDto>>> ListUsers(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<UserResponseDto>>();
            var users = await _unitOfWork.User.ListUsers(filters);

            if (users is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<BaseEntityResponse<UserResponseDto>>(users);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<IEnumerable<UserSelectResponseDto>>> ListSelectUsers()
        {
            var response = new BaseResponse<IEnumerable<UserSelectResponseDto>>();
            var users = await _unitOfWork.User.GetAllAsync();

            if (users is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<UserSelectResponseDto>>(users);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<UserResponseDto>> UserById(int userId)
        {
            var response = new BaseResponse<UserResponseDto>();

            var userSelected = await _unitOfWork.User.GetByIdAsync(userId, u => u.Person);
            if (userSelected is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<UserResponseDto>(userSelected);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterUser(CreateUserRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            var validationResult = await _userValidator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_VALIDATE;
                response.Errors = validationResult.Errors;
                return response;
            }

            if (requestDto.RolName != "ADMIN" && requestDto.RolName != "USER")
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_ROL_MUST_BE;
                return response;
            }

            if (await _unitOfWork.User.UsernameExists(requestDto.Username))
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_USERNAME_EXISTS;
                return response;
            }

            if (await _unitOfWork.Person.IdentityCardExists(requestDto.IdentityCard))
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_PERSON_IDENTIFICATION_ALREADY_EXISTS;
                return response;
            }

            try
            {
                var person = _mapper.Map<Person>(requestDto);
                var personRegistered = await _unitOfWork.Person.RegisterAsync(person);

                if (!personRegistered)
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_PERSON_REGISTER_FAILED;
                    return response;
                }

                var user = _mapper.Map<User>(requestDto);

                user.Password = BC.HashPassword(user.Password);

                user.PersonId = person.Id;
                user.Mail = await GenerateEmailFromPerson(person);

                response.Data = await _unitOfWork.User.RegisterAsync(user);

                if (response.Data)
                {
                    var role = await _unitOfWork.UserRol.GetRolByName(requestDto.RolName);
                    if (role == null)
                    {
                        response.IsSuccess = false;
                        response.Message = RepplyMessage.MESSAGE_ROL_NAME_NOT_FOUND + $"Nombre: {requestDto.RolName}" ;
                        return response;
                    }

                    await _unitOfWork.UserRol.AssignRolesToUser(user.Id, new List<int> { role.Id }, user.Id);

                    response.IsSuccess = true;
                    response.Message += RepplyMessage.MESSAGE_SAVE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message += RepplyMessage.MESSAGE_FAILED;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EditUser(int userId, UpdateUserRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var userEdit = await UserById(userId);
            if (userEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            if (userEdit.Data.Username != requestDto.Username)
            {
                if (await _unitOfWork.User.UsernameExists(requestDto.Username))
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_USERNAME_EXISTS;
                    return response;
                }
            }

            var personId = userEdit.Data.Person?.PersonId;
            if (personId.HasValue && requestDto.IdentityCard != userEdit.Data.Person?.IdentityCard)
            {
                if (await _unitOfWork.Person.IdentityCardExists(requestDto.IdentityCard))
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_PERSON_IDENTIFICATION_ALREADY_EXISTS;
                    return response;
                }
            }

            if (requestDto.RolName != "ADMIN" && requestDto.RolName != "USER")
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_ROL_MUST_BE;
                return response;
            }

            try
            {
                if (personId.HasValue)
                {
                    var person = _mapper.Map<Person>(requestDto);
                    person.Id = personId.Value;

                    var personUpdated = await _unitOfWork.Person.EditAsync(person);
                    if (!personUpdated)
                    {
                        response.IsSuccess = false;
                        response.Message = RepplyMessage.MESSAGE_FAILED;
                        return response;
                    }
                }

                var user = _mapper.Map<User>(requestDto);
                user.Id = userId;

                var personChecked = _mapper.Map<Person>(requestDto);
                user.Mail = await GenerateEmailFromPerson(personChecked);

                user.PersonId = personId ?? 0;

                if (!string.IsNullOrEmpty(requestDto.Password))
                {
                    user.Password = BC.HashPassword(requestDto.Password);
                }
                else
                {
                    var userWithPass = await _unitOfWork.User.GetByIdAsync(userId);

                    user.Password = userWithPass.Password; 
                }

                var userUpdated = await _unitOfWork.User.EditAsync(user);

                if (!userUpdated)
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_FAILED;
                    return response;
                }

                var role = await _unitOfWork.UserRol.GetRolByName(requestDto.RolName);
                if (role == null)
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_ROL_NAME_NOT_FOUND + $"Nombre: {requestDto.RolName}";
                    return response;
                }

                await _unitOfWork.UserRol.AssignRolesToUser(user.Id, new List<int> { role.Id }, user.Id);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = RepplyMessage.MESSAGE_UPDATE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RemoveUser(int userId)
        {
            var response = new BaseResponse<bool>();
            var user = await UserById(userId);

            if (user.Data is null)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            try
            {
                var userRemoved = await _unitOfWork.User.RemoveAsync(userId);

                if (!userRemoved)
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_FAILED;
                    return response;
                }

                var personId = user.Data.Person?.PersonId;

                if (personId.HasValue)
                {
                    var personRemoved = await _unitOfWork.Person.RemoveAsync(personId.Value);
                    if (!personRemoved)
                    {
                        response.IsSuccess = false;
                        response.Message = RepplyMessage.MESSAGE_FAILED;
                        return response;
                    }
                }

                response.IsSuccess = true;
                response.Data = true;
                response.Message = RepplyMessage.MESSAGE_DELETE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

        private async Task<string> GenerateEmailFromPerson(Person person)
        {
            string firstNameLetter = person.FirstName.Trim().Split(' ')[0].Substring(0, 1).ToLower();

            string lastName = person.LastName.Trim().Split(' ')[0].ToLower();

            firstNameLetter = RemoveAccents(firstNameLetter);
            lastName = RemoveAccents(lastName);

            string baseEmail = $"{firstNameLetter}{lastName}";

            var emailExists = await _unitOfWork.User.EmailExists(baseEmail + "@mail.com");

            if (!emailExists)
            {
                return baseEmail + "@mail.com";
            }

            int counter = 1;
            string newEmail;

            do
            {
                newEmail = $"{baseEmail}{counter}@mail.com";
                counter++;
            } while (await _unitOfWork.User.EmailExists(newEmail));

            return newEmail;
        }

        private string RemoveAccents(string text)
        {
            string normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (char c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string result = stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
            result = Regex.Replace(result, @"[^a-zA-Z0-9]", "");

            return result;
        }

        public async Task<BaseResponse<string>> GenerateToken(TokenRequestDto requestDto)
        {
            var response = new BaseResponse<string>();
            var account = await _unitOfWork.User.AccountByUserName(requestDto.Username!);

            if (account.AuditDeleteUser != null && account.AuditDeleteDate != null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario ha sido eliminado del sistema.";
                return response;
            }

            if (account is not null)
            {
                if (BC.Verify(requestDto.Password, account.Password))
                {
                    response.IsSuccess = true;
                    response.Data = await GenerateToken(account);
                    response.Message = RepplyMessage.MESSAGE_TOKEN;
                    return response;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_TOKEN_ERROR;
            }

            return response;
        }

        private async Task<string> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRoles = await _unitOfWork.UserRol.GetRolesByUserId(user.Id);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Email, user.Mail!),
                new Claim("username", user.Username!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RolName));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"]!)),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}