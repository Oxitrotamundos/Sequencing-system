using UnityEngine;
using UnityEditor;

namespace SequencerSystem
{
    [System.Serializable]
    public class ChangeColorAction : BaseAction
    {
        [SerializeField] private Color newColor;

        public override string GetActionName()
        {
            return "Change Color";
        }

   
        public override void Execute()
        {
            ActionEvents.TriggerChangeColor(channel, newColor);
        }

        public override void DrawInspector()
        {
            DrawChannelField();
            DrawDurationField();

            #if UNITY_EDITOR
            newColor = UnityEditor.EditorGUILayout.ColorField("New Color", newColor);
            #endif
        }
    }
}