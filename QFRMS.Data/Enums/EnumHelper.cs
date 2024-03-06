using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QFRMS.Data.Enums
{
    public class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString())!;

            if (field != null)
            {
                DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>()!;
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();    
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            //throw new ArgumentException("Not found.", nameof(description));
            return default(T);
        }
    }
}
