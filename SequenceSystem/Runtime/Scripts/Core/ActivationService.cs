using UnityEngine;
using System.Collections.Generic;

namespace Yeltic.SequencerSystem
{
    public static class ActivationService
    {
        private static Dictionary<int, HashSet<GameObject>> channelObjects = new Dictionary<int, HashSet<GameObject>>();

        public static void RegisterObject(int channel, GameObject obj)
        {
            if (!channelObjects.TryGetValue(channel, out var objects))
            {
                objects = new HashSet<GameObject>();
                channelObjects[channel] = objects;
            }
            objects.Add(obj);
            Debug.Log($"ActivationService: Registered object {obj.name} on channel {channel}");
        }

        public static void UnregisterObject(int channel, GameObject obj)
        {
            if (channelObjects.TryGetValue(channel, out var objects))
            {
                objects.Remove(obj);
                Debug.Log($"ActivationService: Unregistered object {obj.name} from channel {channel}");
                if (objects.Count == 0)
                {
                    channelObjects.Remove(channel);
                    Debug.Log($"ActivationService: Removed empty channel {channel}");
                }
            }
        }

        public static void HandleActivation(int channel, GameObject obj, bool activate)
        {
            Debug.Log($"ActivationService: Handling activation for object {obj.name} on channel {channel}, activate: {activate}");
            if (obj != null)
            {
                obj.SetActive(activate);
                Debug.Log($"ActivationService: Set object {obj.name} to {activate} on channel {channel}");
                
                // Reactivar el ActivateObjectObserver si el objeto se está activando
                if (activate)
                {
                    var observer = obj.GetComponent<ActivateObjectObserver>();
                    if (observer != null)
                    {
                        observer.enabled = true;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"ActivationService: Null object found in channel {channel}");
            }
        }
    }
}