using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlSugar.CodeBuilder
{
    public class EntityCodeBuilder
    {
        private const string classTemplateString = @"{using}

namespace {namespace}
{
    {classdescription}
    public partial class {classname}
    {
        public {classname}()
        {
            {constructor}
        }
        {property}
    }
}";
        private const string classDescriptionTemplateString = @"/// <summary>
/// {classdescription}
/// </summary>";

        private const string usingTemplateString = @"using System;
using System.Linq;
using System.Text";

        private const string constractorTemplateString = "this.{propertyname} = {defaultvalue}";

        private const string propertyDescriptionTemplateString = @"/// <summary>
/// {propertydescription}
/// </summary>";

        private const string propertyTemplateString = "public {propertytype} {propertyname} { get; set; }";

        private Func<string, string> classTemplate = template =>
        {
            return template;
        };

        private Func<string, string> classDescriptionTemplate = template =>
        {
            return template;
        };

        private Func<string, string> usingTemplate = template =>
        {
            return template;
        };

        private Func<string, string> constructorTemplate = template =>
        {
            return template;
        };

        private Func<string, string> propertyDescriptionTemplate = template =>
        {
            return template;
        };

        private Func<string, string> propertyTemplate = template =>
        {
            return template;
        };

        public EntityCodeBuilder SettingClassTemplate(Func<string, string> func)
        {
            classTemplate = func;
            return this;
        }

        public EntityCodeBuilder SettingClassDescriptionTemplate(Func<string, string> func)
        {
            classDescriptionTemplate = func;
            return this;
        }

        public EntityCodeBuilder SettingUsingTemplate(Func<string, string> func)
        {
            usingTemplate = func;
            return this;
        }

        public EntityCodeBuilder SettingConstructorTemplate(Func<string, string> func)
        {
            constructorTemplate = func;
            return this;
        }

        public EntityCodeBuilder SettingPropertyDescriptionTemplate(Func<string, string> func)
        {
            propertyDescriptionTemplate = func;
            return this;
        }

        public EntityCodeBuilder SettingPropertyTemplate(Func<string, string> func)
        {
            propertyTemplate = func;
            return this;
        }

        public void Build(IEnumerable<DbTableInfo> tables, string outPath, string ns)
        {
            foreach (var table in tables)
            {
                var sb = new StringBuilder();
                foreach (var column in table.Columns)
                {
                    var propdescription = propertyDescriptionTemplate.Invoke(propertyDescriptionTemplateString);
                    var m = Regex.Match(propdescription, @"(.*)?{propertydescription}");
                    var description = Regex.Replace(column.Description, "(\r\n|\r|\n)", $"$1{m.Groups[1].Value}");

                    sb.Append(Regex.Replace(propdescription, "{propertydescription}", description));
                }
            }
        }
    }

    public class DbTableInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<DbColumnInfo> Columns { get; set; }
    }

    public class DbColumnInfo
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public string Description { get; set; }

        public bool IsNullable { get; set; }
    }
}
