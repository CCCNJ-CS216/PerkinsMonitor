using System;

using System.Collections.Generic;
namespace PerkinsMonitor
{
	public class ValidationRequest
	{
		/// <summary>
		/// Where to use the information
		/// </summary>
		/// <value>The target</value>
		public string Target { get; set;}

		/// <summary>
		/// The parameters that still need to be filled out
		/// </summary>
		/// <value>The required parameters</value>
		public List<string> RequiredParameters { get; set; }

		public ValidationRequest (string target, params string[] required)
		{
			Target = target;
			RequiredParameters = new List<string> ();
			RequiredParameters.AddRange (required);
		}
	}
}

