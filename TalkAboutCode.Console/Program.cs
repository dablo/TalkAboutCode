using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace TalkAboutCode.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var employeeInfo = new EmployeeInfo();
            employeeInfo.Print();
        }
    }

    public class EmployeeInfo
    {
        public string connectionString;

        public struct MyDbRow
        {
            internal string Column1;
            internal int Column2;

            public MyDbRow(string col1, int col2)
            {
                Column1 = col1;
                Column2 = col2;
            }
        }

        public void Print()
        {
            List<MyDbRow> res = new List<MyDbRow>();
            var dbResult = new List<MyDbRow>();

            try
            {
                // Querying EmployeeTable
                var connection = new SqlConnection(connectionString);
                connection.Open();
                var myCommand = new SqlCommand("SELECT Name, Salary FROM EmployeeTable", connection);
                var myReader = myCommand.ExecuteReader();
                //Iterate over every row
                while (myReader.Read())
                {
                    //Get column=0 and column=1
                    var row = new MyDbRow(myReader.GetString(0), myReader.GetInt32(1));
                    dbResult.Add(row);
                }
                //Extract stuff of interest
                res = dbResult.Where(x => x.Column1.StartsWith("Lars") && x.Column2 > 10000).ToList();
            }
            finally
            {
                IEnumerable<string> rslt = (res.Count == 0)
                    ? new string[] { "No one found" }
                    : res.Select(x => x.Column1 + x.Column2);
                foreach (var item in rslt)
                {
                    System.Console.WriteLine(item);
                }
            }
        }
    }
}
