namespace Test1.Email;
using System.Net;
using System.Net.Mail;
using Test1.Interface;

public class EmailSender : IEmailSender
{
	public Task SendEmailAsync(string email, string subject, string message)
	{
		var senderId = "codecraftcrew23@outlook.com";
		var password = "Windsor@123";
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