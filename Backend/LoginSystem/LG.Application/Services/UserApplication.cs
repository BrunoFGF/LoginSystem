using AutoMapper;
using LG.Application.Commons.Bases;
using LG.Application.Dtos.User.Request;
using LG.Application.Dtos.User.Response;
using LG.Application.Interfaces;
using LG.Application.Validators.User;
using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;
using LG.Infrastructure.Persistences.Interfaces;
using LG.Utilities.Static;
using System.Text.RegularExpressions;

namespace LG.Application.Services
{
    public class UserApplication : IUserApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserValidator _userValidator;

        public UserApplication(IUnitOfWork unitOfWork, IMapper mapper, UserValidator userValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userValidator = userValidator;
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
            var userSelected = await _unitOfWork.User.GetByIdAsync(userId);

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

        public async Task<BaseResponse<bool>> RegisterUser(UserRequestDto requestDto)
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

            var person = await _unitOfWork.Person.GetByIdAsync(requestDto.PersonId);
            if (person == null)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_PERSON_NOT_FOUND;
                return response;
            }

            var user = _mapper.Map<User>(requestDto);

            if (await _unitOfWork.User.UsernameExists(user.Username))
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_USERNAME_EXISTS;
                return response;
            }

            user.Mail = await GenerateEmailFromPerson(person);

            response.Data = await _unitOfWork.User.RegisterAsync(user);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message += RepplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_FAILED;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> EditUser(int userId, UserRequestDto requestDto)
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

            var user = _mapper.Map<User>(requestDto);
            user.Id = userId;
            user.Mail = userEdit.Data.Mail; 

            response.Data = await _unitOfWork.User.EditAsync(user);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message += RepplyMessage.MESSAGE_UPDATE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_FAILED;
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

            response.Data = await _unitOfWork.User.RemoveAsync(userId);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message += RepplyMessage.MESSAGE_DELETE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_FAILED;
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

            // Remover caracteres especiales y espacios
            string result = stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
            result = Regex.Replace(result, @"[^a-zA-Z0-9]", "");

            return result;
        }
    }
}