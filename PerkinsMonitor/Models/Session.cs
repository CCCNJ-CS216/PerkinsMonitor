using System;

namespace PerkinsMonitor
{
	/// <summary>
	/// A session in the computer lab
	/// </summary>
	public class Session
	{
		public Student Student {get; set;}
		public int MachineNumber { get; set; }

		public DateTime TimeStarted { get; set; }

		private StudentDatabase Database;

		/// <summary>
		/// Initializes a new instance of the <see cref="PerkinsMonitor.Session"/> class.
		/// </summary>
		/// <param name="student">The student object to use</param>
		/// <param name="machineNumber">The machine being utilized by this student</param>
		public Session (Student student, int machineNumber, int timeIn)
		{
			Student = student;
			MachineNumber = machineNumber;

			TimeStarted = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
			TimeStarted = TimeStarted.AddSeconds(timeIn).ToLocalTime();
		}
	}
}