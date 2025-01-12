using System.Collections.Generic;
using UnityEngine;

namespace SequencerSystem
{
    [System.Serializable]
    public class Turn
    {
        [SerializeReference] public List<BaseAction> actions = new List<BaseAction>();
        public float initialDelay = 0f;

        public float GetTotalDuration()
        {
            float maxDuration = 0f;
            foreach (var action in actions)
            {
                maxDuration = Mathf.Max(maxDuration, action.Duration);
            }
            return initialDelay + maxDuration;
        }
    }
}