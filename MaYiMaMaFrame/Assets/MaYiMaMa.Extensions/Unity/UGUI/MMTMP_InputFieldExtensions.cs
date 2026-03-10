using System;
using TMPro;

namespace MaYiMaMa.Unity.Extensions
{
    public static class MMTMP_InputFieldExtensions
    {
        public delegate void OnEndEdit(TMP_InputField inputField, string content);
        public delegate void OnValueChanged(TMP_InputField inputField, string content);
        public delegate void OnSelect(TMP_InputField inputField, string content);
        public delegate void OnDeselect(TMP_InputField inputField, string content);

        #region FloatValue
        public static bool FloatValue(this TMP_InputField inputField, out float value)
        {
            value = 0.0f;
            return float.TryParse(inputField.text, out value);
        }

        public static void FloatValue(this TMP_InputField inputField, float value, int precision)
        {
            inputField.text = Math.Round(value, precision).ToString($"F{precision}");
        }

        public static void FloatValue(this TMP_InputField inputField, float value, int precision, string unit)
        {
            inputField.text = $"{Math.Round(value, precision).ToString($"F{precision}")} {unit}";
        }
        #endregion

        #region IntValue
        public static bool IntValue(this TMP_InputField inputField, out int value)
        {
            value = 0;
            return int.TryParse(inputField.text, out value);
        }

        public static void IntValue(this TMP_InputField inputField, int value)
        {
            inputField.text = $"{value}";
        }

        public static void IntValue(this TMP_InputField inputField, int value, string unit)
        {
            inputField.text = $"{value} {unit}";
        }
        #endregion

        public static void AddListener4ValueChanged(this TMP_InputField inputField, OnValueChanged onValueChanged)
        {
            inputField.onValueChanged.AddListener(content =>
            {
                onValueChanged?.Invoke(inputField, content);
            });
        }

        public static void AddListener4EndEdit(this TMP_InputField inputField, OnEndEdit onEndEdit)
        {
            inputField.onEndEdit.AddListener(content =>
            {
                onEndEdit?.Invoke(inputField, content);
            });
        }

        public static void AddListener4OnSelect(this TMP_InputField inputField, OnSelect onSelect)
        {
            inputField.onSelect.AddListener(content =>
            {
                onSelect?.Invoke(inputField, content);
            });
        }

        public static void AddListener4OnDeselect(this TMP_InputField inputField, OnDeselect onDeselect)
        {
            inputField.onDeselect.AddListener(content =>
            {
                onDeselect?.Invoke(inputField, content);
            });
        }

        public static void TriggerListener4ValueChanged(this TMP_InputField inputField, string content)
        {
            if (inputField.text == content)
            {
                inputField.onValueChanged?.Invoke(content);
            }
            else
            {
                inputField.text = content;
            }
        }

        public static void TriggerListener4EndEdit(this TMP_InputField inputField, string content)
        {
            if (inputField.text == content)
            {
                inputField.onEndEdit?.Invoke(content);
            }
            else
            {
                inputField.text = content;
            }
        }
    }
}

