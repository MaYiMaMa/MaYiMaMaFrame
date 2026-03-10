using UnityEngine.UI;

public static class MMToggleExtensions 
{
    public delegate void OnValueChanged(Toggle toggle, bool isOn);

    public static void AddListener(this Toggle toggle, OnValueChanged onValueChanged)
    {
        toggle.onValueChanged.AddListener(isOn =>
        {
            onValueChanged?.Invoke(toggle, isOn); 
        });
    }

    public static void TriggerListener(this Toggle toggle, bool isOn)
    {
        if(toggle.isOn == isOn)
        {
            toggle.onValueChanged?.Invoke(isOn);
        }
        else
        {
            toggle.isOn = isOn;
        }
    }
}

