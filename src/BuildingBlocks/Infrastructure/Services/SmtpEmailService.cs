using Contracts.Service;
using Shared.Services;
using Serilog;
using MailKit.Net.Smtp;
using Contracts.Configurations;
using Contracts.Services;
using Infrastructure.Configurations;
using MimeKit;

namespace Infrastructure.Services
{
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly ILogger _logger;
        private readonly IEmailSettings _settings;
        private readonly SmtpClient _smtpClient;

        public SmtpEmailService(ILogger logger, SMTPEmailSetting settings)
        {
            _logger = logger;
            _settings = settings ?? throw new ArgumentNullException(nameof(_settings));
            _smtpClient = new SmtpClient();
        }

        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.DisplayName, request.From ?? _settings.From),
                Subject = request.Subject,
                Body = new BodyBuilder()
                {
                    HtmlBody = request.Body
                }.ToMessageBody(),
            };

            if (request.ToAddresses.Any())
            {
                foreach (var address in request.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(address));
                }
            }
            else
            {
                emailMessage.To.Add(MailboxAddress.Parse(request.ToAddress));
            }

            try
            {
                await _smtpClient.ConnectAsync(_settings.SmtpServer, _settings.Port, _settings.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }

        }
    }
}
