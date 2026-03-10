using System;
using System.Collections.Generic;
using System.Reflection;

namespace MaYiMaMa.CSharp.Extensions
{
    public static class EnumExtensions
    {
        #region EnumDescriptionAttribute
        public static EnumDescriptionAttribute GetEnumDescription4AttributeUseEnumValue<TEnum>(this TEnum enumValue) 
            where TEnum : Enum
        {
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());
            EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
            if (attribute != null)
            {
                return attribute;
            }
            return null;
        } 

        public static EnumDescriptionAttribute GetEnumDescription4AttributeUseDescription<TEnum>(string description) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.Description == description)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        public static EnumDescriptionAttribute GetEnumDescription4AttributeUseOptionIndex<TEnum>(int optionIndex) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.OptionIndex == optionIndex)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        public static EnumDescriptionAttribute GetEnumDescription4AttributeUseIntValue<TEnum>(int intValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.IntValue == intValue)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }

        public static EnumDescriptionAttribute GetEnumDescription4AttributeUseFloatValue<TEnum>(float floatValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.FloatValue == floatValue)
                    {
                        return attribute;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Enum
        public static TEnum GetEnum4AttributeUseDescription<TEnum>(string description) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.Description == description)
                    {
                        return (TEnum)fieldInfo.GetValue(null);
                    }
                }
            }
            return default;
        }

        public static TEnum GetEnum4AttributeUseOptionIndex<TEnum>(int optionIndex) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.OptionIndex == optionIndex)
                    {
                        return (TEnum)fieldInfo.GetValue(null);
                    }
                }
            }
            return default;
        }

        public static TEnum GetEnum4AttributeUseIntValue<TEnum>(int intValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.IntValue == intValue)
                    {
                        return (TEnum)fieldInfo.GetValue(null);
                    }
                }
            }
            return default;
        }

        public static TEnum GetEnum4AttributeUseFloatValue<TEnum>(float floatValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.FloatValue == floatValue)
                    {
                        return (TEnum)fieldInfo.GetValue(null);
                    }
                }
            }
            return default;
        }
        #endregion

        #region Description
        public static string GetDescription4AttributeUseEnumValue<TEnum>(this TEnum enumValue) 
            where TEnum : Enum
        {
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());
            EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.Description;
            }
            return null;
        }

        public static string GetDescription4AttributeUseOptionIndex<TEnum>(int optionIndex) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.OptionIndex == optionIndex)
                    {
                        return attribute.Description;
                    }
                }
            }
            return null;
        }

        public static string GetDescription4AttributeUseIntValue<TEnum>(int intValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.IntValue == intValue)
                    {
                        return attribute.Description;
                    }
                }
            }
            return null;
        }

        public static string GetDescription4AttributeUseFloatValue<TEnum>(float floatValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.FloatValue == floatValue)
                    {
                        return attribute.Description;
                    }
                }
            }
            return null;
        }
        #endregion

        #region OptionIndex
        public static int GetOptionIndex4AttributeUseEnumValue<TEnum>(this TEnum enumValue) 
            where TEnum : Enum
        {
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());
            EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
            if (attribute != null)
            {
                return attribute.OptionIndex;
            }
            return -1;
        }

        public static int GetOptionIndex4AttributeUseDescription<TEnum>(string description) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.Description == description)
                    {
                        return attribute.OptionIndex;
                    }
                }
            }
            return -1;
        }

        public static int GetOptionIndex4AttributeUseIntValue<TEnum>(int intValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.IntValue == intValue)
                    {
                        return attribute.OptionIndex;
                    }
                }
            }
            return -1;
        }

        public static int GetOptionIndex4AttributeUseFloatValue<TEnum>(float floatValue) 
            where TEnum : Enum
        {
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.FloatValue == floatValue)
                    {
                        return attribute.OptionIndex;
                    }
                }
            }
            return -1;
        }
        #endregion

        #region IntValue
        public static bool GetIntValue4AttributeUseEnumValue<TEnum>(this TEnum enumValue, out int intValue) 
            where TEnum : Enum
        {
            intValue = 0;
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());
            EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
            if (attribute != null)
            {
                intValue = attribute.IntValue;
                return true;
            }
            return false;
        }

        public static bool GetIntValue4AttributeUseDescription<TEnum>(string description, out int intValue) 
            where TEnum : Enum
        {
            intValue = 0;
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.Description == description)
                    {
                        intValue = attribute.IntValue;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool GetIntValue4AttributeUseOptionIndex<TEnum>(int optionIndex, out int intValue) 
            where TEnum : Enum
        {
            intValue = 0;
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.OptionIndex == optionIndex)
                    {
                        intValue = attribute.IntValue;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool GetIntValue4AttributeUseFloatValue<TEnum>(float floatValue, out int intValue) 
            where TEnum : Enum
        {
            intValue = 0;
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.FloatValue == floatValue)
                    {
                        intValue = attribute.IntValue;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region FloatValue
        public static bool GetFloatValue4AttributeUseEnumValue<TEnum>(this TEnum enumValue, out float floatValue) where TEnum : Enum
        {
            floatValue = 0.0f;
            Type type = enumValue.GetType();
            FieldInfo fieldInfo = type.GetField(enumValue.ToString());
            EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
            if (attribute != null)
            {
                floatValue = attribute.FloatValue;
                return true;
            }
            return false;
        }

        public static bool GetFloatValue4AttributeUseDescription<TEnum>(string description, out float floatValue)
            where TEnum : Enum
        {
            floatValue = 0.0f;
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.Description == description)
                    {
                        floatValue = attribute.FloatValue;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool GetFloatValue4AttributeUseOptionIndex<TEnum>(int optionIndex, out float floatValue)
            where TEnum : Enum
        {
            floatValue = 0.0f;
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.OptionIndex == optionIndex)
                    {
                        floatValue = attribute.FloatValue;
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool GetFloatValue4AttributeUseIntValue<TEnum>(int intValue, out float floatValue)
            where TEnum : Enum
        {
            floatValue = 0.0f;
            Type enumType = typeof(TEnum);
            FieldInfo[] fieldInfos = enumType.GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType == enumType)
                {
                    EnumDescriptionAttribute attribute = fieldInfo.GetCustomAttribute<EnumDescriptionAttribute>();
                    if (attribute != null && attribute.IntValue == intValue)
                    {
                        floatValue = attribute.FloatValue;
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 枚举位处理
        /// <summary>
        /// 枚举列表合并为byte字节
        /// </summary>
        public static byte CombineEnumList2Byte<T>(List<T> enumValues) where T : struct, Enum
        {
            byte value = 0;
            if(enumValues == null || enumValues.Count <= 0)
            {
                return value;
            }
            foreach(T enumValue in enumValues)
            {
                value |= (byte)Convert.ChangeType(enumValue, typeof(byte));
            }
            return value;
        }

        /// <summary>
        /// 字节按照位运算拆分为枚举列表
        /// </summary>
        public static List<T> SplitByte2EnumList<T>(byte value) where T : struct, Enum
        {
            List<T> result = new List<T>();
            foreach(T enumValue in Enum.GetValues(typeof(T)))
            {
                byte enumByte = (byte)Convert.ChangeType(enumValue, (typeof(byte)));
                if((value & enumByte) != 0)
                {
                    result.Add(enumValue);
                }
            }
            return result;
        }
        #endregion

        #region 比较
        public static int CompareTo<T>(this T enumValue1, T enumValue2) where T : Enum
        {
            int value1 = Convert.ToInt32(enumValue1);
            int value2 = Convert.ToInt32(enumValue2);
            return value1.CompareTo(value2);
        }

        public static bool GreaterThan<T>(this T enumValue1, T enumValue2) where T : Enum
        {
            return enumValue1.CompareTo(enumValue2) > 0;
        }

        public static bool EqualOrGreaterThan<T>(this T enumValue1, T enumValue2) where T : Enum
        {
            return enumValue1.CompareTo(enumValue2) >= 0;
        }

        public static bool LessThan<T>(this T enumValue1, T enumValue2) where T : Enum
        {
            return enumValue1.CompareTo(enumValue2) < 0;
        }

        public static bool EqualOrLessThan<T>(this T enumValue1, T enumValue2) where T : Enum
        {
            return enumValue1.CompareTo(enumValue2) <= 0;
        }
        #endregion
    }
}
