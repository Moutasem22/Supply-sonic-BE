using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class SysSettingsService
    {
        private AppSettings _appSettings;
        private readonly RestHelper _restHelper;
        public string ServiceUrl { get; private set; }

        public SysSettingsService(IOptions<AppSettings> appSettings, RestHelper restHelper)
        {
            _appSettings = appSettings.Value;
            _restHelper = restHelper;
            ServiceUrl = _appSettings.APIEndPoint;
        }

        public string GetAppSetting(string key, string _serviceUrl = "")
        {
            try
            {
                ServiceUrl = string.IsNullOrWhiteSpace(_serviceUrl) ? ServiceUrl : _serviceUrl;
                var accessToken = string.Empty;
                List<Tuple<string, string>> Headers = null;

                var res = _restHelper.Get<string>(ServiceUrl,
                        "api/SysSettings/GetAppSetting?key=" + key, headers: Headers);
                return res.Result;
            }
            catch
            {
                return "";
            }
        }
    }
}
