using SequencerSystem;
using UnityEngine;

public class SetLightParametersObserver : BaseObserver
{
    protected override void RegisterEvents()
    {
        ActionEvents.OnSetLightParameters += HandleSetLightParameters;
    }

    protected override void UnregisterEvents()
    {
        ActionEvents.OnSetLightParameters -= HandleSetLightParameters;
    }

    private void HandleSetLightParameters(
        int channel,
        SetLightParametersAction.LightType lightType,
        Color color,
        float intensity,
        float range,
        float indirectMultiplier,
        bool isActive,
        float spotInnerAngle,
        float spotOuterAngle
    )
    {
        if (this.channel == channel)
        {
            Light targetLight = gameObject.GetComponent<Light>();

            if (targetLight != null)
            {
                targetLight.enabled = isActive;
                targetLight.color = color;
                targetLight.intensity = intensity;
                targetLight.bounceIntensity = indirectMultiplier;

                if (lightType == SetLightParametersAction.LightType.Directional)
                {
                    targetLight.range = 0;
                }
                else
                {
                    targetLight.range = range;
                }

                switch (lightType)
                {
                    case SetLightParametersAction.LightType.Point:
                        targetLight.type = LightType.Point;
                        break;
                    case SetLightParametersAction.LightType.Spot:
                        targetLight.type = LightType.Spot;
                        targetLight.spotAngle = spotOuterAngle;
                        targetLight.innerSpotAngle = spotInnerAngle;
                        break;
                    case SetLightParametersAction.LightType.Directional:
                        targetLight.type = LightType.Directional;
                        break;
                }

            }
            else
            {
                Debug.LogWarning("SetLightParametersObserver: No Light component found on the GameObject.");
            }
        }
    }
}
