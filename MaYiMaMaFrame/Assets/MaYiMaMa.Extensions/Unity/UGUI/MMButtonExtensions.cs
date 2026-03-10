using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MaYiMaMa.Unity.Extensions
{
    public static class MMButtonExtensions
    {
        public delegate void OnClick(Button button);

        private static Dictionary<Button, Dictionary<Delegate, Delegate>> ActionMap = new Dictionary<Button, Dictionary<Delegate, Delegate>>();

        public static bool IsExistListener(this Button button, OnClick onClick)
        {
            if(!ActionMap.TryGetValue(button, out var dict) || dict == null || dict.Count <= 0)
            {
                return false;
            }
            return dict.ContainsKey(onClick);
        }

        public static bool AddListener(this Button button, OnClick onClick)
        {
            if(IsExistListener(button, onClick))
            {
                return false;
            }
            UnityAction action = () => 
            { 
                onClick?.Invoke(button); 
            };
            if(!ActionMap.TryGetValue(button, out var dict) || dict == null)
            {
                dict = new Dictionary<Delegate, Delegate>();
                ActionMap.Add(button, dict);
            }
            dict.Add(onClick, action);
            button.onClick.AddListener(action);
            return true;
        }

        public static bool RemoveListener(this Button button, OnClick onClick)
        {
            if(!ActionMap.TryGetValue(button, out var dict) || dict == null)
            {
                return false;
            }
            UnityAction removeAction = null;
            foreach(var pair in dict)
            {
                if(pair.Key.Target == onClick.Target && pair.Key.Method == onClick.Method)
                {
                    removeAction = (UnityAction)pair.Value;
                    break;
                } 
            }
            if(removeAction != null)
            {
                button.onClick.RemoveListener(removeAction);
                return ActionMap[button].Remove(removeAction);
            }
            return false;
        }

        public static void RemoveAllListeners(this Button button)
        {
            if(ActionMap.ContainsKey(button))
            {
                ActionMap.Remove(button);
            }
            button.onClick.RemoveAllListeners();
        }

        public static void TriggerListener(this Button button)
        {
            button.onClick?.Invoke();
        }
    }
}

