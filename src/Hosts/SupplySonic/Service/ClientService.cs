using Core.Enums;
using Core.Models;
using DB;
using DTO;
using DTO.CommandDTO;
using FluentValidation;
using Helpers;
using IServiceContractor;
using IServiceContractor.ICommonService;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ClientService : IClientService
    {
        private readonly DBContext _dBContext;
        private readonly IBaseService _baseService;
        private readonly IValidator<ClientAddEditDto> _validator;
   
        public ClientService(DBContext dBContext, IBaseService baseService, IValidator<ClientAddEditDto> validator)
        {
            _dBContext = dBContext;
            _baseService = baseService;
            _validator = validator;

            TypeAdapterConfig<Client, ClientResultDto>.NewConfig()
                .Map(dest => dest.ClientName, src => baseService.Language == "ar" ? src.NameAr : src.NameEn ?? null);
        }
        public async Task<ResultViewModel<ClientResultDto>> Add(ClientAddEditDto lockupDto)
        {
            _baseService.CheckValidation(lockupDto, _validator);

            Client lockup = new(lockupDto.NameAr, lockupDto.NameEn, lockupDto.PhoneNumber, lockupDto.Email);
            await _dBContext.Clients.AddAsync(lockup);
            await _dBContext.SaveChangesAsync();
            _baseService.sendActionNotification(EnumPageCode.Client, EnumActionCode.add, lockup.Id);

            ResultViewModel<ClientResultDto> result = new() { Data = lockup.Adapt<ClientResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<ClientResultDto>> Delete(int id)
        {
            var lockup = _dBContext.Clients.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
            if (lockup == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "Client" };
               _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
            }

            lockup.Delete();

            await _dBContext.SaveChangesAsync();
            _baseService.sendActionNotification(EnumPageCode.Client, EnumActionCode.delete, lockup.Id);

            ResultViewModel<ClientResultDto> result = new() { Data = lockup.Adapt<ClientResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<List<ClientResultDto>>> GetAll(QueryViewModel<ClientAddEditDto> queryViewModel)
        {
            IQueryable<Client> query = _dBContext.Clients.AsNoTracking().Where(s => s.IsActive && !s.IsDeleted);
       
            queryViewModel.Filter.ToList().ForEach(x =>
            {
                      switch (x.Operation)
                        {
                            case (FilterOperation.Equal):
                                if (x.FieldName.ToLower() == "name")
                                    query = query.Where(r => (r.NameAr.ToLower().Contains(x.value.Trim().ToLower())
                                                            || r.NameEn.ToLower().Contains(x.value.Trim().ToLower())));
                                else if (x.FieldName.ToLower() == "phonenumber")
                                {
                                    query = query.Where(r => (r.PhoneNumber.ToLower().Contains(x.value.Trim().ToLower().ToLower())));
                                }
                                else if (x.FieldName.ToLower() == "email")
                                {
                                    query = query.Where(r => (r.Email.ToLower().Contains(x.value.Trim().ToLower().ToLower())));
                                }

                        break;
                        
                            }
                     });

            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("namear"):
                        query = query.OrderBy(x => x.NameAr);
                        break;
                    case ("nameen"):
                        query = query.OrderBy(x => x.NameEn);
                        break;
                    case ("phonenumber"):
                        query = query.OrderBy(x => x.PhoneNumber);
                        break;
                    case ("email"):
                        query = query.OrderBy(x => x.Email);
                        break;
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("namear"):
                        query = query.OrderByDescending(x => x.NameAr);
                        break;
                    case ("nameen"):
                        query = query.OrderByDescending(x => x.NameEn);
                        break;
                    case ("phonenumber"):
                        query = query.OrderByDescending(x => x.PhoneNumber);
                        break;
                    case ("email"):
                        query = query.OrderByDescending(x => x.Email);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }
            var lockup = queryViewModel.PageSize == 0 ? await query.ToListAsync() : await query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize).ToListAsync();
            ResultViewModel<List<ClientResultDto>> PagedDataResult = new()
            {
                Data = lockup.Adapt<List<ClientResultDto>>(),
                PageSize = queryViewModel.PageSize,
                PageNumber = queryViewModel.PageNumber,
                Total = query.Count(),
                IsSuccess = true
            };
            return PagedDataResult;
        }

        public async Task<ResultViewModel<ClientResultDto>> Getone(int id)
        {
            var query = await _dBContext.Clients.Where(n => n.Id == id && !n.IsDeleted).FirstOrDefaultAsync();

            if (query == null)
            {
                ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
                _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
            }
            ResultViewModel<ClientResultDto> result = new() { Data = query.Adapt<ClientResultDto>(), IsSuccess = true };
            return result;
        }

        public async Task<ResultViewModel<ClientResultDto>> Update(ClientAddEditDto lockupDto)
        {
            _baseService.CheckValidation(lockupDto, _validator);
            Client lockup = await _dBContext.Clients.FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted);

            lockup.Update(lockupDto.NameAr, lockupDto.NameEn, lockupDto.PhoneNumber, lockupDto.Email);
            await _dBContext.SaveChangesAsync();
            _baseService.sendActionNotification(EnumPageCode.Client, EnumActionCode.update, lockup.Id);

            ResultViewModel<ClientResultDto> result = new() { Data = lockup.Adapt<ClientResultDto>(), IsSuccess = true };
            return result;
        }
    }
}
