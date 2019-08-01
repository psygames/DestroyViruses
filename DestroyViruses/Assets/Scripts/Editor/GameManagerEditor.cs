using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestroyViruses;
using ReflectionEx;

namespace DestroyViruses.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var mgr = target as GameManager;
            if (mgr != null)
            {
                var sm = mgr.ReflectField<StateMachine.StateMachine<StateMachine.State>>("mStateMachine");
                if (sm != null)
                {
                    EditorGUILayout.LabelField("Current State", sm.currentState.GetType().Name);
                }
            }

        }
    }
}