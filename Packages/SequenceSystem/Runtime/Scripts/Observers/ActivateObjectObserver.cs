using UnityEngine;

namespace Yeltic.SequencerSystem
{
    public class ActivateObjectObserver : BaseObserver
    {
        private bool isRegistered = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (!isRegistered)
            {
                ActivationService.RegisterObject(channel, gameObject);
                isRegistered = true;
            }
            RegisterEvents();
        }

        protected override void OnDisable()
        {
            UnregisterEvents();
        }

        private void OnDestroy()
        {
            if (isRegistered)
            {
                ActivationService.UnregisterObject(channel, gameObject);
                isRegistered = false;
            }
        }

        protected override void RegisterEvents()
        {
            ActionEvents.OnActivateObject += HandleActivateObject;
        }

        protected override void UnregisterEvents()
        {
            ActionEvents.OnActivateObject -= HandleActivateObject;
        }

        private void HandleActivateObject(int actionChannel, bool activate)
        {
            if (actionChannel == channel)
            {
                ActivationService.HandleActivation(channel, gameObject, activate);
            }
        }
    }
}