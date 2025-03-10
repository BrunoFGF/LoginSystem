using LG.Application.Commons.Bases;
using LG.Application.Dtos.Account.Request;
using LG.Application.Dtos.Account.Response;
using LG.Application.Dtos.Person.Response;
using LG.Application.Interfaces;
using LG.Domain.Entities;
using LG.Domain.Repositories;
using LG.Domain.Services;
using LG.Infrastructure.Persistences.Repositories;
using LG.Utilities.Static;
using System.Text.RegularExpressions;
using BC = BCrypt.Net.BCrypt;

namespace LG.Application.Services
{
    public class AccountApplication : IAccountApplication
    {
        private readonly ICurrentSessionService _sessionService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountApplication(ICurrentSessionService sessionService, IUnitOfWork unitOfWork)
        {
            _sessionService = sessionService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountResponseDto> GetUserAccountAsync()
        {
            try
            {
                var userId = _sessionService.Get();
                var user = await _unitOfWork.User.GetUserByIdWithPerson(userId);

                if (user == null)
                    return null;


                return new AccountResponseDto
                {
                    Username = user.Username,
                    Mail = user.Mail,
                    Status = user.Status,
                    Person = user.Person != null ? new PersonResponseDto
                    {
                        PersonId = user.Person.Id,
                        FirstName = user.Person.FirstName,
                        LastName = user.Person.LastName,
                        IdentityCard = user.Person.IdentityCard,
                        BirthDate = user.Person.BirthDate
                    } : null
                };
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<BaseResponse<bool>> UpdateAccountAsync(AccountRequestDto request)
        {
            var response = new BaseResponse<bool>();
            var userId = _sessionService.Get();
            var user = await _unitOfWork.User.GetUserByIdWithPerson(userId);

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            // Check if username exists (if changed)
            if (user.Username != request.Username)
            {
                if (await _unitOfWork.User.UsernameExists(request.Username))
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_USERNAME_EXISTS;
                    return response;
                }
            }

            // Check if identity card exists (if changed)
            if (user.Person != null && request.IdentityCard != user.Person.IdentityCard)
            {
                if (await _unitOfWork.Person.IdentityCardExists(request.IdentityCard))
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_PERSON_IDENTIFICATION_ALREADY_EXISTS;
                    return response;
                }
            }

            try
            {
                if (user.Person != null)
                {
                    user.Person.FirstName = request.FirstName;
                    user.Person.LastName = request.LastName;
                    user.Person.IdentityCard = request.IdentityCard;
                    user.Person.BirthDate = request.BirthDate;

                    var personUpdated = await _unitOfWork.Person.EditAsync(user.Person);
                    if (!personUpdated)
                    {
                        response.IsSuccess = false;
                        response.Message = RepplyMessage.MESSAGE_FAILED;
                        return response;
                    }
                }


                user.Username = request.Username;

                var personForEmail = new Person
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IdentityCard = request.IdentityCard
                };
                user.Mail = await GenerateEmailFromPerson(personForEmail);

                if (!string.IsNullOrEmpty(request.Password))
                {
                    user.Password = BC.HashPassword(request.Password);
                }

                if (!string.IsNullOrEmpty(request.Status))
                {
                    user.Status = request.Status;
                }

                var userUpdated = await _unitOfWork.User.EditAsync(user);
                if (!userUpdated)
                {
                    response.IsSuccess = false;
                    response.Message = RepplyMessage.MESSAGE_FAILED;
                    return response;
                }

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
    }
}
