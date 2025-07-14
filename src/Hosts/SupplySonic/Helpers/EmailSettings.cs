using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class EmailSettings
    {
        string _MailServer;
        string _SenderName;
        string _Sender;
        string _Password;
        string _MailBBC;
        string _ToEmail;
        public string MailServer
        {
            get
            {
                return _MailServer;
            }
            set
            {
                _MailServer = value;
            }
        }
        public int MailPort
        {
            get;
            set;
        }

        public string SenderName
        {
            get
            {
                return _SenderName;
            }
            set
            {
                _SenderName = value;
            }
        }

        public string Sender
        {
            get
            {
                return _Sender;
            }
            set
            {
                _Sender = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        public string MailBBC
        {
            get
            {
                return _MailBBC;
            }
            set
            {
                _MailBBC = value;
            }
        }
        public string ToEmail
        {
            get
            {
                return _ToEmail;
            }
            set
            {
                _ToEmail = value;
            }
        }
        public bool EnableSsl
        {
            get;
            set;
        }       
    }
}
