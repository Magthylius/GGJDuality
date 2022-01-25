using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Magthylius
{
    namespace Utilities
    {
        public static class MathUtil
        {
            public static bool Tolerance(Vector3 a, Vector3 b, float condition = 0.1f) => Vector3.Distance(a, b) < condition;
            public static bool Tolerance(float a, float b, float condition = 0.1f) => Mathf.Abs(a - b) < condition;
        }

        public static class ImageUtil
        {
            public static void SetAlpha(ref Image image, float targetAlpha)
            {
                Color imgNewCol = image.color;
                imgNewCol.a = targetAlpha;
                image.color = imgNewCol;
            }

            public static void LerpAlpha(ref Image image, float targetAlpha, float lerpAlpha, float toleranceSnap = 0.1f)
            {
                float newAlpha = Mathf.Lerp(image.color.a, targetAlpha, lerpAlpha);
                
                if (MathUtil.Tolerance(newAlpha, targetAlpha, toleranceSnap))
                    SetAlpha(ref image, targetAlpha);
                else
                    SetAlpha(ref image, newAlpha);
            }
        }
    }
}