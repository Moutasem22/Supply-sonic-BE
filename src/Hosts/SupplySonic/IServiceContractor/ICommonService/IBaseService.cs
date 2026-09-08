using Core.Enums;
using Core.Models.Identity;
using DB;
using DTO;
using FluentValidation;
using Helpers;
using Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor.ICommonService
{
    public interface IBaseService
    {
        string GetCurrentLanguage();
        DBContext GetContext();
        IStringLocalizer<SharedResource> GetLocalizer();
        void Validate<T>(T dto, IValidator<T> validator);
        IExceptionMessages GetExceptionMessages();

        DBContext Context { get; }
        IStringLocalizer<SharedResource> Localizer { get; }

        string Language { get; }
        int CurrentUserId { get; }
        string CurrentUrlPath { get; }

        IExceptionMessages ExceptionMessages { get; }

        //IPasswordHasher<AppUser> PasswordHasher { get; }
        //SignInManager<AppUser> SignInManager { get; }

        IHttpContextAccessor HttpContextAccessor { get; }

        IHostEnvironment HostEnvironment { get; }

        void CheckValidation<T>(T dto, IValidator<T> validator);

        void sendActionNotification(EnumPageCode pageCode, EnumActionCode actionCode, int objId = 0);

    }
}
