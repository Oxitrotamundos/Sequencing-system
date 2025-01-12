using UnityEngine;
using System;

namespace SequencerSystem
{
    public static class ActionEvents
    {
        public static event Action<int, bool> OnActivateObject;
        public static event Action<int, Color> OnChangeColor;
        public static event Action<int, Vector3> OnSetGlobalPosition;
        public static event Action<int, Vector3, Vector3> OnSetPlayerPosition;
        public static event Action<int, Vector3> OnSetGlobalScale;
        public static event Action<int, SetLightParametersAction.LightType, Color, float, float, float, bool, float, float> OnSetLightParameters;
        public static event Action<int, Vector3, Space> OnSetRotation;
        public static event Action<int> OnUnityEventAction;
        public static event Action<int> OnTabletAction;
        public static event Action<int> OnHideInstructionAction;
        public static event Action<int, AudioClip, float> OnPlayAudio;
        public static event Action<int, int> OnSetLookatTarget;
        public static event Action<int, string> OnTriggerAnimationToCall;
        public static event Action<int, Vector3, Vector3, AnimationCurve, RigidbodyMoveAction.RigidbodyMovementType, float> OnRigidbodyMove;


        /// Plugins - De momento comentar sino se importaron los paquetes de NPC´s
        //Sistema NPC´s

        //Movement
        public static event Action<int, Vector3, float, float, string, string> OnNPCMovement;

        //NPC Actions
        public static event Action<int, string, string> OnNPCAction;




        public static void TriggerActivateObject(int channel, bool activate)
        {
            Debug.Log($"ActionEvents: Triggering ActivateObject for channel {channel}, activate: {activate}");
            OnActivateObject?.Invoke(channel, activate);
        }

        public static void TriggerChangeColor(int channel, Color newColor)
        {
            OnChangeColor?.Invoke(channel, newColor);
        }

        public static void TriggerSetGlobalPosition(int channel, Vector3 position)
        {
            OnSetGlobalPosition?.Invoke(channel, position);
        }

        public static void TriggerSetPlayerPosition(int channel, Vector3 position, Vector3 rotation)
        {
            OnSetPlayerPosition?.Invoke(channel, position, rotation);
        }

        public static void TriggerSetScale(int channel, Vector3 escale)
        {
            OnSetGlobalScale?.Invoke(channel, escale);
        }

        public static void TriggerSetLightParameters(
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
            OnSetLightParameters?.Invoke(channel, lightType, color, intensity, range, indirectMultiplier, isActive, spotInnerAngle, spotOuterAngle);
        }
        public static void TriggerUnityEventAction(int channel)
        {
            OnUnityEventAction?.Invoke(channel);
        }
        public static void TriggerTabletAction(int channel)
        {
            OnTabletAction?.Invoke(channel);
        }
        public static void TriggerHideInstructionAction(int channel)
        {
            OnHideInstructionAction?.Invoke(channel);
        }
        public static void TriggerSetRotation(int channel, Vector3 rotation, Space space)
        {
            OnSetRotation?.Invoke(channel, rotation, space);
        }

        public static void TriggerPlayAudio(int channel, AudioClip clip, float delay)
        {
            OnPlayAudio?.Invoke(channel, clip, delay);


        }

        //Physics
        public static void TriggerRigidbodyMove(int channel, Vector3 targetPos, Vector3 targetRot, AnimationCurve curve, RigidbodyMoveAction.RigidbodyMovementType moveType, float duration)
        {
            OnRigidbodyMove?.Invoke(channel, targetPos, targetRot, curve, moveType, duration);
        }

        //Animators

        public static void TriggerToCallAnimation(int channel, string TriggerName)
        {
            OnTriggerAnimationToCall?.Invoke(channel, TriggerName);
        }




        #region Plugins 
        //Acciones estaticas que no siempre se usaran. primeras pruebas - Comentar si no se importo el paquete de NPC´s
        public static void TriggerNPCMovement(int channel, Vector3 targetPosition, float rotationThreshold, float finalRotationOffset, string actionType, string actionParameter)
        {
            Debug.Log($"ActionEvents: Triggering NPCMovement for channel {channel}, target: {targetPosition}, action: {actionType}");
            OnNPCMovement?.Invoke(channel, targetPosition, rotationThreshold, finalRotationOffset, actionType, actionParameter);
        }

        //Trigger acciones Npc
        public static void TriggerNPCAction(int channel, string actionName, string actionParameter)
        {
            Debug.Log($"ActionEvents: Triggering NPCAction for channel {channel}, action: {actionName}, parameter: {actionParameter}");
            OnNPCAction?.Invoke(channel, actionName, actionParameter);
        }

        public static void TriggerSetLookatTarget(int channel, int targetId)
        {
            Debug.Log($"ActionEvents: Triggering SetLookatTarget for channel {channel}, target channel: {targetId}");
            OnSetLookatTarget?.Invoke(channel, targetId);
        }
        #endregion
    }
}