using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    namespace UI
    {
        [System.Serializable]
        public class CanvasGroupFader : TickerObject
        {
            public CanvasGroup canvas;
            public bool affectsTouch;
            public float precision;
            public event FadeEvent FadeEndedEvent;

            private OpacityState _state;
            private bool _isPaused = false;

            public LerpSettings LerpSettings = new LerpSettings(5f, 0.01f);
            
            public CanvasGroupFader(CanvasGroup canvasGroup, bool startFadeInState, bool canAffectTouch, float alphaPrecision = 0.001f)
            {
                canvas = canvasGroup;
                affectsTouch = canAffectTouch;
                if (startFadeInState) SetStateFadeIn();
                else SetStateFadeOut();

                precision = alphaPrecision;
            }

            /// <summary> Essential to check for animation. Insert this in an Update function. </summary>
            /// <param name="speed">Speed of lerp, without deltaTime</param>
            public void Step(float speed)
            {
                if (_isPaused) return;

                if (_state == OpacityState.FadeToOpaque)
                {
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1f, speed);

                    if (1f - canvas.alpha <= precision)
                    {
                        _state = OpacityState.Opaque;
                        canvas.alpha = 1f;
                
                        if (affectsTouch) SetInteraction(true);

                        FadeEndedEvent?.Invoke(_state);
                        Pause();
                    }
                }
                else if (_state == OpacityState.FadeToTransparent)
                {
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0f, speed);
                
                    if (canvas.alpha <= precision)
                    {
                        _state = OpacityState.Transparent;
                        canvas.alpha = 0f;
                
                        if (affectsTouch) SetInteraction(false);
                        
                        FadeEndedEvent?.Invoke(_state);

                        Pause();
                    }
                }
            }
            
            public override void Update()
            {
                Step(LerpSettings.Speed * Time.deltaTime);
            }

            /// <summary> Force sets alpha of the canvas. </summary>
            /// <param name="alpha">Alpha to set</param>
            public void SetAlpha(float alpha)
            {
                canvas.alpha = alpha;
            }
            /// <summary> Force sets the interaction of canvas, both BlockRaycast and Interactable </summary>
            /// <param name="interaction">Interaction mode to set</param>
            public void SetInteraction(bool interaction)
            {
                canvas.blocksRaycasts = interaction;
                canvas.interactable = interaction;
            }
            /// <summary> Sets state to FadeToOpaque and allows the fade </summary>
            public void StartFadeIn()
            {
                SetStateFadeIn();
                Continue();
            }
            /// <summary> Sets state to FadeToTransparent and allows the fade </summary>
            public void StartFadeOut()
            {
                SetStateFadeOut();
                Continue();
            }

            /// <summary> Forces state to Transparent and disables the fade </summary>
            public void SetTransparent()
            {
                SetAlpha(0f);
                _state = OpacityState.Transparent;
                if (affectsTouch) SetInteraction(false);
                Pause();
            }
            /// <summary> Forces state to Opaque and disables the fade </summary>
            public void SetOpaque()
            {
                SetAlpha(1f);
                _state = OpacityState.Opaque;
                if (affectsTouch) SetInteraction(true);
                Pause();
            }
            
            public void SetStateFadeIn() => _state = OpacityState.FadeToOpaque;
            public void SetStateFadeOut() => _state = OpacityState.FadeToTransparent;
            public bool IsFading => _state == OpacityState.FadeToOpaque || _state == OpacityState.FadeToTransparent;

            public void Pause() => _isPaused = true;
            public void Continue() => _isPaused = false;

            public float Alpha => canvas.alpha;
        }
    }
}