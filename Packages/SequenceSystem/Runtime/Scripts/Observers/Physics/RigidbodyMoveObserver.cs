using UnityEngine;
using SequencerSystem;
using System.Collections;

public class RigidbodyMoveObserver : BaseObserver
{
    private Rigidbody rb;
    private Coroutine currentMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void RegisterEvents()
    {
        ActionEvents.OnRigidbodyMove += HandleRigidbodyMove;
    }

    protected override void UnregisterEvents()
    {
        ActionEvents.OnRigidbodyMove -= HandleRigidbodyMove;
    }

    private void HandleRigidbodyMove(int actionChannel, Vector3 targetPos, Vector3 targetRot,
        AnimationCurve curve, RigidbodyMoveAction.RigidbodyMovementType moveType, float duration)
    {
        if (actionChannel == channel && rb != null)
        {
            if (currentMovement != null)
                StopCoroutine(currentMovement);

            currentMovement = StartCoroutine(MoveRigidbody(targetPos, targetRot, curve, moveType, duration));
        }
    }

    private IEnumerator MoveRigidbody(Vector3 targetPos, Vector3 targetRot,
        AnimationCurve curve, RigidbodyMoveAction.RigidbodyMovementType moveType, float duration)
    {
        Vector3 startPos = rb.position;
        Quaternion startRot = rb.rotation;
        Quaternion endRot = Quaternion.Euler(targetRot);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float curveValue = curve.Evaluate(t);

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, curveValue);
            Quaternion newRot = Quaternion.Lerp(startRot, endRot, curveValue);

            switch (moveType)
            {
                case RigidbodyMoveAction.RigidbodyMovementType.MovePosition:
                    rb.MovePosition(newPos);
                    rb.MoveRotation(newRot);
                    break;

                case RigidbodyMoveAction.RigidbodyMovementType.AddForce:
                    rb.AddForce((newPos - rb.position) * rb.mass, ForceMode.Force);
                    rb.AddTorque(new Vector3(
                        Mathf.DeltaAngle(rb.rotation.eulerAngles.x, newRot.eulerAngles.x),
                        Mathf.DeltaAngle(rb.rotation.eulerAngles.y, newRot.eulerAngles.y),
                        Mathf.DeltaAngle(rb.rotation.eulerAngles.z, newRot.eulerAngles.z)
                    ) * rb.mass, ForceMode.Force);
                    break;

                case RigidbodyMoveAction.RigidbodyMovementType.Velocity:
                    rb.velocity = (newPos - rb.position) / Time.fixedDeltaTime;
                    rb.angularVelocity = new Vector3(
                        Mathf.DeltaAngle(rb.rotation.eulerAngles.x, newRot.eulerAngles.x),
                        Mathf.DeltaAngle(rb.rotation.eulerAngles.y, newRot.eulerAngles.y),
                        Mathf.DeltaAngle(rb.rotation.eulerAngles.z, newRot.eulerAngles.z)
                    ) / Time.fixedDeltaTime;
                    break;
            }

            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Aseguramos la posición final
        rb.position = targetPos;
        rb.rotation = endRot;
    }
}