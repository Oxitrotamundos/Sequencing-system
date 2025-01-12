using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SequencerSystem
{
    [CustomEditor(typeof(Sequencer))]
    public class SequencerEditor : Editor
    {
        private Dictionary<int, bool> turnFoldouts = new Dictionary<int, bool>();
        private Dictionary<string, bool> actionFoldouts = new Dictionary<string, bool>();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Sequencer sequencer = (Sequencer)target;

            if (GUILayout.Button("Force Save Changes"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

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

            if (!turnFoldouts.ContainsKey(turnIndex))
            {
                turnFoldouts[turnIndex] = true;
            }

            turnFoldouts[turnIndex] = EditorGUILayout.Foldout(turnFoldouts[turnIndex], $"Turn {turnIndex + 1}", true);

            // Aqui se dibujan los nuevos botones :D
            GUI.enabled = turnIndex > 0;
            if (GUILayout.Button("↑", GUILayout.Width(25)))
            {
                SwapTurns(turnIndex, turnIndex - 1);
            }
            GUI.enabled = true;

            GUI.enabled = turnIndex < ((Sequencer)target).Turns.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                SwapTurns(turnIndex, turnIndex + 1);
            }
            GUI.enabled = true;

            if (GUILayout.Button("Remove Turn", GUILayout.Width(100)))
            {
                Sequencer sequencer = (Sequencer)target;
                sequencer.Turns.RemoveAt(turnIndex);
                turnFoldouts.Remove(turnIndex);
                EditorUtility.SetDirty(target);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                return;
            }
            EditorGUILayout.EndHorizontal();

            if (turnFoldouts[turnIndex])
            {
                EditorGUI.indentLevel++;

                turn.initialDelay = EditorGUILayout.FloatField("Initial Delay", turn.initialDelay);

                EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                for (int i = 0; i < turn.actions.Count; i++)
                {
                    DrawAction(turn, i);
                }

                EditorGUI.indentLevel--;

                if (GUILayout.Button("Add Action"))
                {
                    AddAction(turn);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void SwapTurns(int index1, int index2)
        {
            Sequencer sequencer = (Sequencer)target;
            Turn temp = sequencer.Turns[index1];
            sequencer.Turns[index1] = sequencer.Turns[index2];
            sequencer.Turns[index2] = temp;
            EditorUtility.SetDirty(target);
        }

        private void DrawAction(Turn turn, int actionIndex)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            BaseAction action = turn.actions[actionIndex];
            string actionKey = $"turn_{turn.GetHashCode()}_action_{actionIndex}";

            if (!actionFoldouts.ContainsKey(actionKey))
            {
                actionFoldouts[actionKey] = true;
            }

            EditorGUILayout.BeginHorizontal();

            string foldoutLabel = string.IsNullOrEmpty(action.CustomLabel)
                ? action.GetActionName()
                : $"{action.CustomLabel} ({action.GetActionName()})";

            actionFoldouts[actionKey] = EditorGUILayout.Foldout(actionFoldouts[actionKey], foldoutLabel, true);

            if (GUILayout.Button("↑", GUILayout.Width(20)) && actionIndex > 0)
            {
                var temp = turn.actions[actionIndex];
                turn.actions[actionIndex] = turn.actions[actionIndex - 1];
                turn.actions[actionIndex - 1] = temp;
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("↓", GUILayout.Width(20)) && actionIndex < turn.actions.Count - 1)
            {
                var temp = turn.actions[actionIndex];
                turn.actions[actionIndex] = turn.actions[actionIndex + 1];
                turn.actions[actionIndex + 1] = temp;
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                turn.actions.RemoveAt(actionIndex);
                actionFoldouts.Remove(actionKey);
                EditorUtility.SetDirty(target);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                return;
            }
            EditorGUILayout.EndHorizontal();

            if (actionFoldouts[actionKey])
            {
                EditorGUI.indentLevel++;
                action.CustomLabel = EditorGUILayout.TextField("Custom Label", action.CustomLabel);
                action.DrawInspector();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private void AddAction(Turn turn)
        {
            GenericMenu menu = new GenericMenu();

            //Set transform
            menu.AddItem(new GUIContent("Set/Transform/SetGlobalPosition"), false, () => CreateAction(turn, typeof(SetGlobalPositionAction)));
            menu.AddItem(new GUIContent("Set/Transform/SetLocalScale"), false, () => CreateAction(turn, typeof(SetScaleAction)));
            menu.AddItem(new GUIContent("Set/Transform/SetRotation"), false, () => CreateAction(turn, typeof(SetRotationAction)));

            //Set status
            menu.AddItem(new GUIContent("Set/Status/Activate Object"), false, () => CreateAction(turn, typeof(ActivateObjectAction)));

            //Set Renderer
            menu.AddItem(new GUIContent("Rendering/Change Color"), false, () => CreateAction(turn, typeof(ChangeColorAction)));

            //Set lighting
            menu.AddItem(new GUIContent("Lighting/SetLightParameters"), false, () => CreateAction(turn, typeof(SetLightParametersAction)));


            //Unity events
            menu.AddItem(new GUIContent("Systems/Unity Event"), false, () => CreateAction(turn, typeof(UnityEventAction)));

            //Debug message
            menu.AddItem(new GUIContent("Debug/Debug Message"), false, () => CreateAction(turn, typeof(DebugMessageAction))); // New debug message action

            //physics
            menu.AddItem(new GUIContent("Physics/Rigidbody Move"), false, () => CreateAction(turn, typeof(RigidbodyMoveAction)));



            //Pluging - Descomenta si se importaron los paquetes de NPC´s:
            // menu.AddItem(new GUIContent("Systems/NPC/NPC Movement"), false, () => CreateAction(turn, typeof(NPCMovementAction)));
            // menu.AddItem(new GUIContent("Systems/NPC/NPC Actions"), false, () => CreateAction(turn, typeof(NPCActionSequencerAction)));
            //menu.AddItem(new GUIContent("Systems/NPC/Set Look at"), false, () => CreateAction(turn, typeof(SetLookatTargetAction)));

            //Plugin - Descomenta si se importaron los paquetes de animación:
            //menu.AddItem(new GUIContent("Animator/Trigger Animation"), false, () => CreateAction(turn, typeof(AnimationTriggerAction)));



            //Custom - Deja aqui las acciones que son especificas de tu proyecto:



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