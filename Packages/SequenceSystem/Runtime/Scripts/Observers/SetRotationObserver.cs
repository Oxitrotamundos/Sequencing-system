using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SequencerSystem
{
    public class SetRotationObserver : BaseObserver
    {
        protected override void RegisterEvents()
        {
            ActionEvents.OnSetRotation += HandleSetRotation;
        }

        protected override void UnregisterEvents()
        {
            ActionEvents.OnSetRotation -= HandleSetRotation;

        }


        private void HandleSetRotation(int ActionChannel, Vector3 rotation, Space space)
        {
            if (ActionChannel == channel)
            {
                if (space == Space.World)
                {
                    transform.rotation = Quaternion.Euler(rotation);
                }

                else
                {
                    transform.localRotation = Quaternion.Euler(rotation);
                }
            }
        }



    }
}

