using System;

namespace SequencerSystem
{
    [Serializable]
    public class UnityEventAction : BaseAction
    {
        public override void Execute()
        {
            ActionEvents.TriggerUnityEventAction(channel);
        }
        public override void DrawInspector()
        {
            DrawChannelField();
            DrawDurationField();
        }
        public override string GetActionName()
        {
            return "Unity Event";
        }
    }
}

