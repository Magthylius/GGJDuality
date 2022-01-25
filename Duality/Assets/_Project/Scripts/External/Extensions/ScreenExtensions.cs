using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class ScreenEx
    {
        public static Vector2 Size => new Vector2(Screen.width, Screen.height);
        public static float HalfWidth => Screen.width * 0.5f;
        public static float HalfHeight => Screen.height * 0.5f;
    }
}
