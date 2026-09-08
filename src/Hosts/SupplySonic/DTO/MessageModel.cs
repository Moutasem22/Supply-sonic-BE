using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{

    public class MessageModel
    {
        public MessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }
        public string InputName { get; set; }
    }
    public enum MessageTypeEnum
    {
        Error = 0,
        Success = 1

    }
}
