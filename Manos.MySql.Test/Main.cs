using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
//using System.Security.Cryptography;
using System.Linq;

using Manos.IO;
using Manos.MySql;

using Epoch.Extensions;

namespace Manos.MySql.Test
{
	public static class MysqlExtensions
	{
		// Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		
		public static string ToDate(this DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-dd");
		}
	}
	
	class MainClass
	{
		static string host     = "127.0.0.1";
		static int    port     = 3306;
		static string user     = "test";
		static string password = "";
		
		public static void Query(Context context, string database, string table)
		{
			MySqlConnection conn = new MySqlConnection(context.CreateSocket());
			
			conn.Connect(host, port, user, password, delegate (Exception e) {
				conn.Query(string.Format("use {0}", database), (response) => {
					conn.Query(string.Format("SELECT * FROM {0}", table))
					.On(row: delegate (Row data) {
						for (int i = 0; i < data.Length; i++) {
							Console.Write(data.GetRawValue(i));
							Console.Write("\t");
						}
						Console.WriteLine();
					}).On(end: delegate {
						conn.Close();
					});
				});
			});
		}
		
		public static void Main(string[] args)
		{
			var context = Context.Create();
			
			Query(context, args[0], args[1]);
			
			context.Start();
		}
	}
}
