using System.Diagnostics;

namespace Common.ExtensionMethods
{
	public class EventLogger
	{
		private EventLogger() { }

		private const string NotificationServiceLogName = "Application";

		/// <summary>
		/// Write the message to the event log with event log entry type as Information
		/// </summary>
		/// <param name="serviceSource"></param>
		/// <param name="message"></param>
		public static void WriteEntry(string serviceSource, string message)
		{
			if (!EventLog.SourceExists(serviceSource))
			{
				EventLog.CreateEventSource(serviceSource, NotificationServiceLogName);
			}
			EventLog.WriteEntry(serviceSource, message, EventLogEntryType.Information);
		}

		/// <summary>
		/// Write the message to the event log with event log entry type as Error
		/// </summary>
		/// <param name="serviceSource"></param>
		/// <param name="message"></param>
		public static void WriteEntryOnException(string serviceSource, string message)
		{
			if (!EventLog.SourceExists(serviceSource))
			{
				EventLog.CreateEventSource(serviceSource, NotificationServiceLogName);
			}
			EventLog.WriteEntry(serviceSource, message, EventLogEntryType.Error);
		}
	}
}
