using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using MaYiMaMa.CSharp.Extensions;

namespace MaYiMaMa.Unity.Extensions
{
    public static class MMTMP_DropdownExtensions
    {
        public delegate void OnValueChanged(TMP_Dropdown dropdown, int optionIndex);

        public static void SetOptions4EnumDescription<TEnum>(this TMP_Dropdown dropdown)
            where TEnum : Enum
        {
            dropdown.options.Clear();
            List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
            foreach (TEnum item in Enum.GetValues(typeof(TEnum)))
            {
                EnumDescriptionAttribute attribute = EnumExtensions.GetEnumDescription4AttributeUseEnumValue(item);
                if (attribute == null || string.IsNullOrEmpty(attribute.Description))
                {
                    continue;
                }
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = attribute.Description;
                optionDataList.Add(optionData);
            }
            dropdown.AddOptions(optionDataList);
        }

        public static void SetOptions4EnumDescription<TEnum>(this TMP_Dropdown dropdown, List<TEnum> enums)
            where TEnum : Enum
        {
            dropdown.options.Clear();
            List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
            foreach (TEnum item in enums)
            {
                EnumDescriptionAttribute attribute = EnumExtensions.GetEnumDescription4AttributeUseEnumValue(item);
                if (attribute == null || string.IsNullOrEmpty(attribute.Description))
                {
                    continue;
                }
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
                optionData.text = attribute.Description;
                optionDataList.Add(optionData);
            }
            dropdown.AddOptions(optionDataList);
        }

        public static void SortOptions4Enum<TEnum>(this TMP_Dropdown dropdown)
            where TEnum : Enum
        {
            dropdown.options.Sort((r, l) =>
            {
                int rOptionIndex = EnumExtensions.GetOptionIndex4AttributeUseDescription<TEnum>(r.text);
                int lOptionIndex = EnumExtensions.GetOptionIndex4AttributeUseDescription<TEnum>(l.text);
                int offset = rOptionIndex - lOptionIndex;
                if (offset > 0)
                    return 1;
                else if (offset < 0)
                    return -1;
                else
                    return 0;
            });
        }

        public static int GetOptionIndex(this TMP_Dropdown dropdown, string optionText)
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                if (dropdown.options[i].text == optionText)
                {
                    return i;
                }
            }
            return 0;
        }

        public static void AddOption(this TMP_Dropdown dropdown, string optionText, bool isTrigger)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(optionText);
            dropdown.options.Add(optionData);
            if (isTrigger)
            {
                if (dropdown.options.Count == 1)
                {
                    dropdown.TriggerListener(0);
                }
                else
                {
                    TMP_Dropdown.OptionData selectedOptionData = dropdown.options[dropdown.value];
                    int selectedOptionIndex = dropdown.options.IndexOf(selectedOptionData);
                    dropdown.TriggerListener(selectedOptionIndex);
                }
            }
        }

        public static void SafeAddOption(this TMP_Dropdown dropdown, string optionText, bool isTrigger)
        {
            var existOptionDatas = dropdown.options.Where(option => option.text == optionText);
            if (existOptionDatas.Count() <= 0)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(optionText);
                dropdown.options.Add(optionData);
            }
            if (isTrigger)
            {
                if (dropdown.options.Count == 1)
                {
                    dropdown.TriggerListener(0);
                }
                else
                {
                    TMP_Dropdown.OptionData selectedOptionData = dropdown.options[dropdown.value];
                    int selectedOptionIndex = dropdown.options.IndexOf(selectedOptionData);
                    dropdown.TriggerListener(selectedOptionIndex);
                }
            }
        }

        public static bool RemoveOption(this TMP_Dropdown dropdown, string optionText)
        {
            IEnumerable<TMP_Dropdown.OptionData> optionDatas = dropdown.options.Where(option => option.text == optionText);
            if(optionDatas.Count() <= 0)
            {
                return false;
            }

            TMP_Dropdown.OptionData optionData = optionDatas.First();
            if (optionData == null)
            {
                return false;
            }

            bool isSame = false;
            TMP_Dropdown.OptionData selectedOptionData = dropdown.options[dropdown.value];
            if (selectedOptionData == optionData)
            {
                isSame = true;
            }

            int deleteOptionIndex = dropdown.options.IndexOf(optionData);
            dropdown.options.RemoveAt(deleteOptionIndex);
            if (dropdown.options.Count > 0)
            {
                if (isSame)
                {
                    dropdown.TriggerListener(0);
                }
                else
                {
                    int selectedOptionIndex = dropdown.options.IndexOf(selectedOptionData);
                    dropdown.TriggerListener(selectedOptionIndex);
                }
            }
            else
            {
                dropdown.ClearCaptionText();
            }
            return true;
        }

        public static void AddListener(this TMP_Dropdown dropdown, OnValueChanged onValueChanged)
        {
            dropdown.onValueChanged.AddListener(optionIndex =>
            {
                onValueChanged?.Invoke(dropdown, optionIndex);
            });
        }

        public static void RemoveAllListeners(this TMP_Dropdown dropdown)
        {
            dropdown.onValueChanged.RemoveAllListeners();
        }

        // 即使前后设置的value一致，也保证会触发OnValueChanged
        public static void TriggerListener(this TMP_Dropdown dropdown, int optionIndex)
        {
            // 设置-1，其实是无效的，下层代码会把value设置为0，并执行OnValueChanged(0)的效果
            // dropdown.value = -1;    
            // 设置-1，其实是无效的，下层代码会把value设置为0，但是不会执行OnValueChanged(0)的效果
            dropdown.SetValueWithoutNotify(-1);
            if (dropdown.value == optionIndex)
            {
                dropdown.onValueChanged?.Invoke(optionIndex);
            }
            else
            {
                dropdown.value = optionIndex;
            }
        }

        public static void TriggerListener(this TMP_Dropdown dropdown, string optionText)
        {
            int optionIndex = GetOptionIndex(dropdown, optionText);
            TriggerListener(dropdown, optionIndex);
        }

        public static void ClearCaptionText(this TMP_Dropdown dropdown)
        {
            // 设置-1，其实是无效的，下层代码会把value设置为0，但是不会执行OnValueChanged(0)的效果
            dropdown.SetValueWithoutNotify(-1);
            dropdown.captionText.text = string.Empty;
        }
    }
}

