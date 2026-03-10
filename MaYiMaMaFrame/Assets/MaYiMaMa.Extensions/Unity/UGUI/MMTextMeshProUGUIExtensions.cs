using System;
using TMPro;

namespace MaYiMaMa.Unity.Extensions
{
    public static class MMTextMeshProUGUIExtensions
    {
        public static bool FloatValue(this TextMeshProUGUI textMeshProUGUI, out float value)
        {
            value = 0.0f;
            return float.TryParse(textMeshProUGUI.text, out value);
        }

        public static void FloatValue(this TextMeshProUGUI textMeshProUGUI, float value, int precision)
        {
            textMeshProUGUI.text = Math.Round(value, precision).ToString($"F{precision}");
        }

        public static void FloatValue(this TextMeshProUGUI textMeshProUGUI, float value, int precision, string unit)
        {
            textMeshProUGUI.text = $"{Math.Round(value, precision).ToString($"F{precision}")} {unit}";
        }

        public static bool IntValue(this TextMeshProUGUI textMeshProUGUI, out int value)
        {
            value = 0;
            return int.TryParse(textMeshProUGUI.text, out value);
        }

        public static void IntValue(this TextMeshProUGUI textMeshProUGUI, int value)
        {
            textMeshProUGUI.text = $"{value}";
        }

        public static void IntValue(this TextMeshProUGUI textMeshProUGUI, int value, string unit)
        {
            textMeshProUGUI.text = $"{value} {unit}";
        }
    }
}

