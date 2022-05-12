using System.Reflection;
#nullable disable

namespace StopSnooze.Core
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Retrieves the EnumMessage Attribute from an enum value,
        /// returns the .ToString() value if no attribute is found
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Message(this Enum value)
        {
            string stringValue = value.ToString();
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            EnumMessage[] attrs = fieldInfo.
                GetCustomAttributes(typeof(EnumMessage), false) as EnumMessage[];
            if (attrs.Length > 0)
            {
                stringValue = attrs[0].Value;
            }
            return stringValue;
        }
    }
}
