using Core.Enums;
using Core.Models;
using DB;
using DTO;
using Helpers;
using IServiceContractor;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class UserNotificationService : IUserNotificationService
    {
        private DBContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string lang { get; set; }
        public UserNotificationService(DBContext dBContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = dBContext;
            _httpContextAccessor = httpContextAccessor;

            TypeAdapterConfig<UserNotification, UserNotificationDTO>.NewConfig()

             .Map(dest => dest.Message, src => src.Notification.MessageAr)
             .Map(dest => dest.MessageEn, src => src.Notification.Message)
             .Map(dest => dest.Subject, src => src.Notification.SubjectAr)
             .Map(dest => dest.SubjectEn, src => src.Notification.Subject)
             .Map(dest => dest.URL, src => src.Notification.URL);
        }
        public ResultViewModel<List<UserNotificationDTO>> GetAll(QueryViewModel<UserNotificationDTO> queryViewModel, int userId)
        {
            var PagedDataResult = new ResultViewModel<List<UserNotificationDTO>>();

            var query = _dbcontext.UserNotifications.Include(x=>x.Notification).OrderByDescending(x => x.Id).Where(r => r.IsActive == true && r.IsDeleted != true && r.AppUserId == userId && r.Notification.NotificationType == EnumNotificationType.Web).Include(x => x.Notification) as IQueryable<UserNotification>;

            queryViewModel.Filter.ToList().ForEach(x =>
            {
                switch (x.Operation)
                {
                    case (FilterOperation.Equal):
                        if (x.FieldName.ToLower() == "rolename")
                        {
                            //query = query.Where(r => r.AppUser.FirstName==x.value);
                        }
                        break;
                }
            });

            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("name"):
                        //query = query.OrderBy(x => x.NameAr);
                        break;
                    default:
                        //query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("name"):
                        //query = query.OrderByDescending(x => x.NameAr);
                        break;
                    default:
                        //query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }

            var Total = query.Count();

            var userNotifications = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            var UnReadNumber = _dbcontext.UserNotifications.Count(r => r.IsActive == true && r.IsDeleted != true && r.AppUserId == userId
            && r.NotificationState == EnumNotificationState.New && r.Notification.NotificationType == EnumNotificationType.Web);

            List<UserNotificationDTO> userNotificationDtos = userNotifications.Adapt<List<UserNotificationDTO>>();
            PagedDataResult.Messages.Add(new MessageModel() { InputName = "UnReadNumber", Message = UnReadNumber.ToString() });
            PagedDataResult.Data = userNotificationDtos;
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;
        }

        public bool ResetCounter(int userId)
        {
            try
            {
                var newNotificationList = _dbcontext.UserNotifications.Where(x => x.AppUserId == userId && x.IsActive != false && x.IsDeleted != true
                  && x.NotificationState == EnumNotificationState.New && x.Notification.NotificationType == EnumNotificationType.Web);
                newNotificationList.ToList().ForEach(x =>
                {
                    x.NotificationState = EnumNotificationState.Seen;
                    _dbcontext.UserNotifications.Update(x);
                }

               );
                _dbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool MakeNotifyRead(int userId, int UserNotificationId)
        {
            try
            {
                var unotification = _dbcontext.UserNotifications.FirstOrDefault(x => x.AppUserId == userId && x.Id == UserNotificationId);
                if (unotification != null)
                {
                    unotification.NotificationState = EnumNotificationState.Read;
                    _dbcontext.UserNotifications.Update(unotification);
                }
                _dbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public ResultViewModel<List<UserNotificationDTO>> GetAllNotification(QueryViewModel<UserNotificationDTO> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<UserNotificationDTO>>();

            var query = _dbcontext.Notifications.Where(r => r.IsActive && !r.IsDeleted).ToList();

            //queryViewModel.Filter.ToList().ForEach(x =>
            //{
            //    switch (x.Operation)
            //    {
            //        case (FilterOperation.Equal):
            //            if (x.FieldName.ToLower() == "rolename")
            //            {
            //                //query = query.Where(r => r.AppUser.FirstName==x.value);
            //            }
            //            break;
            //    }
            //});

            //if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            //{
            //    switch (queryViewModel.Order.FieldName.ToLower())
            //    {
            //        case ("name"):
            //            //query = query.OrderBy(x => x.NameAr);
            //            break;
            //        default:
            //            //query = query.OrderBy(x => x.Id);
            //            break;
            //    }
            //}
            //else
            //{
            //    switch (queryViewModel.Order.FieldName.ToLower())
            //    {
            //        case ("name"):
            //            //query = query.OrderByDescending(x => x.NameAr);
            //            break;
            //        default:
            //            //query = query.OrderByDescending(x => x.Id);
            //            break;
            //    }
            //}

            var Total = query.Count();

            var userNotifications = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            var UnReadNumber = _dbcontext.UserNotifications.Count(r => r.IsActive == true && r.IsDeleted != true
            && r.NotificationState == EnumNotificationState.New);

            List<UserNotificationDTO> userNotificationDtos = userNotifications.Adapt<List<UserNotificationDTO>>();
            PagedDataResult.Messages.Add(new MessageModel() { InputName = "UnReadNumber", Message = UnReadNumber.ToString() });
            PagedDataResult.Data = userNotificationDtos;
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;
        }

    }
}
