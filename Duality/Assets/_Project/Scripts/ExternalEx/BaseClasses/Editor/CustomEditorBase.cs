using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Magthylius
{
    public class CustomEditorBase<T> : Editor where T : MonoBehaviour
    {
        protected T Instance;

        protected virtual void OnEnable()
        {
            Instance = (T)target;
        }

        protected void DrawLabel(string label, GUIStyle style) => EditorGUILayout.LabelField(label, style);
        protected void DrawLabelHeader(string label) => DrawLabel(label, GUIStyleEx.Header);
        protected void DrawLabelComment(string label) => DrawLabel(label, GUIStyleEx.Comment);
    }
}
