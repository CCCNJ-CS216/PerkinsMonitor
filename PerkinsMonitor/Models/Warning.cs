using System;

namespace PerkinsMonitor
{
	/// <summary>
	/// Provides a loadable warning for things such as forbidden access
	/// </summary>
	public class Warning
	{
		/// <summary>
		/// The actual body of the warning
		/// </summary>
		/// <value>The warning text</value>
		public string WarningText {get; set;}

		public Warning (string warningText)
		{
			WarningText = warningText;	
		}
	}
}

