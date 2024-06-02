using Settings;
using System.Net;
using System.Net.Mail;

namespace Server.Components;

internal static class EmailSender
{
	private static readonly SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

	static EmailSender()
	{
		smtpClient.UseDefaultCredentials = false;
		smtpClient.Port = 587;
		smtpClient.Credentials = new NetworkCredential(Configuration.EmailHost, Configuration.EmailPassword);
		smtpClient.EnableSsl = true;
	}

	public static bool SendEmail(string email, string subject, string body)
	{
		try
		{
			smtpClient.Send(Configuration.EmailHost, email, subject, body);
			return true;
		}
		catch (Exception ex) { return false; }
	}
}