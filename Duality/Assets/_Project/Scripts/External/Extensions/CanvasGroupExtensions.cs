using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class CanvasGroupEx
    {
        /// <summary>Starts a coroutine that fades the canvas group in and out.</summary>
        /// <param name="attachedMonoBehav">Monobehaviour to attach coroutine on.</param>
        /// <param name="fadeInTime">Duration of fading in.</param>
        /// <param name="waitTime">Wait time until fade out.</param>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static Coroutine StartFadeInOut(this CanvasGroup canvasGroup, MonoBehaviour attachedMonoBehav, float fadeInTime, float waitTime, float fadeOutTime)
        {
            return attachedMonoBehav.StartCoroutine(FadeInOutCoroutine(canvasGroup, fadeInTime, waitTime, fadeOutTime));
        }

        /// <summary>Starts a coroutine that fades the canvas group in.</summary>
        /// <param name="attachedMonoBehav">Monobehaviour to attach coroutine on.</param>
        /// <param name="fadeInTime">Duration of fading in.</param>
        public static Coroutine StartFadeIn(this CanvasGroup canvasGroup, MonoBehaviour attachedMonoBehav, float fadeInTime)
        {
            return attachedMonoBehav.StartCoroutine(FadeInCoroutine(canvasGroup, fadeInTime));
        }

        /// <summary>Starts a coroutine that fades the canvas group out.</summary>
        /// <param name="attachedMonoBehav">Monobehaviour to attach coroutine on.</param>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static Coroutine StartFadeOut(this CanvasGroup canvasGroup, MonoBehaviour attachedMonoBehav, float fadeOutTime)
        {
            return attachedMonoBehav.StartCoroutine(FadeOutCoroutine(canvasGroup, fadeOutTime));
        }

        /// <summary>Starts a coroutine that waits then fades the canvas group out.</summary>
        public static Coroutine StartWaitAndFadeOut(this CanvasGroup canvasGroup, MonoBehaviour attachedMonoBehav, float waitTime, float fadeOutTime)
        {
            return attachedMonoBehav.StartCoroutine(WaitAndFadeOutCoroutine(canvasGroup, waitTime, fadeOutTime));
        }

        /// <summary>IEnumerator coroutine that creates a simple fade in and out effect.</summary>
        /// <param name="fadeInTime">Duration of fading in.</param>
        /// <param name="waitTime">Wait time until fade out.</param>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static IEnumerator FadeInOutCoroutine(this CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime)
        {
            //! Fade In
            float alphaStep = 1f / fadeInTime;
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += alphaStep * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //! Wait
            canvasGroup.alpha = 1f;
            yield return new WaitForSeconds(waitTime);

            //! Fade Out
            alphaStep = 1f / fadeOutTime;
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= alphaStep * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = 0f;
        }

        /// <summary>IEnumerator coroutine that creates a simple fade in effect.</summary>
        /// <param name="fadeOutTime">Duration of fading in.</param>
        public static IEnumerator FadeInCoroutine(this CanvasGroup canvasGroup, float fadeInTime)
        {
            float alphaStep = 1f / fadeInTime;
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += alphaStep * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = 1f;
        }

        /// <summary>IEnumerator coroutine that creates a simple fade out effect.</summary>
        /// <param name="fadeOutTime">Duration of fading out.</param>
        public static IEnumerator FadeOutCoroutine(this CanvasGroup canvasGroup, float fadeOutTime)
        {
            float alphaStep = 1f / fadeOutTime;
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= alphaStep * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = 0f;
        }

        /// <summary>IEnumerator coroutine that creates a simple fade out effect after a wait duration.</summary>
        public static IEnumerator WaitAndFadeOutCoroutine(this CanvasGroup canvasGroup, float waitTime, float fadeOutTime)
        {
            yield return new WaitForSeconds(waitTime);

            float alphaStep = 1f / fadeOutTime;
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= alphaStep * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            canvasGroup.alpha = 0f;
        }

        /// <summary>Sets alpha to 0.</summary>
        public static void SetTransparent(this CanvasGroup canvasGroup) => canvasGroup.alpha = 0f;
        
        /// <summary>Sets alpha to 1.</summary>
        public static void SetOpaque(this CanvasGroup canvasGroup) => canvasGroup.alpha = 1f;
    }
}
