using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Yeltic.SequencerSystem
{
    public class Sequencer : MonoBehaviour
    {
        [SerializeField] private List<Turn> turns = new List<Turn>();
        [SerializeField] private float initialDelay = 0f;

        public List<Turn> Turns { get => turns; set => turns = value; }
        public float InitialDelay { get => initialDelay; set => initialDelay = value; }

        public void ExecuteSequence()
        {
            StartCoroutine(ExecuteSequenceCoroutine());
        }

        private IEnumerator ExecuteSequenceCoroutine()
        {
            yield return new WaitForSeconds(initialDelay);

            foreach (var turn in turns)
            {
                yield return new WaitForSeconds(turn.initialDelay);

                foreach (var action in turn.actions)
                {
                    action.Execute();
                }

                yield return new WaitForSeconds(turn.GetTotalDuration() - turn.initialDelay);
            }
        }

        public void ResetTurns()
        {
            turns = new List<Turn> { new Turn() };
        }
    }
}