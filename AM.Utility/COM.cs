using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Collections;
using Newtonsoft.Json;

public static class SettingsKey
{
    public const string _user_id = "user_id";
    public const string _designation = "designation";
    public const string _department = "department";
    public const string _role = "role";
    public const string _location = "location";

    public const string _projectId = "project_id";
    public const string _year = "year";
    public const string _rfinumber_format = "rfinumber_format";
    public const string _pgt_jobnumber = "pgt_jobnumber";
    public const string _project_name = "project_name";
    public const string _tekla_version = "tekla_version";
}

public static class FileExtension
{
    public const string Jpg = ".jpg";
    public const string Pdf = ".pdf";
}

public class COM
{

    #region Math
    public static int GenerateRandomNo()
    {
        int _min = 1000;
        int _max = 9999;
        Random _rdm = new Random();
        return _rdm.Next(_min, _max);
    }
    public static string GenerateRandomAlpha()
    {
        Random random = new Random();

        // ASCII values for lowercase letters (97 to 122)
        // You can use uppercase letters (65 to 90) by changing the ASCII range
        int minAscii = 65; // ASCII value for 'a'
        int maxAscii = 90; // ASCII value for 'z'

        // Generate three random letters
        char letter1 = (char)random.Next(minAscii, maxAscii + 1);
        char letter2 = (char)random.Next(minAscii, maxAscii + 1);
        char letter3 = (char)random.Next(minAscii, maxAscii + 1);

        // Combine the letters into a string
        return $"{letter1}{letter2}{letter3}";
    }
    public static double Percent(double percent, double val) => val * percent / 100;
    #endregion

    #region Valid Check
    public static bool IsValidID(int? num) => num != null && num > 0 ? true : false;
    public static bool IsNull<T>(T obj) where T : class
    {
        if (obj is null) return true;
        else return false;
    }
    public static bool IsAny<T>(IEnumerable<T> data)
    {
        return data != null && data.Any();
    }
    public static bool IsNullOrEmpty(string str) => string.IsNullOrEmpty(str) ? true : false;
    public static bool IsValidCount(int? count)
    {
        if (count == null) return false;
        return count > 0;
    }
    #endregion

    #region FilesOperation
    public static bool IsFileExist(string fileName) => System.IO.File.Exists(fileName) ? true : false;
    public static void FileDelete(string fileName) => File.Delete(fileName);
    public static void CopyTo(string fileName, string destination) => File.Copy(fileName, destination);
    public static string PathCombine(string fn1, string fn2) => Path.Combine(fn1, fn2);
    #endregion

    #region DateTime
    public const string DateTimeFormat = "MM/dd/yyyy HH:mm:ss";
    public static DateTime GetFormatedDate(DateTime date)
    {
        date = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
        return date;
    }
    public static DateTime GetFormatedDate(string date)
    {
        DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", null);
        return dt;
    }
    public static string GetCusomizedDate(string date)
    {
        DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy", null);
        return dt.ToString("yyyy-MM-dd");
    }
    public static DateTime GetFormatedDateWithTime(string date)
    {
        //DateTime dateTime = Convert.ToDateTime(date);
        DateTime dt = DateTime.ParseExact(date, COM.DateTimeFormat, null);
        return dt;
        //DateTime dt = DateTime.ParseExact(date, "dd/MM/yyyy tt:hh:ss", null);
        //return dt;
    }
    public static DateTime GetUtcToLocal() => DateTime.UtcNow.ToLocalTime();
    #endregion


    #region Conversion
    public static double ToDouble(string val) => Convert.ToDouble(val);
    #endregion


    #region Reflection
    public static object GetProppertyValue(object obj, string propertyName)
    {
        var propertyInfo = obj.GetType().GetProperty(propertyName);

        if (propertyInfo != null)
        {
            var value = propertyInfo.GetValue(obj, null);
            return value;
        }

        if (propertyName.Split('.').Length > 0)
        {
            string[] fieldNames = propertyName.Split('.');

            PropertyInfo currentProperty;

            object currentObject = obj;

            foreach (string fieldName in fieldNames)
            {
                if (currentObject == null) return null;

                Type curentRecordType = currentObject.GetType();

                currentProperty = curentRecordType.GetProperty(fieldName);

                if (currentProperty != null)
                {
                    var value = currentProperty.GetValue(currentObject, null);

                    if (fieldNames.Last() == fieldName)
                    {
                        return value;
                    }

                    currentObject = value;
                }
            }

            return null;
        }
        return null;
    }
    #endregion


    public static T ParseEnum<T>(string value)
    {
        try
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        catch
        {
            throw;
        }
    }
    public static class Path
    {
        public static string Combine(string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = System.IO.Path.Combine(directory, fileName);
            return path;
        }
    }
    public class Json
    {
        public static string SerializeObject<T>(T obj) where T : class => JsonConvert.SerializeObject(obj);
        public static List<T> DeSerializeObject<T>(string cach) where T : class => JsonConvert.DeserializeObject<List<T>>(cach);
    }
}


