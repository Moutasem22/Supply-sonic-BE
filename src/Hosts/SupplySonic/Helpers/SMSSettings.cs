using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class SMSSettings
    {
        string _SMSAPI;
        string _Appsid;
        string _Sender;
        public string SMSAPI
        {
            get
            {
                return _SMSAPI;
            }
            set
            {
                _SMSAPI = value;
            }
        }
        public string Appsid
        {
            get
            {
                return _Appsid;
            }
            set
            {
                _Appsid = value;
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
    }
}
