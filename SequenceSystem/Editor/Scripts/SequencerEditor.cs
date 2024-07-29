using UnityEngine;
using UnityEditor;

namespace Yeltic.SequencerSystem
{
    [CustomEditor(typeof(Sequencer))]
    public class SequencerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Sequencer sequencer = (Sequencer)target;

            if (GUILayout.Button("Reinitialize Sequencer"))
            {
                sequencer.ResetTurns();
                EditorUtility.SetDirty(target);
            }

            sequencer.InitialDelay = EditorGUILayout.FloatField("Initial Delay", sequencer.InitialDelay);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Turns", EditorStyles.boldLabel);

            for (int i = 0; i < sequencer.Turns.Count; i++)
            {
                DrawTurn(sequencer.Turns[i], i);
            }

            if (GUILayout.Button("Add Turn"))
            {
                sequencer.Turns.Add(new Turn());
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Execute Sequence"))
            {
                sequencer.ExecuteSequence();
            }

            serializedObject.ApplyModifiedProperties();
        }


        private void DrawTurn(Turn turn, int turnIndex)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Turn {turnIndex + 1}", EditorStyles.boldLabel);
            if (GUILayout.Button("Remove Turn", GUILayout.Width(100)))
            {
                Sequencer sequencer = (Sequencer)target;
                sequencer.Turns.RemoveAt(turnIndex);
                EditorUtility.SetDirty(target);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                return;
            }
            EditorGUILayout.EndHorizontal();

            turn.initialDelay = EditorGUILayout.FloatField("Initial Delay", turn.initialDelay);

            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            for (int i = 0; i < turn.actions.Count; i++)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.BeginHorizontal();

                // Obtener el nombre de la acción
                string actionName = turn.actions[i].GetActionName();
                EditorGUILayout.LabelField(actionName, EditorStyles.boldLabel);

                if (GUILayout.Button("↑", GUILayout.Width(20)) && i > 0)
                {
                    var temp = turn.actions[i];
                    turn.actions[i] = turn.actions[i - 1];
                    turn.actions[i - 1] = temp;
                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("↓", GUILayout.Width(20)) && i < turn.actions.Count - 1)
                {
                    var temp = turn.actions[i];
                    turn.actions[i] = turn.actions[i + 1];
                    turn.actions[i + 1] = temp;
                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    turn.actions.RemoveAt(i);
                    EditorUtility.SetDirty(target);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    break;
                }
                EditorGUILayout.EndHorizontal();

                // Dibujar las propiedades específicas de la acción
                EditorGUI.indentLevel++;
                turn.actions[i].DrawInspector();
                EditorGUI.indentLevel--;

                EditorGUILayout.EndVertical();
            }

            EditorGUI.indentLevel--;

            if (GUILayout.Button("Add Action"))
            {
                AddAction(turn);
            }

            EditorGUILayout.EndVertical();
        }

        private void AddAction(Turn turn)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Activate Object"), false, () => CreateAction(turn, typeof(ActivateObjectAction)));
            menu.AddItem(new GUIContent("Change Color"), false, () => CreateAction(turn, typeof(ChangeColorAction)));
            menu.ShowAsContext();
        }

        private void CreateAction(Turn turn, System.Type actionType)
        {
            BaseAction newAction = (BaseAction)System.Activator.CreateInstance(actionType);
            turn.actions.Add(newAction);
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}