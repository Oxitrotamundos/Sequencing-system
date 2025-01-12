using UnityEngine;
using UnityEditor;
using SequencerSystem;

public class RigidbodyMoveAction : BaseAction
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private RigidbodyMovementType movementType = RigidbodyMovementType.MovePosition;

    public enum RigidbodyMovementType
    {
        MovePosition,
        AddForce,
        Velocity
    }

    public override void Execute()
    {
        ActionEvents.TriggerRigidbodyMove(channel, targetPosition, targetRotation, movementCurve, movementType, duration);
    }

    public override void DrawInspector()
    {
        DrawChannelField();
        DrawDurationField();
#if UNITY_EDITOR
        targetPosition = EditorGUILayout.Vector3Field("Target Position", targetPosition);
        targetRotation = EditorGUILayout.Vector3Field("Target Rotation", targetRotation);
        movementType = (RigidbodyMovementType)EditorGUILayout.EnumPopup("Movement Type", movementType);
        movementCurve = EditorGUILayout.CurveField("Movement Curve", movementCurve);
#endif
    }

    public override string GetActionName()
    {
        return "Rigidbody Move";
    }
}