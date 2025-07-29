
using PathbitLevel2Challenge.Domain.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PathbitLevel2Challenge.Infrastructure.Services;

public class EmailSenderService
{
  private readonly ISendGridClient _sendGridClient;

  public EmailSenderService(ISendGridClient sendGridClient)
  {
    _sendGridClient = sendGridClient;
  }

  public async Task SendEmailAsync(IEmailMessage message)
  {
    var from = new EmailAddress("no-reply@pathbit.com", "Equipe PATHBIT");
    var to = new EmailAddress(message.Email, message.Name);
    var subject = "Cadastro em Análise";
    var content = $@"Olá {message.Name},
Olá {{nome}},

O seu cadastro está em análise e em breve você receberá um e-mail com novas atualizações sobre seu cadastro.

Atenciosamente,
Equipe PATHBIT";

    var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
    await _sendGridClient.SendEmailAsync(msg);
  }
}
