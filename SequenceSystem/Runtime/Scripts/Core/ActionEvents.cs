using UnityEngine;
using System;

namespace Yeltic.SequencerSystem
{
    public static class ActionEvents
    {
        public static event Action<int, bool> OnActivateObject;
        public static event Action<int, Color> OnChangeColor;

        public static void TriggerActivateObject(int channel, bool activate)
        {
            Debug.Log($"ActionEvents: Triggering ActivateObject for channel {channel}, activate: {activate}");
            OnActivateObject?.Invoke(channel, activate);
        }

        public static void TriggerChangeColor(int channel, Color newColor)
        {
            OnChangeColor?.Invoke(channel, newColor);
        }
    }
}