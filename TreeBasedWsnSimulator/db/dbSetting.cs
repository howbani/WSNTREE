using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeBasedWsnSimulator.db
{
  
	public class DbSetting  
	{

        public static string DATABASENAME = "db";
        public static string DATABASEPASSWORD = "_12_LG1705504004";
        public static string ConnectionString = ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DATABASENAME + ".accdb;Jet OLEDB:Database Password=" + DATABASEPASSWORD + ";";

	}
}
