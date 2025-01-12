using UnityEngine;
using UnityEditor;

namespace SequencerSystem
{
    [System.Serializable]
    public abstract class BaseAction
    {
        [SerializeField] protected string customLabel = "";
        [SerializeField] protected int channel;
        [SerializeField] protected float duration;

        public string CustomLabel
        {
            get => customLabel;
            set => customLabel = value;
        }

        public float Duration => duration;
        public abstract void Execute();
        public abstract void DrawInspector();
        public abstract string GetActionName();


        public string GetDisplayName()
        {
            return string.IsNullOrEmpty(customLabel)
                ? GetActionName()
                : $"{GetActionName()} - {customLabel}";
        }


        protected void DrawChannelField()
        {
#if UNITY_EDITOR
            channel = EditorGUILayout.IntField("Channel", channel);
#endif

        }

        protected void DrawDurationField()
        {
#if UNITY_EDITOR
            duration = EditorGUILayout.FloatField("Duration", duration);
#endif

        }
    }
}