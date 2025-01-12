using UnityEngine;
using UnityEditor;

namespace SequencerSystem
{
    public class SetLightParametersAction : BaseAction
    {
        public enum LightType
        {
            Point,
            Spot,
            Directional,
        }

        [SerializeField] private LightType lightType = LightType.Point;
        [SerializeField] private Color lightColor = Color.white;
        [SerializeField] private float lightIntensity = 1.0f;
        [SerializeField] private float lightRange = 10.0f;
        [SerializeField] private float indirectMultiplier = 1.0f;
        [SerializeField] private bool isActive = true;

        [SerializeField] private float spotInnerAngle = 30.0f;
        [SerializeField] private float spotOuterAngle = 45.0f;

        public override void Execute()
        {
            ActionEvents.TriggerSetLightParameters(
                channel,
                lightType,
                lightColor,
                lightIntensity,
                lightRange,
                indirectMultiplier,
                isActive,
                spotInnerAngle,
                spotOuterAngle
            );
        }

        public override void DrawInspector()
        {
            DrawChannelField();
            DrawDurationField();

#if UNITY_EDITOR
            isActive = EditorGUILayout.Toggle("Light Is Active", isActive);

            if (isActive)
            {
                lightType = (LightType)EditorGUILayout.EnumPopup("Light Type", lightType);
                lightColor = EditorGUILayout.ColorField("Light Color", lightColor);
                lightIntensity = EditorGUILayout.FloatField("Light Intensity", lightIntensity);

                if (lightType != LightType.Directional)
                {
                    lightRange = EditorGUILayout.FloatField("Light Range", lightRange);
                }
                else
                {
                    EditorGUILayout.LabelField("Light Range", "N/A for Directional light");
                }

                indirectMultiplier = EditorGUILayout.FloatField("Light Indirect Multiplier", indirectMultiplier);

                if (lightType == LightType.Spot)
                {
                    spotInnerAngle = EditorGUILayout.Slider("Spot Inner Angle", spotInnerAngle, 0f, 180f);
                    spotOuterAngle = EditorGUILayout.Slider("Spot Outer Angle", spotOuterAngle, spotInnerAngle, 180f);
                }
            }
#endif
        }

        public override string GetActionName()
        {
            return "Set Light Parameters";
        }
    }
}
