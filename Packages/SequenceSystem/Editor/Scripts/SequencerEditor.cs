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

            EditorGUILayout.Space(10);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 16;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.LabelField("Sequence Editor", headerStyle);
            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = new Color(0.7f, 0.9f, 0.7f);
            if (GUILayout.Button("Save Changes", GUILayout.Height(30)))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
            GUI.backgroundColor = new Color(0.9f, 0.7f, 0.7f);
            if (GUILayout.Button("Reset Sequencer", GUILayout.Height(30)))
            {
                sequencer.ResetTurns();
                EditorUtility.SetDirty(target);
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.BeginChangeCheck();
            float newDelay = EditorGUILayout.FloatField("Initial Delay", sequencer.InitialDelay);
            if (EditorGUI.EndChangeCheck())
            {
                sequencer.InitialDelay = newDelay;
                EditorUtility.SetDirty(target);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(15);

            GUIStyle turnHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
            turnHeaderStyle.fontSize = 14;
            EditorGUILayout.LabelField("Turns", turnHeaderStyle);

            for (int i = 0; i < sequencer.Turns.Count; i++)
            {
                DrawTurn(sequencer.Turns[i], i);
            }

            EditorGUILayout.Space(5);
            GUI.backgroundColor = new Color(0.8f, 0.8f, 1f);
            if (GUILayout.Button("Add Turn", GUILayout.Height(25)))
            {
                sequencer.Turns.Add(new Turn());
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Space(5);
            GUI.backgroundColor = new Color(1f, 0.9f, 0.7f);
            if (GUILayout.Button("Execute Sequence", GUILayout.Height(30)))
            {
                sequencer.ExecuteSequence();
            }
            GUI.backgroundColor = Color.white;

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTurn(Turn turn, int turnIndex)
        {
            var turnBoxStyle = new GUIStyle(GUI.skin.box);
            turnBoxStyle.padding = new RectOffset(10, 10, 10, 10);
            turnBoxStyle.margin = new RectOffset(0, 0, 5, 5);

            EditorGUILayout.BeginVertical(turnBoxStyle);

            var headerStyle = new GUIStyle(EditorStyles.toolbar);
            headerStyle.fixedHeight = 30;

            EditorGUILayout.BeginHorizontal(headerStyle);

            if (!turnFoldouts.ContainsKey(turnIndex))
            {
                turnFoldouts[turnIndex] = true;
            }

            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontSize = 12;
            foldoutStyle.fontStyle = FontStyle.Bold;

            turnFoldouts[turnIndex] = EditorGUILayout.Foldout(turnFoldouts[turnIndex],
                $"Turn {turnIndex + 1}", true, foldoutStyle);

            GUI.enabled = turnIndex > 0;
            if (GUILayout.Button("↑", GUILayout.Width(30), GUILayout.Height(20)))
            {
                SwapTurns(turnIndex, turnIndex - 1);
            }
            GUI.enabled = true;

            GUI.enabled = turnIndex < ((Sequencer)target).Turns.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(30), GUILayout.Height(20)))
            {
                SwapTurns(turnIndex, turnIndex + 1);
            }
            GUI.enabled = true;

            GUI.backgroundColor = new Color(1f, 0.7f, 0.7f);
            if (GUILayout.Button("Remove", GUILayout.Width(70), GUILayout.Height(20)))
            {
                Sequencer sequencer = (Sequencer)target;
                sequencer.Turns.RemoveAt(turnIndex);
                turnFoldouts.Remove(turnIndex);
                EditorUtility.SetDirty(target);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                return;
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            if (turnFoldouts[turnIndex])
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.Space(5);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.BeginChangeCheck();
                float newDelay = EditorGUILayout.FloatField("Initial Delay", turn.initialDelay);
                if (EditorGUI.EndChangeCheck())
                {
                    turn.initialDelay = newDelay;
                    EditorUtility.SetDirty(target);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                for (int i = 0; i < turn.actions.Count; i++)
                {
                    DrawAction(turn, i);
                }

                EditorGUI.indentLevel--;

                EditorGUILayout.Space(5);
                GUI.backgroundColor = new Color(0.7f, 0.9f, 0.7f);
                if (GUILayout.Button("Add Action", GUILayout.Height(25)))
                {
                    AddAction(turn);
                }
                GUI.backgroundColor = Color.white;

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
            var actionStyle = new GUIStyle(GUI.skin.box);
            actionStyle.padding = new RectOffset(5, 5, 5, 5);
            actionStyle.margin = new RectOffset(10, 10, 2, 2);

            EditorGUILayout.BeginVertical(actionStyle);

            BaseAction action = turn.actions[actionIndex];
            string actionKey = $"turn_{turn.GetHashCode()}_action_{actionIndex}";

            if (!actionFoldouts.ContainsKey(actionKey))
            {
                actionFoldouts[actionKey] = true;
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            string foldoutLabel = string.IsNullOrEmpty(action.CustomLabel)
                ? action.GetActionName()
                : $"{action.CustomLabel} ({action.GetActionName()})";

            GUIStyle actionFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            actionFoldoutStyle.fontSize = 11;

            actionFoldouts[actionKey] = EditorGUILayout.Foldout(actionFoldouts[actionKey],
                foldoutLabel, true, actionFoldoutStyle);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("↑", GUILayout.Width(25), GUILayout.Height(18)) && actionIndex > 0)
            {
                var temp = turn.actions[actionIndex];
                turn.actions[actionIndex] = turn.actions[actionIndex - 1];
                turn.actions[actionIndex - 1] = temp;
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("↓", GUILayout.Width(25), GUILayout.Height(18)) &&
                actionIndex < turn.actions.Count - 1)
            {
                var temp = turn.actions[actionIndex];
                turn.actions[actionIndex] = turn.actions[actionIndex + 1];
                turn.actions[actionIndex + 1] = temp;
                EditorUtility.SetDirty(target);
            }

            GUI.backgroundColor = new Color(1f, 0.7f, 0.7f);
            if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(18)))
            {
                turn.actions.RemoveAt(actionIndex);
                actionFoldouts.Remove(actionKey);
                EditorUtility.SetDirty(target);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                return;
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            if (actionFoldouts[actionKey])
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.Space(5);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.BeginChangeCheck();
                string newLabel = EditorGUILayout.TextField("Custom Label", action.CustomLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    action.CustomLabel = newLabel;
                    EditorUtility.SetDirty(target);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space(5);
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