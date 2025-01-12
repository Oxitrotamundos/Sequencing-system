using UnityEditor;
using UnityEngine;

namespace SequencerSystem
{
    [System.Serializable]

    public class SetScaleAction : BaseAction
    {
        [SerializeField] private Vector3 scale;


        public override void Execute()
        {
            ActionEvents.TriggerSetScale(channel, scale);
        }

        public override void DrawInspector()
        {
            DrawChannelField();
            DrawDurationField();

#if UNITY_EDITOR
            scale = EditorGUILayout.Vector3Field("Escala", scale);
#endif
        }

        public override string GetActionName()
        {
            return "Set Scale";
        }
    }
}
