using System.Text;
using System.Text.RegularExpressions;

namespace SqlSugar.CodeBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Code Builder Begin....");

            var db = new SqlSugarScope(new ConnectionConfig
            {
                //ConnectionString = "Server=localhost;Port=3306;Database=geologicaldatainformation;Uid=root;Pwd=123456;",
                //DbType = DbType.MySql,
                ConnectionString = "Data Source=192.168.1.100/orcl;User ID=C##EVGET;Password=123456",
                DbType = DbType.Oracle,
                ConfigId = 1,
            });

            //foreach (var tableInfo in db.DbMaintenance.GetTableInfoList())
            //{
            //    //var entityName = Regex.Replace(tableInfo.Name, "^[lL][kK]_", "").ToPascalString();
            //    var entityName = tableInfo.Name.ToUpper();
            //    db.MappingTables.Add(entityName, tableInfo.Name);
            //    foreach (var columnInfo in db.DbMaintenance.GetColumnInfosByTableName(tableInfo.Name))
            //    {
            //        var propertyName = columnInfo.DbColumnName.ToUpper();//.ToPascalString();
            //        db.MappingColumns.Add(propertyName, columnInfo.DbColumnName, entityName);
            //    }
            //}

            //db.DbFirst.SettingClassTemplate(temp =>
            //{
            //    return temp;
            //}).SettingClassDescriptionTemplate(temp =>
            //{
            //    return temp;
            //}).SettingConstructorTemplate(temp =>
            //{
            //    return temp;
            //}).SettingNamespaceTemplate(temp =>
            //{
            //    return temp;
            //}).SettingPropertyDescriptionTemplate(temp =>
            //{
            //    return temp;
            //}).SettingPropertyTemplate(temp =>
            //{
            //    return temp;
            //}).CreateClassFile("./classes", "DBEntities");

            db.DbFirst.IsCreateAttribute().CreateClassFile("./classes", "Evget.ReportsAnalysisingAPI.DBEntities");

            Console.WriteLine("Code Build Complete!");
            Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        public static string ToPascalString(this string str)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                var underline = true;
                foreach (char c in str)
                {
                    if (c == '_')
                    {
                        underline = true;
                        continue;
                    }
                    if (underline || sb.Length == 0)
                    {
                        if (c >= 'a' && c <= 'z')
                        {
                            sb.Append(Convert.ToChar(c - 32));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        underline = false;
                        continue;
                    }
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}