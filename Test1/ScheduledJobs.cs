using Test1.Data;
using Test1.Interface;

namespace Test1
{
	public class ScheduledJobs
	{
		private readonly AppDbContext _context;
		private readonly IEmailSender _emailSender;

		public ScheduledJobs(AppDbContext context, IEmailSender emailSender)
		{
			_context = context;
			_emailSender = emailSender;
		}

		public async Task SendEndOfDayEmailAsync()
		{
			// Get all Names from DB
			var user_Names = _context.User.Select(u => u.Name).ToList();

			// Counter for emails sent
			int emailsSentCount = 0;

			// Calculate view count 
			foreach (var name in user_Names)
			{
				var count = 0;
				var data = _context.User.FirstOrDefault(u => u.Name == name);
				if (data != null)
					count = (int)data.ViewersCount;
				else
					count = -1;

				if (count > 0)
				{
					// Create the email content
					var emailBody = "Today's view count for " + name + " is " + count + ". " + System.Environment.NewLine + System.Environment.NewLine + "CodeCraftCrew";

					// Send the email
					await _emailSender.SendEmailAsync(data.Email, "End of Day Report", emailBody);

					// Increment the count of emails sent
					emailsSentCount++;
				}
			}

			// Log the count of emails sent for the day into the database
			var logEntry = new EmailLog
			{
				LogDate = DateTime.UtcNow, // Use UTC time for consistency
				EmailsSentCount = emailsSentCount
			};

			_context.EmailLog.Add(logEntry);
			await _context.SaveChangesAsync();

		}

		public void Scheduler()
		{
			DateTime now = DateTime.Now;
			DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, 23, 59, 23);

			if (now > scheduledTime)
			{
				scheduledTime = scheduledTime.AddDays(1);
			}

			TimeSpan timeUntilScheduled = scheduledTime - DateTime.Now;

			Timer timer = new Timer(_ =>
			{
				// Execute the function when the timer elapses
				SendEndOfDayEmailAsync();
				Scheduler();
			}, null, (int)timeUntilScheduled.TotalMilliseconds, Timeout.Infinite);
		}
	}
}