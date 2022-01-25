using System.Collections;
using System.Collections.Generic;
using Magthylius;
using UnityEditor;
using UnityEngine;

namespace Duality.Enemy
{
    [CustomEditor(typeof(EnemyBase))]
    public class EnemyBaseCustomEditor : CustomEditorBase<EnemyBase>
    {
        public override void OnInspectorGUI()
        {
            DrawHeader("Test");
            DrawHeader("Try");
            DrawLabel("Try2", GUIStyleEx.Header.SetFontColor(Color.red));
            base.OnInspectorGUI();
        }
    }

}