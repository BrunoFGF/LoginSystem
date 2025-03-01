using AutoMapper;
using LG.Application.Commons.Bases;
using LG.Application.Dtos.Person.Request;
using LG.Application.Dtos.Person.Response;
using LG.Application.Interfaces;
using LG.Application.Validators.Person;
using LG.Domain.Entities;
using LG.Infrastructure.Commons.Bases.Request;
using LG.Infrastructure.Commons.Bases.Response;
using LG.Infrastructure.Persistences.Interfaces;
using LG.Utilities.Static;

namespace LG.Application.Services
{
    public class PersonApplication : IPersonApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PersonValidator _personValidator;

        public PersonApplication(IUnitOfWork unitOfWork, IMapper mapper, PersonValidator personValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _personValidator = personValidator;
        }

        public async Task<BaseResponse<BaseEntityResponse<PersonResponseDto>>> ListPersons(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<PersonResponseDto>>();
            var persons = await _unitOfWork.Person.ListPersons(filters);

            if (persons is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<BaseEntityResponse<PersonResponseDto>>(persons);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<IEnumerable<PersonSelectResponseDto>>> ListSelectPersons()
        {
            var response = new BaseResponse<IEnumerable<PersonSelectResponseDto>>();
            var persons = await _unitOfWork.Person.GetAllAsync();

            if (persons is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<PersonSelectResponseDto>>(persons);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<PersonResponseDto>> PersonById(int personId)
        {
            var response = new BaseResponse<PersonResponseDto>();
            var personSelected = await _unitOfWork.Person.GetByIdAsync(personId);

            if (personSelected is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<PersonResponseDto>(personSelected);
                response.Message = RepplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSuccess = false;
                response.Message += RepplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterPerson(PersonRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var validationResult = await _personValidator.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_VALIDATE;
                response.Errors = validationResult.Errors;
                return response;
            }

            var person = _mapper.Map<Person>(requestDto);
            response.Data = await _unitOfWork.Person.RegisterAsync(person);

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

        public async Task<BaseResponse<bool>> EditPerson(int personId, PersonRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var personEdit = await PersonById(personId);

            if (personEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var person = _mapper.Map<Person>(requestDto);
            person.Id = personId;
            response.Data = await _unitOfWork.Person.EditAsync(person);

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

        public async Task<BaseResponse<bool>> RemovePerson(int personId)
        {
            var response = new BaseResponse<bool>();
            var person = await PersonById(personId);

            if (person.Data is null)
            {
                response.IsSuccess = false;
                response.Message = RepplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Person.RemoveAsync(personId);

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
    }
}
