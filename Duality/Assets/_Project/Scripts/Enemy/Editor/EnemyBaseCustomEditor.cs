using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Magthylius;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Duality.Enemy
{
    [CustomEditor(typeof(EnemyBase))]
    public class EnemyBaseCustomEditor : CustomEditorBase<EnemyBase>
    {
        public override void OnInspectorGUI()
        {
            DrawLabelHeader("Enemy base script");
            DrawLabelComment("Tries to kill the player");

            DrawPreview(new Rect(Vector2.zero, Vector2.one * 10f));
            base.OnInspectorGUI();
        }
    }

}