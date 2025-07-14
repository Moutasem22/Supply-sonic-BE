using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Attachments;

namespace Core.Models.Setting
{
    public class AppearancSetting : BaseEntity<int>
    {
        public string SystemColorAlpha { get; private set; }
        public string SystemColorHexa { get; private set; }
        public string SystemColorHex { get; private set; }

        public string ButtonsColorsAlpha { get; private set; }
        public string ButtonsColorsHexa { get; private set; }
        public string ButtonsColorsHex { get; private set; }

        public string Logo { get; private set; }
        public int? LogoAttachmentId { get; private set; }
        public Attachment LogoAttachment { get; private set; }
        public AppearancSetting(string systemColorAlpha, string systemColorHexa, string systemColorHex, string buttonsColorsAlpha, string buttonsColorsHexa, string buttonsColorsHex, string logo, int? logoAttachmentId)
        {
            SystemColorAlpha = systemColorAlpha;
            SystemColorHexa = systemColorHexa;
            SystemColorHex = systemColorHex;
            ButtonsColorsAlpha = buttonsColorsAlpha;
            ButtonsColorsHexa = buttonsColorsHexa;
            ButtonsColorsHex = buttonsColorsHex;
            Logo = logo;
            LogoAttachmentId = logoAttachmentId;
        }
        public void Update(string systemColorAlpha, string systemColorHexa, string systemColorHex, string buttonsColorsAlpha, string buttonsColorsHexa, string buttonsColorsHex, string logo, int? logoAttachmentId)
        {
            SystemColorAlpha = systemColorAlpha;
            SystemColorHexa = systemColorHexa;
            SystemColorHex = systemColorHex;
            ButtonsColorsAlpha = buttonsColorsAlpha;
            ButtonsColorsHexa = buttonsColorsHexa;
            ButtonsColorsHex = buttonsColorsHex;
            Logo = logo;
            LogoAttachmentId = logoAttachmentId;
        }
    }
}
