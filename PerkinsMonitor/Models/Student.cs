using System;

namespace PerkinsMonitor
{
	/// <summary>
	/// Represents a student
	/// </summary>
	public class Student
	{
		public int ID {get; set;}
		public string Major { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public Student(int ident, string major) {
			ID = ident;
			Major = major;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerkinsMonitor.Student"/> class.
		/// </summary>
		/// <param name="ID">I.</param>
		/// <param name="major">Major.</param>
		/// <param name="name">Name.</param>
		public Student (int ident, string major, string name) : this (ident, major)
		{
			ID = ident;
			Major = major;

			try {
				FirstName = name.Split (' ') [0];
				LastName = name.Split (' ') [1];
			} catch(Exception e) {
				FirstName = name;
				LastName = "";
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerkinsMonitor.Student"/> class.
		/// This version is explicit with first and last names
		/// </summary>
		/// <param name="Ident">Ident.</param>
		/// <param name="major">Major.</param>
		/// <param name="first">First.</param>
		/// <param name="last">Last.</param>
		public Student (int Ident, string major, string first, string last) : this(Ident, major) {
			FirstName = first;
			LastName = last;
		}

		public string ToJSON()
		{
			string json = "";
			try { // Notice {{ and }}. This is important because JSON needs { } and so does format. 2 escape this.
				json = String.Format ("{{\"studentID\": \"{0}\", \"major\": \"{1}\", \"name\": \"{2}\"}}",
					ID,
					Major,
					FirstName+" "+LastName);
			} catch(Exception e) {
				Console.WriteLine ("String formatting error. Possible SQL-injection detected");
			}
			return json;
		}
	}
}

