using UnityEngine;
using UnityEditor;

namespace SequencerSystem
{
    public class DebugMessageAction : BaseAction
    {
        public enum MessageType
        {
            Log,
            Warning,
            Error,
            Custom
        }

        public string message = "Debug Message";
        public MessageType messageType = MessageType.Log;
        public Color customColor = Color.green;

        public override string GetActionName()
        {
            return "Debug Message";
        }

        public override void Execute()
        {
            switch (messageType)
            {
                case MessageType.Log:
                    Debug.Log(message);
                    break;
                case MessageType.Warning:
                    Debug.LogWarning(message);
                    break;
                case MessageType.Error:
                    Debug.LogError(message);
                    break;
                case MessageType.Custom:
                    Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(customColor)}>{message}</color>");
                    break;
            }
        }


        public override void DrawInspector()
        {
            #if UNITY_EDITOR

            message = EditorGUILayout.TextField("Message", message);
            messageType = (MessageType)EditorGUILayout.EnumPopup("Message Type", messageType);

            if (messageType == MessageType.Custom)
            {
                customColor = EditorGUILayout.ColorField("Custom Color", customColor);
            }
            #endif

        }

    }
}