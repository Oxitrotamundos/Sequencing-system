using UnityEngine;

namespace Yeltic.SequencerSystem
{
    public class ChangeColorObserver : BaseObserver
    {
        [SerializeField] private Renderer targetRenderer;

        protected override void RegisterEvents()
        {
            ActionEvents.OnChangeColor += HandleChangeColor;
        }

        protected override void UnregisterEvents()
        {
            ActionEvents.OnChangeColor -= HandleChangeColor;
        }

        private void HandleChangeColor(int actionChannel, Color newColor)
        {
            if (actionChannel == channel && targetRenderer != null)
            {
                targetRenderer.material.color = newColor;
            }
        }
    }
}