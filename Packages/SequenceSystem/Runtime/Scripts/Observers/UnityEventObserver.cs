using UnityEngine;
using UnityEngine.Events;

namespace SequencerSystem
{
    public class UnityEventObserver : BaseObserver
    {
        [SerializeField] private string Note; //<-- describe para podamos ver en editor para que se esta utilizando el unity event
        [SerializeField] private UnityEvent unityEvent;

        protected override void RegisterEvents()
        {
            ActionEvents.OnUnityEventAction += HandleUnityEventAction;
        }

        protected override void UnregisterEvents()
        {
            ActionEvents.OnUnityEventAction -= HandleUnityEventAction;
        }

        private void HandleUnityEventAction(int actionChannel)
        {
            if (actionChannel == channel)
            {
                unityEvent.Invoke();
            }
        }
    }
}