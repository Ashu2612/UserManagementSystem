using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace UserManagementSystem
{
    public class CommonClass
    {
        public static string connectionString = "Server=CQMPRDTNE01\\MSSQLSERVER01;Database=UserManagementSystem;Integrated Security=true;";

        public static bool ErrorLogging(string exMessage)
        {
            try
            {
                string appPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

                string directoryPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string filePath = System.IO.Path.Combine(directoryPath, "Errorlog.txt");
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{DateTime.Now} - {exMessage}");
                }
            }
            catch
            {
                MessageBox.Show("Error logging has failed.");
                return false;
            }
            return true;
        }
        public static bool ValidateEmailFormat(string email)
        {
            // Regular expression pattern for email format validation
            string emailPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            // Check if the email matches the pattern
            bool isValidEmail = Regex.IsMatch(email, emailPattern);
            return isValidEmail;
        }

        public static string GetHomePath()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return System.Environment.GetEnvironmentVariable("HOME");

            return System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }
        public static string GetDownloadFolderPath()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                string pathDownload = System.IO.Path.Combine(GetHomePath(), "Downloads");
                return pathDownload;
            }

            return Convert.ToString(
                Registry.GetValue(
                     @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    , "{374DE290-123F-4565-9164-39C4925E467B}"
                    , String.Empty
                )
            );
        }

        public static bool SQLToCSV(string query, string filename)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader dr = cmd.ExecuteReader();

            using (StreamWriter fs = new StreamWriter(filename))
            {
                // Loop through the fields and add headers
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string name = dr.GetName(i);
                    if (name.Contains(","))
                        name = "\"" + name + "\"";

                    fs.Write(name + ",");
                }
                fs.WriteLine();

                // Loop through the rows and output the data
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(","))
                            value = "\"" + value + "\"";

                        fs.Write(value + ",");
                    }
                    fs.WriteLine();
                }
            }

            // Convert the CSV file to XLSX
            string xlsxFilePath = Path.ChangeExtension(filename, ".xlsx");
            ConvertCSVToXLSX(filename, xlsxFilePath);

            // Optional: Delete the original CSV file
            File.Delete(filename);
            return true;
        }
        private static void ConvertCSVToXLSX(string csvFilePath, string xlsxFilePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Data");

                using (var reader = new StreamReader(csvFilePath))
                {
                    string line;
                    int row = 1;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var values = line.Split(',');

                        for (int col = 0; col < values.Length; col++)
                        {
                            worksheet.Cell(row, col + 1).Value = values[col];
                        }

                        row++;
                    }
                }

                workbook.SaveAs(xlsxFilePath);
            }
        }
    }
}
