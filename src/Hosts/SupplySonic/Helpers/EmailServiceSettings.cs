using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers;

public class EmailServiceSettings
{
    public int Port { get; set; }
    public string? HostName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? VirtualHost { get; set; }
    public string? ProjectCode { get; set; }
}
