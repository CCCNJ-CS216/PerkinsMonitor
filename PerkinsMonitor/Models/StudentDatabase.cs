using System;

using HumDrum.Collections;
using NLog;

using System.Collections.Generic;

using Mono.Data.Sqlite;

namespace PerkinsMonitor
{
	/// <summary>
	/// A class that makes accessing information in the database easier
	/// </summary>
	public class StudentDatabase
	{
		/// <summary>
		/// The database file
		/// </summary>
		private string DBFile = "/home/nate/perkins.sqlite";

		/// <summary>
		/// The actual database
		/// </summary>
		/// <value>The database</value>
		private SqliteConnection Database { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="PerkinsMonitor.StudentDatabase"/> class.
		/// Uses the default filepath for the dbFile
		/// </summary>
		public StudentDatabase ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PerkinsMonitor.StudentDatabase"/> class.
		/// This will use a different file for the database
		/// </summary>
		/// <param name="dbFile">Db file.</param>
		public StudentDatabase (string file)
		{
			DBFile = file;
		}

		/// <summary>
		/// Connect to the database
		/// </summary>
		public void Connect ()
		{
			try {
				Database = new SqliteConnection ("Data Source=" + DBFile + ";Version=3;");
				Database.Open ();
			} catch (Exception e) {
				Console.WriteLine ("Failure");
			}

		}

		/// <summary>
		/// Close the connection to the database
		/// </summary>
		public void Disconnect ()
		{
			Database.Close ();
			GC.Collect ();//http://stackoverflow.com/questions/8511901/system-data-sqlite-close-not-releasing-database-file
		}

		/// <summary>
		/// Attempts to get a student from the database
		/// </summary>
		/// <returns>The student</returns>
		/// <param name="ID">The student with this ID. Otherwise, null.</param>
		public Student RetrieveStudent (int ID)
		{
			string sql = "SELECT * FROM cachedStudents WHERE studentID = " + ID;

			SqliteDataReader reader = ExecuteRawSQL (sql);

			if (reader.VisibleFieldCount < 4) // Student ID, Major, First, Last is the "magic" 4 here
				return null;
			else {
				reader.Read ();

				try {
					int studentID = reader.GetInt32 (0);
					string major = reader.GetString (3);
					string first = reader.GetString (1);
					string last = reader.GetString (2);

					return new Student (
						studentID,
						major,
						first,
						last);
				} catch (Exception e) {
					Console.WriteLine ("Corrupted Record for " + ID + " detected. Manual intervention required.");
				}

				return null;
			}
		}

		/// <summary>
		/// See how many students are signed in right now
		/// </summary>
		/// <returns>The students who are signed in</returns>
		public IEnumerable<Session> SignedInStudents ()
		{
			SqliteDataReader reader = ExecuteRawSQL ("SELECT * FROM loggedIn;");

			while (reader.Read ()) {
				yield return new Session (
					RetrieveStudent (reader.GetInt32 (0)),
					reader.GetInt32 (2),
					reader.GetInt32 (1));
			}

			yield break;
		}

		/// <summary>
		/// Signs in the student
		/// </summary>
		/// <param name="ID">The Student ID for the student</param>
		/// <param name="first">The student's first name</param>
		/// <param name="last">The student's last name</param>
		/// <param name="major">The student's major</param>
		/// <param name="machine">The machine that the student is using</param>
		public void SignIn (int ID, string first, string last, string major, int machine, int timeIn)
		{
			try {
				string sql = "";

				// Delete the old record, if it exists.
				sql += "DELETE FROM cachedStudents where studentID = " + ID + "; ";

				// Replace it with the new record
				sql += String.Format(
					"INSERT INTO cachedStudents (studentID, first, last, major) VALUES ('{0}', '{1}', '{2}', '{3}');",
					ID,
					first,
					last,
					major);

				ExecuteRawSQL (sql);
			} catch (Exception e) {
				Console.WriteLine ("Update of cached student record has failed");
			}

			try {
				ExecuteRawSQL (String.Format ("INSERT INTO loggedIn (studentID, timeIn, machineNumber) VALUES ('{0}', '{1}', '{2}');",
					ID,
					timeIn,
					machine
				));
			} catch (Exception e) {
				Console.WriteLine ("Sign in of student failed");
			}
		}

		/// <summary>
		/// Gets the session for a student who is currently logged in.
		/// </summary>
		/// <returns>The session that the student is partaking in</returns>
		/// <param name="ID">The ID for the student</param>
		public IEnumerable<Session> GetSession(int ID)
		{
			Student inQuestion = RetrieveStudent (ID);

			string sql = String.Format ("SELECT * FROM loggedIn where studentID = {0};", ID);
			var reader = ExecuteRawSQL (sql);

			while(reader.Read())
				yield return new Session (inQuestion, reader.GetInt32 (2), reader.GetInt32 (1));

			yield break;
		}

		/// <summary>
		/// Signs the student out of the database, by moving this session into the history table and
		/// removing them from loggedIn
		/// </summary>
		/// <param name="ID"></param>
		public void SignOut(int ID)
		{
			string sql = "";
			Session latestSession = GetSession (ID).Get (0);

			GC.Collect ();
			sql += String.Format ("DELETE FROM loggedIn WHERE studentID = {0};", ID);
			ExecuteRawNonQuery (sql);

			sql = String.Format ("INSERT INTO history (studentID, timeIn, timeOut) VALUES('{0}', '{1}', '{2}');",
				ID,
				latestSession.TimeStarted,
				DateTime.Now);

			ExecuteRawNonQuery (sql);
		
		}
		/// <summary>
		/// Performs a raw SQL command on the database, returning the reader
		/// </summary>
		/// <returns>The reader for this query</returns>
		/// <param name="sql">The string representing the query</param>
		public SqliteDataReader ExecuteRawSQL (string sql)
		{
			try {
				return new SqliteCommand (sql, Database).ExecuteReader();
			} catch(Exception e) {
				return null;
			}
		}

		public void ExecuteRawNonQuery(string sql)
		{
			SqliteCommand command = new SqliteCommand(sql, Database);
			command.ExecuteNonQuery ();
			Console.WriteLine ("Query Executed - Return Value Ignored");
		}
	}
}

