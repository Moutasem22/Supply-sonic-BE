using DTO.CommandDTO;
using DTO.SettingDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor.ISettingServices
{
    public interface IAppearanceSettingsService
    {
        Task<ResultViewModel<AppearanceResultDto>> Update(AppearanceAddEditDto AppearanceDto);
        Task<ResultViewModel<AppearanceResultDto>> GetCurrent();
    }
}
