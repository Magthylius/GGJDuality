using System;
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
    public class EnemyBaseCustomEditor : CustomEditorBase
    {
        private void OnEnable()
        {
            Summary = "Enemy base script";
            Remarks = "Tries to kill the player";
        }
    }

}