using UnityEngine;
using UnityEditor;

namespace Yeltic.SequencerSystem
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
            newColor = EditorGUILayout.ColorField("New Color", newColor);
        }
    }
}