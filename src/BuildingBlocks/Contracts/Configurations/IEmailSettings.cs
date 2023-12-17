using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Configurations
{
    public interface IEmailSettings
    {
        string DisplayName { get; set; }
        bool EnableVerification { get; set; }
        string From { get; set; }
        string SmtpServer { get; set; }
        bool UseSsl { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}
