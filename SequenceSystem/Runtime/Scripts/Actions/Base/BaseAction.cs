using UnityEngine;
using UnityEditor;

namespace Yeltic.SequencerSystem
{
    [System.Serializable]
    public abstract class BaseAction
    {
        [SerializeField] protected int channel;
        [SerializeField] protected float duration;

        public float Duration => duration;

        public abstract void Execute();
        public abstract void DrawInspector();

        public abstract string GetActionName();


        protected void DrawChannelField()
        {
            channel = EditorGUILayout.IntField("Channel", channel);
        }

        protected void DrawDurationField()
        {
            duration = EditorGUILayout.FloatField("Duration", duration);
        }
    }
}