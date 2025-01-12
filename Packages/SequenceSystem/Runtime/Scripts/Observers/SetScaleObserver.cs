using SequencerSystem;
using UnityEngine;

public class SetScaleObserver : BaseObserver
{
    protected override void RegisterEvents()
    {
        ActionEvents.OnSetGlobalScale += HandlerSetScale;
    }

    protected override void UnregisterEvents() 
    {
        ActionEvents.OnSetGlobalScale -= HandlerSetScale;
    }

    private void HandlerSetScale(int actionChannel, Vector3 escale)
    {
        if (actionChannel == channel)
            transform.localScale = escale;
    }
}
