using AutoMapper.Internal;
using MailKit.Net.Smtp;
using MailKit.Security;
using WebService.API.Helpers;

namespace WebService.API.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}