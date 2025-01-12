using SequencerSystem;
using UnityEditor;
using UnityEngine;

public class SetGlobalPositionAction : BaseAction
{
    [SerializeField] private Vector3 globalPosition;

    public override void Execute()
    {
        ActionEvents.TriggerSetGlobalPosition(channel, globalPosition);
    }

    public override void DrawInspector()
    {
        DrawChannelField();
        DrawDurationField();
#if UNITY_EDITOR
        globalPosition = EditorGUILayout.Vector3Field("Global Position", globalPosition);
#endif
    }

    public override string GetActionName()
    {
        return "Set Global Position";
    }

}