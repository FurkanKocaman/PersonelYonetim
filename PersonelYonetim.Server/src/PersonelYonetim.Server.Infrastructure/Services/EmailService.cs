using Microsoft.Extensions.Configuration;
using PersonelYonetim.Server.Application.Services;
using System.Net;
using System.Net.Mail;

namespace PersonelYonetim.Server.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;
    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = configuration["EmailSettings:Email"];
        var password = configuration["EmailSettings:Password"];
        var host = configuration["EmailSettings:Host"];
        var port = configuration.GetValue<int>("EmailSettings:Port");

        var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(email, password),
            EnableSsl = true,
        };
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(email!),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);
        smtpClient.SendMailAsync(mailMessage);

        return Task.CompletedTask;
    }
}
