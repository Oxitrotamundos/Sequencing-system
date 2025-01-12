using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SequencerSystem
{
    [System.Serializable]
    public class SetRotationAction : BaseAction
    {
        [SerializeField] private Vector3 rotation;
        [SerializeField] private Space rotationSpace = Space.World;

        public override void Execute()
        {
            ActionEvents.TriggerSetRotation(channel, rotation, rotationSpace);
        }

        public override void DrawInspector()
        {
            DrawChannelField();
            DrawDurationField();

            #if UNITY_EDITOR
            rotation = EditorGUILayout.Vector3Field("rotation", rotation);
            rotationSpace = (Space)EditorGUILayout.EnumPopup("Rotation Space", rotationSpace);
            #endif
        }

        public override string GetActionName()
        {
            return "Set rotation";
        }
    }
}