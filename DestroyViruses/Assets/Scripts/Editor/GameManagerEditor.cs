using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DestroyViruses;

namespace DestroyViruses.Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var mgr = target as GameManager;
            if (mgr != null && mgr.stateMachine != null && mgr.stateMachine.currentState != null)
                EditorGUILayout.LabelField("Current State", mgr.stateMachine.currentState.GetType().Name);

        }
    }
}