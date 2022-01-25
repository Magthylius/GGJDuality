using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class ColorEx
    {
        public static Color SetAlpha(this Color c, float alpha)
        {
            c.a = alpha;
            return c;
        }
    }

    public static class SpriteRendererEx
    {
        /// <summary>Sets the renderer's sprite alpha color.</summary>
        public static void SetAlpha(this SpriteRenderer sr, float alpha)
        {
            sr.color = sr.color.SetAlpha(alpha);
        }

        /// <summary>Starts a coroutine that fades the canvas group in.</summary>
        /// <param name="attachedMonoBehav">Monobehaviour to attach coroutine on.</param>
        /// <param name="fadeInTime">Duration of fading in.</param>
        public static Coroutine StartFadeIn(this SpriteRenderer sr, MonoBehaviour attachedMonoBehav, float fadeInTime)
        {
            return attachedMonoBehav.StartCoroutine(FadeInCoroutine(sr, fadeInTime));
        }

        /// <summary>Starts a coroutine that fades the canvas group out.</summary>
        /// <param name="attachedMonoBehav">Monobehaviour to attach coroutine on.</param>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static Coroutine StartFadeOut(this SpriteRenderer sr, MonoBehaviour attachedMonoBehav, float fadeOutTime)
        {
            return attachedMonoBehav.StartCoroutine(FadeOutCoroutine(sr, fadeOutTime));
        }

        /// <summary>Starts a coroutine that waits then fades the canvas group out.</summary>
        public static Coroutine StartWaitAndFadeOut(this SpriteRenderer sr, MonoBehaviour attachedMonoBehav, float waitTime, float fadeOutTime)
        {
            return attachedMonoBehav.StartCoroutine(WaitAndFadeOutCoroutine(sr, waitTime, fadeOutTime));
        }

        /// <summary>IEnumerator coroutine that creates a simple fade in and out effect.</summary>
        /// <param name="fadeInTime">Duration of fading in.</param>
        /// <param name="waitTime">Wait time until fade out.</param>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static IEnumerator FadeInOutCoroutine(this SpriteRenderer sr, float fadeInTime, float waitTime, float fadeOutTime)
        {
            //! Fade In
            float alphaStep = 1f / fadeInTime;
            while (sr.Alpha() < 1f)
            {
                sr.SetAlpha(sr.Alpha() + alphaStep * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }

            //! Wait
            sr.SetAlpha(1f);
            yield return new WaitForSeconds(waitTime);

            //! Fade Out
            alphaStep = 1f / fadeOutTime;
            while (sr.Alpha() > 0f)
            {
                sr.SetAlpha(sr.Alpha() - alphaStep * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            sr.SetAlpha(0f);
        }

        /// <summary>IEnumerator coroutine that creates a simple fade in effect.</summary>
        /// <param name="fadeOutTime">Duration of fading in.</param>
        public static IEnumerator FadeInCoroutine(this SpriteRenderer sr, float fadeInTime)
        {
            float alphaStep = 1f / fadeInTime;
            while (sr.Alpha() < 1f)
            {
                sr.SetAlpha(sr.Alpha() + alphaStep * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            sr.SetAlpha(1f);
        }

        /// <summary>IEnumerator coroutine that creates a simple fade out effect.</summary>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static IEnumerator FadeOutCoroutine(this SpriteRenderer sr, float fadeOutTime)
        {
            float alphaStep = 1f / fadeOutTime;
            while (sr.Alpha() > 0f)
            {
                sr.SetAlpha(sr.Alpha() - alphaStep * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            sr.SetAlpha(0f);
        }

        /// <summary>IEnumerator coroutine that creates a simple fade out effect after a wait duration.</summary>
        public static IEnumerator WaitAndFadeOutCoroutine(this SpriteRenderer sr, float waitTime, float fadeOutTime)
        {
            yield return new WaitForSeconds(waitTime);

            float alphaStep = 1f / fadeOutTime;
            while (sr.Alpha() > 0f)
            {
                sr.SetAlpha(sr.Alpha() - alphaStep * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            sr.SetAlpha(0f);
        }

        /// <summary>Sets alpha to 0.</summary>
        public static void SetTransparent(this SpriteRenderer sr) => sr.SetAlpha(0f);

        /// <summary>Sets alpha to 1.</summary>
        public static void SetOpaque(this SpriteRenderer sr) => sr.SetAlpha(1f);

        public static float Alpha(this SpriteRenderer sr) => sr.color.a;
    }
}