using UnityEngine.UI;

public static class MMSliderExtensions 
{
    public delegate void OnValueChanged(Slider slider, float value);

    public static void AddListener(this Slider slider, OnValueChanged onValueChanged)
    {
        slider.onValueChanged.AddListener(optionIndex =>
        {
            onValueChanged?.Invoke(slider, optionIndex); 
        });
    }

    public static void RemoveAllListeners(this Slider slider) 
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    public static void TriggerListener(this Slider slider, float value)
    {
        if(slider.value == value)
        {
            slider.onValueChanged?.Invoke(value);
        }
        else
        {
            slider.value = value;
        }
    }
}

