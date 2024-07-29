using UnityEngine;

namespace Yeltic.SequencerSystem
{
    public abstract class BaseObserver : MonoBehaviour
    {
        [SerializeField] protected int channel;

        protected virtual void OnEnable()
        {
            RegisterEvents();
        }

        protected virtual void OnDisable()
        {
            UnregisterEvents();
        }

        protected abstract void RegisterEvents();
        protected abstract void UnregisterEvents();
    }
}