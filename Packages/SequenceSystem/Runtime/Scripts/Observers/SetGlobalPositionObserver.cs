using SequencerSystem;
using UnityEngine;

public class SetGlobalPositionObserver : BaseObserver
{
    protected override void RegisterEvents()
    {
        ActionEvents.OnSetGlobalPosition += HandleSetGlobalPosition;
    }

    protected override void UnregisterEvents()
    {
        ActionEvents.OnSetGlobalPosition -= HandleSetGlobalPosition;
    }

    private void HandleSetGlobalPosition(int actionChannel, Vector3 position)
    {
        if (actionChannel == channel)
        {
            transform.position = position;
        }
    }
}