using UnityEngine;
using UnityEditor;

namespace SequencerSystem
{
    [System.Serializable]
    public class ActivateObjectAction : BaseAction
    {
        [SerializeField] private int actionChannel = 0;
        [SerializeField] private bool activate = true;

        public override string GetActionName()
        {
            return $"Activate Object ({(activate ? "Enable" : "Disable")})";
        }


        public override void Execute()
        {
            Debug.Log($"ActivateObjectAction: Executing for channel {actionChannel}, activate: {activate}");
            ActionEvents.TriggerActivateObject(actionChannel, activate);
        }

        public override void DrawInspector()
        {
            #if UNITY_EDITOR
            actionChannel = EditorGUILayout.IntField("Channel", actionChannel);
            #endif
            DrawDurationField();
            #if UNITY_EDITOR
            activate = EditorGUILayout.Toggle("Activate", activate);
            #endif
        }
    }
}