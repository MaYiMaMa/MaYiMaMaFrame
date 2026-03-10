using System;

namespace MaYiMaMa.CSharp.Extensions
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public int OptionIndex { get; set; }
        public int IntValue { get; set; }
        public float FloatValue { get; set; }

        public EnumDescriptionAttribute(string description, int optionIndex)
            : this(description, optionIndex, 0, 0.0f)
        {

        }

        public EnumDescriptionAttribute(string description, int optionIndex, int intValue)
            : this(description, optionIndex, intValue, 0.0f)
        {

        }

        public EnumDescriptionAttribute(string description, int optionIndex, float floatValue)
            : this(description, optionIndex, 0, floatValue)
        {

        }

        public EnumDescriptionAttribute(string description, int optionIndex, int intValue, float floatValue)
        {
            Description = description;
            OptionIndex = optionIndex;
            IntValue = intValue;
            FloatValue = floatValue;
        }
    }
}
