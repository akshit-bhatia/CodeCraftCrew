namespace Test1.Email;
using System.Net;
using System.Net.Mail;
using Test1.Interface;

public class EmailSender : IEmailSender
{
	public Task SendEmailAsync(string email, string subject, string message)
	{
		var senderId = "your outlook email";
		var password = "your password";
		var client = new SmtpClient("smtp.office365.com", 587)
		{
			EnableSsl = true,
			UseDefaultCredentials = false,
			Credentials = new NetworkCredential(senderId, password)
		};

		return client.SendMailAsync(
			new MailMessage(from: senderId,
							to: email,
							subject,
							message
							));
	}
}