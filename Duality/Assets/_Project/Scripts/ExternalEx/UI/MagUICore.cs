using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Magthylius
{
    namespace UI
    {
        using Utilities;

        public delegate void FadeEvent(OpacityState state);

        public enum OpacityState
        {
            Transparent = 0,
            Opaque,
            FadeToTransparent,
            FadeToOpaque
        }

        //! Data Struct
        [System.Serializable]
        public struct OffsetGroup
        {
            public Vector2 min;
            public Vector2 max;

            public OffsetGroup(Vector2 n, Vector2 x)
            {
                min = n;
                max = x;
            }

            public OffsetGroup(RectTransform rect)
            {
                min = rect.offsetMin;
                max = rect.offsetMax;
            }

            public OffsetGroup(Rect rect)
            {
                min = new Vector2(rect.x, rect.y - rect.height);
                max = new Vector2(rect.x + rect.width, rect.y);
            }

            public OffsetGroup(OffsetGroup copy)
            {
                min = copy.min;
                max = copy.max;
            }

            public void AddWidth(float width)
            {
                min.x += width;
                max.x += width;
            }

            public void AddHeight(float height)
            {
                min.y += height;
                min.y += height;
            }

            public static void Copy(RectTransform target, OffsetGroup other)
            {
                target.offsetMin = other.min;
                target.offsetMax = other.max;
            }
        }

        
        [System.Serializable]
        public class FlexibleRectCorners : FlexibleRect
        {
            enum FRCornerMode
            {
                CENTER = 0,
                MIDDLE
            }

            public Vector2 originalOffSetMax;
            public Vector2 originalOffsetMin;

            Vector2 centeredOffsetMax;
            Vector2 centeredOffsetMin;

            Vector2 middledOffsetMax;
            Vector2 middledOffsetMin;

            float lerpPrecision = 0.01f;
            bool cornerTransition = false;
            bool goingOpen = false;
            FRCornerMode mode = FRCornerMode.CENTER;

            bool debugEnabled = false;

            public FlexibleRectCorners(RectTransform rectTr) : base(rectTr)
            {
                originalOffsetMin = rectTr.offsetMin;
                originalOffSetMax = rectTr.offsetMax;

                //! not a stretching RectTransform
                if (rectTr.anchorMax == rectTr.anchorMin)
                {
                    centeredOffsetMax = new Vector2(0f, originalOffSetMax.y);
                    centeredOffsetMin = new Vector2(0f, originalOffsetMin.y);

                    middledOffsetMax = new Vector2(originalOffSetMax.x, 0f);
                    middledOffsetMin = new Vector2(originalOffsetMin.x, 0f);
                }
                else Debug.LogError("Strecthed RectTr!");

                mode = FRCornerMode.CENTER;
                lerpPrecision = 0.01f;
            }

            public void CornerStep(float speed)
            {
                if (cornerTransition)
                {
                    if (goingOpen) cornerTransition = !CornerLerp(originalOffsetMin, originalOffSetMax, speed);
                    else
                    {
                        if (mode == FRCornerMode.CENTER) cornerTransition = !CornerLerp(centeredOffsetMin, centeredOffsetMax, speed);
                        else cornerTransition = !CornerLerp(middledOffsetMin, middledOffsetMax, speed);
                    }

                    if (debugEnabled) Debug.Log("Corner transitioning");
                    if (!cornerTransition) goingOpen = !goingOpen;
                }
            }

            public void CornerJump(Vector2 minTarget, Vector2 maxTarget)
            {
                rectTransform.offsetMin = minTarget;
                rectTransform.offsetMax = maxTarget;
            }

            bool CornerLerp(Vector2 minTarget, Vector2 maxTarget, float speed)
            {
                rectTransform.offsetMin = Vector2.Lerp(rectTransform.offsetMin, minTarget, speed);
                rectTransform.offsetMax = Vector2.Lerp(rectTransform.offsetMax, maxTarget, speed);

                if (Vector2.SqrMagnitude(rectTransform.offsetMin - minTarget) <= lerpPrecision * lerpPrecision)
                {
                    rectTransform.offsetMin = minTarget;
                    rectTransform.offsetMax = maxTarget;
                    return true;
                }
                return false;
            }

            public void StartMiddleLerp()
            {
                if (debugEnabled) Debug.Log("Middle lerp triggered");

                cornerTransition = true;
                mode = FRCornerMode.MIDDLE;
            }

            public void StartCenterLerp()
            {
                if (debugEnabled) Debug.Log("Center lerp triggered");

                cornerTransition = true;
                mode = FRCornerMode.CENTER;
            }

            public void Open()
            {
                goingOpen = false;
                CornerJump(originalOffsetMin, originalOffSetMax);
            }

            public void Close()
            {
                goingOpen = true;
                if (mode == FRCornerMode.CENTER) CornerJump(centeredOffsetMin, centeredOffsetMax);
                else CornerJump(middledOffsetMin, middledOffsetMax);
            }

            public void DebugEnable() => debugEnabled = true;
            public void DebugDisable() => debugEnabled = false;
            public override bool IsTransitioning => cornerTransition;
        }

        public class ParallaxRect : FlexibleRect
        {
            public ParallaxRect(RectTransform rectTr) : base(rectTr)
            {
                rectTransform = rectTr;
            }

            public void ParallaxStep(Vector2 mousePos, Canvas canvas, float speed, float precision = 0.1f)
            {
                Vector3 endPoint;
                if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    endPoint = new Vector3(mousePos.x, mousePos.y, canvas.planeDistance);
                    mousePos = Camera.main.ScreenToWorldPoint(endPoint);

                }

                Vector2 nextPos = mousePos - originalPosition;
                //Debug.Log(endPosition);
                StartLerp(nextPos);
                Step(speed, precision);
            }
        }

        public class ImageFiller
        {
            public Image image;
            public delegate void FillCompleteEvent();
            public event FillCompleteEvent OnFillComplete;

            float chargeRate;
            float charge;
            float maxCharge;

            float progress;
            bool allowCharge;
            bool stopChargeWhenFilled;

            bool isFilled;

            public ImageFiller(Image _image, float _chargeRate = 0.1f, float _maxCharge = 1f, bool _stopChargeWhenFilled = true)
            {
                image = _image;

                chargeRate = _chargeRate;
                charge = 0f;
                maxCharge = _maxCharge;
                stopChargeWhenFilled = _stopChargeWhenFilled;

                progress = 0f;
                isFilled = false;
                
                if (maxCharge <= 0) maxCharge = 1f;
            }

            public void Step(float speed)
            {
                if (isFilled && stopChargeWhenFilled) return;

                if (allowCharge) charge += chargeRate * speed;
                else if (charge > 0f) charge -= chargeRate * speed;

                progress = charge / maxCharge;

                if (progress >= 1f)
                {
                    isFilled = true;
                    OnFillComplete?.Invoke();

                    progress = 1f;
                    charge = maxCharge;
                }
                UpdateImage();
            }

            public void UpdateImage()
            {
                if (image == null) return;
                image.fillAmount = progress;
            }

            public void StartCharge()
            {
                allowCharge = true;
            }

            public void StopCharge()
            {
                allowCharge = false;
            }

            public void ResetCharge()
            {
                charge = 0f;
                progress = charge / maxCharge;
                isFilled = false;

                UpdateImage();
            }

            public bool IsFilled => isFilled;
        }

        #region Faders
        //! Canvas Group
        

        //! Graphic (Images and TMP)
        [System.Serializable]
        public class UIFader
        {
            public Graphic uiObject;
            OpacityState state;
            OpacityState targetState;

            Color original;
            Color opaque;
            Color transparent;

            bool allowTransition;
            public FadeEvent FadeEndedEvent;

            public UIFader(Graphic graphicObject)
            {
                uiObject = graphicObject;

                original = graphicObject.color;

                opaque = original;
                opaque.a = 1f;

                transparent = original;
                transparent.a = 0f;

                if (Alpha >= 1f) state = OpacityState.Opaque;
                else state = OpacityState.Transparent;

                targetState = state;
            }

            public void Step(float speed)
            {
                if (allowTransition)
                {
                    if (targetState == OpacityState.Transparent)
                    {
                        uiObject.color = Color.Lerp(uiObject.color, transparent, speed);

                        if (uiObject.color.a <= 0.001f)
                        {
                            uiObject.color = transparent;
                            state = OpacityState.Transparent;
                            allowTransition = false;
                            
                            FadeEndedEvent.Invoke(state);
                        }
                    }
                    else if (targetState == OpacityState.Opaque)
                    {
                        uiObject.color = Color.Lerp(uiObject.color, opaque, speed);

                        if (1f - uiObject.color.a <= 0.001f)
                        {
                            uiObject.color = opaque;
                            state = OpacityState.Opaque;
                            allowTransition = false;
                        }
                    }
                    else allowTransition = false;
                }
            }

            public void FadeToTransparent()
            {
                allowTransition = true;
                targetState = OpacityState.Transparent;
            }

            public void FadeToOpaque()
            {
                allowTransition = true;
                targetState = OpacityState.Opaque;
            }

            public void ForceTransparent()
            {
                uiObject.color = transparent;
                state = OpacityState.Transparent;
            }

            public void ForceOpaque()
            {
                uiObject.color = opaque;
                state = OpacityState.Opaque;
            }
            public float Alpha => uiObject.color.a;
            public Color OriginalColor => original;
            public Color OpaqueColor => opaque;
            public Color TransparentColor => transparent;
            public OpacityState CurrentState => state;
        }

        public class LineRendererFader
        {
            public LineRenderer renderer;
            OpacityState state;
            OpacityState targetState;

            //! Colors
            Color startOriginal;
            Color endOriginal;

            Color startOpaque;
            Color endOpaque;
            Color startTransparent;
            Color endTransparent;

            //! Width
            float startWidthOriginal;
            float endWidthOriginal;

            float startWidthHidden;
            float endWidthHidden;

            bool colorMode;
            bool allowTransition;
            FadeEvent FadeEndedEvent;

            public LineRendererFader(LineRenderer lineRenderer, bool isColorMode)
            {
                renderer = lineRenderer;
                startOriginal = renderer.startColor;
                endOriginal = renderer.endColor;

                startOpaque = startOriginal;
                endOpaque = endOriginal;
                startOpaque.a = 1f;
                endOpaque.a = 1f;

                startTransparent = startOpaque;
                endTransparent = endOpaque;
                startTransparent.a = 0f;
                endTransparent.a = 0f;

                startWidthOriginal = renderer.startWidth;
                endWidthOriginal = renderer.endWidth;
                startWidthHidden = 0f;
                endWidthHidden = 0f;

                colorMode = isColorMode;

                if (AlphaStart >= 1f) state = OpacityState.Opaque;
                else state = OpacityState.Transparent;

                targetState = state;
            }

            public void Step(float speed)
            {
                if (allowTransition)
                {
                    if (targetState == OpacityState.Transparent)
                    {
                        if (colorMode)
                        {
                            renderer.startColor = Color.Lerp(renderer.startColor, startTransparent, speed);
                            renderer.endColor = Color.Lerp(renderer.endColor, endTransparent, speed);

                            if (renderer.startColor.a <= 0.001f && renderer.endColor.a <= 0.001f)
                            {
                                renderer.startColor = startTransparent;
                                renderer.endColor = endTransparent;
                                state = OpacityState.Transparent;
                                allowTransition = false;
                                FadeEndedEvent?.Invoke(state);
                            }
                        }
                        else
                        {
                            renderer.startWidth = Mathf.Lerp(renderer.startWidth, startWidthHidden, speed);
                            renderer.endWidth = Mathf.Lerp(renderer.endWidth, endWidthHidden, speed);

                            if (renderer.startWidth <= 0.001f && renderer.endWidth <= 0.001f)
                            {
                                renderer.startWidth = startWidthHidden;
                                renderer.endWidth = endWidthHidden;
                                state = OpacityState.Transparent;
                                allowTransition = false;
                                FadeEndedEvent?.Invoke(state);
                            }
                        }
                    }
                    else if (targetState == OpacityState.Opaque)
                    {
                        if (colorMode)
                        {
                            renderer.startColor = Color.Lerp(renderer.startColor, startOpaque, speed);
                            renderer.endColor = Color.Lerp(renderer.endColor, endOpaque, speed);

                            if (1f - renderer.startColor.a <= 0.001f && 1f - renderer.endColor.a <= 0.001f)
                            {
                                renderer.startColor = startOpaque;
                                renderer.endColor = endOpaque;
                                state = OpacityState.Opaque;
                                allowTransition = false;
                                FadeEndedEvent?.Invoke(state);
                            }
                        }
                        else
                        {
                            renderer.startWidth = Mathf.Lerp(renderer.startWidth, startWidthOriginal, speed);
                            renderer.endWidth = Mathf.Lerp(renderer.endWidth, endWidthOriginal, speed);

                            if (startWidthOriginal - renderer.startWidth <= 0.001f && endWidthOriginal - renderer.endWidth <= 0.001f)
                            {
                                renderer.startWidth = startWidthOriginal;
                                renderer.endWidth = endWidthOriginal;
                                state = OpacityState.Transparent;
                                allowTransition = false;
                                FadeEndedEvent?.Invoke(state);
                            }
                        }
                    }
                    else allowTransition = false;
                }
            }

            public void FadeToTransparent()
            {
                allowTransition = true;
                targetState = OpacityState.Transparent;
            }

            public void FadeToOpaque()
            {
                allowTransition = true;
                targetState = OpacityState.Opaque;
            }

            public void ForceTransparent()
            {
                if (colorMode)
                {
                    renderer.startColor = startTransparent;
                    renderer.endColor = endTransparent;
                }
                else
                {
                    renderer.startWidth = startWidthHidden;
                    renderer.endWidth = endWidthHidden;
                }

                state = OpacityState.Transparent;
            }

            public void ForceOpaque()
            {
                if (colorMode)
                {
                    renderer.startColor = startOpaque;
                    renderer.endColor = endOpaque;
                }
                else
                {
                    renderer.startWidth = startWidthOriginal;
                    renderer.endWidth = endWidthOriginal;
                }

                state = OpacityState.Opaque;
            }

            public float AlphaStart => renderer.startColor.a;
            public float AlphaEnd => renderer.endColor.a;
        }
        #endregion

        #region Lerp Functions
        public class Lerp 
        {
            // rects
            public static bool Rect(OffsetGroup targetOffset, RectTransform movingObject, float lerpSpeed = 1f)
            {
                Vector2 botLeft = Vector2.Lerp(movingObject.offsetMin, targetOffset.min, lerpSpeed * Time.deltaTime);
                Vector2 topRight = Vector2.Lerp(movingObject.offsetMax, targetOffset.max, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(movingObject.offsetMin, targetOffset.min))
                {
                    movingObject.offsetMin = targetOffset.min;
                    movingObject.offsetMax = targetOffset.max;
                    return true;
                }

                movingObject.offsetMin = botLeft;
                movingObject.offsetMax = topRight;
                return false;
            }
            public static bool Rect(RectTransform targetRect, RectTransform movingObject, float lerpSpeed = 1f)
            {
                Vector2 botLeft = Vector2.Lerp(movingObject.offsetMin, targetRect.offsetMin, lerpSpeed * Time.deltaTime);
                Vector2 topRight = Vector2.Lerp(movingObject.offsetMax, targetRect.offsetMax, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(movingObject.offsetMin, targetRect.offsetMin))
                {
                    movingObject.offsetMin = targetRect.offsetMin;
                    movingObject.offsetMax = targetRect.offsetMax;
                    return true;
                }

                movingObject.offsetMin = botLeft;
                movingObject.offsetMax = topRight;
                return false;
            }

            // float
            public static bool Float(float a, float b, float lerpSpeed = 1f, float tolerance = 0.01f)
            {
                if (b < a)
                    a = Mathf.Lerp(b, a, lerpSpeed * Time.deltaTime);
                else
                    a = Mathf.Lerp(a, b, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(a, b, tolerance))
                {
                    a = b;
                    return true;
                }

                return false;
            }

            public static float Snap(float a, float b, float t, float tolerance = 0.001f)
            {
                float r = Mathf.Lerp(a, b, t);
                if (MathUtil.Tolerance(a, b, tolerance)) return b;
                return r;
            }

            // anchored position
            public static bool APosition(RectTransform obj, Vector2 targetPos, float lerpSpeed = 1f)
            {
                obj.anchoredPosition = Vector2.Lerp(obj.anchoredPosition, targetPos, lerpSpeed * Time.deltaTime);
                obj.anchoredPosition = new Vector2((float)System.Math.Round(obj.anchoredPosition.x, 1), (float)System.Math.Round(obj.anchoredPosition.y, 1));

                if (MathUtil.Tolerance(obj.anchoredPosition, targetPos))
                {
                    obj.anchoredPosition = targetPos;
                    return true;
                }

                return false;
            }

            // position
            public static bool Position(RectTransform obj, Vector2 targetPos, float lerpSpeed = 1f)
            {
                obj.position = Vector2.Lerp(obj.position, targetPos, lerpSpeed * Time.deltaTime);
                obj.position = new Vector2((float)System.Math.Round(obj.position.x, 1), (float)System.Math.Round(obj.position.y, 1));

                if (MathUtil.Tolerance(obj.position, targetPos))
                {
                    obj.position = targetPos;
                    return true;
                }

                return false;
            }

            // offset position
            public static bool OFPosition(RectTransform target, OffsetGroup destination, float lerpSpeed = 1f)
            {
                target.offsetMin = Vector2.Lerp(target.offsetMin, destination.min, lerpSpeed * Time.deltaTime);
                target.offsetMax = Vector2.Lerp(target.offsetMax, destination.max, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.offsetMin, destination.min))
                {
                    target.offsetMin = destination.min;
                    target.offsetMax = destination.max;
                    return true;
                }

                return false;
            }

            // vector
            /*public static bool Vector(Vector2 target, Vector2 destination, float lerpSpeed = 1f)
            {
                target = Vector2.Lerp(target, destination, lerpSpeed * Time.deltaTime);

                if (Tolerance(target, destination))
                {
                    target = destination;
                    return true;
                }

                return false;
            }*/

            public static Vector3 Vector(Vector3 target, Vector3 destination, float lerpSpeed = 1f)
            {
                target = Vector3.Lerp(target, destination, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target, destination))
                {
                    target = destination;
                }

                return target;
            }

            // size delta
            public static bool SizeDelta(RectTransform target, Vector2 targetSizeDelta, float lerpSpeed = 1f)
            {
                target.sizeDelta = Vector2.Lerp(target.sizeDelta, targetSizeDelta, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.sizeDelta, targetSizeDelta))
                {
                    target.sizeDelta = targetSizeDelta;
                    return true;
                }

                return false;
            }

            // offscreens
            public static bool OffScreenBelow(RectTransform target, Vector2 offsetMin, float lerpSpeed = 1f)
            {
                Vector2 minDest = new Vector2(target.offsetMin.x, -Screen.height);
                Vector2 maxDest = new Vector2(target.offsetMax.x, 0);

                target.offsetMin = Vector2.Lerp(target.offsetMin, minDest, lerpSpeed * Time.deltaTime);
                target.offsetMax = Vector2.Lerp(target.offsetMax, maxDest, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.offsetMin, minDest))
                {
                    target.offsetMin = minDest;
                    target.offsetMax = maxDest;
                    return true;
                }

                return false;
            }
            public static bool OffScreenBelow(RectTransform target, OffsetGroup offsetGrp, float lerpSpeed = 1f)
            {
                Vector2 minDest = new Vector2(target.offsetMin.x, Screen.height);
                Vector2 maxDest = new Vector2(target.offsetMax.x, 0);

                target.offsetMin = Vector2.Lerp(target.offsetMin, minDest, lerpSpeed * Time.deltaTime);
                target.offsetMax = Vector2.Lerp(target.offsetMax, maxDest, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.offsetMin, minDest))
                {
                    target.offsetMin = minDest;
                    target.offsetMax = maxDest;
                    return true;
                }

                return false;
            }

            // direct movements
            public static void ForceStay(GameObject obj, Vector3 forcedPos)
            {
                obj.transform.position = forcedPos;
            }
            public static void Warp(RectTransform target, OffsetGroup destination)
            {
                target.offsetMax = destination.max;
                target.offsetMin = destination.min;
            }
            public static void WarpOffScreenBelow(RectTransform target, Vector2 offsetMin)
            {
                target.offsetMin = new Vector2(target.offsetMin.x, -Screen.height);
                target.offsetMax = new Vector2(target.offsetMax.x, 0);
            }
            public static void WarpOffScreenBelow(RectTransform target, OffsetGroup offsetGrp)
            {
                target.offsetMin = new Vector2(target.offsetMin.x, -Screen.height);
                target.offsetMax = new Vector2(target.offsetMax.x, 0);
            }
            public static void Follow(RectTransform followTarget, RectTransform follower, bool stopChildren = false)
            {
                follower.offsetMax = followTarget.offsetMax;
                follower.offsetMin = followTarget.offsetMin;

                if (stopChildren)
                {
                    for (int i = 0; i < follower.childCount; i++)
                    {

                    }
                }
            }

            // queries
            public static bool OnPosition(Vector2 target1, Vector2 target2) => target1 == target2;
            public static bool OnAPosition(RectTransform target1, RectTransform target2) => target1.anchoredPosition == target2.anchoredPosition;
            public static bool OnAPosition(RectTransform target1, Vector2 target2) => target1.anchoredPosition == target2;
            public static bool OnOFPosition(RectTransform target1, RectTransform target2) => target1.offsetMax == target2.offsetMax && target1.offsetMin == target2.offsetMin;

            #region QUATENIONS
            public static bool Rotation(RectTransform target, Quaternion rotation, float lerpSpeed = 1f)
            {
                target.rotation = Quaternion.Lerp(target.rotation, rotation, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.rotation.x, rotation.x, 0.01f))
                {
                    target.rotation = rotation;
                    return true;
                }

                return false;
            }
            #endregion

            #region COLORS
            // textmeshpro
            public static bool AlphaTMP(TextMeshProUGUI target, float alpha, float lerpSpeed = 1f)
            {
                float a = Mathf.Lerp(target.color.a, alpha, lerpSpeed * Time.deltaTime);
                target.color = new Color(target.color.r, target.color.g, target.color.b, a);

                if (MathUtil.Tolerance(target.color.a, alpha, 0.01f))
                {
                    target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
                    return true;
                }

                return false;
            }

            // image
            public static bool AlphaImage(Image img, float alpha, float lerpSpeed = 1f)
            {
                Color target = new Color(img.color.r, img.color.g, img.color.b, alpha);
                img.color = Color.Lerp(img.color, target, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(img.color.a, target.a))
                {
                    img.color = target;
                    return true;
                }

                return false;
            }

            // canvas group
            public static bool AlphaCanvasGroup(CanvasGroup group, float alpha, float lerpSpeed = 1f)
            {
                group.alpha = Mathf.Lerp(group.alpha, alpha, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(group.alpha, alpha, 0.01f))
                {
                    group.alpha = alpha;
                    return true;
                }

                return false;
            }

            // alpha jumps
            public static void AlphaJump(CanvasGroup group, float alpha) => group.alpha = alpha;
            public static void AlphaJump(Image image, float alpha) => image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            public static void AlphaJump(TextMeshProUGUI tmp, float alpha) => tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
            #endregion

            #region SCALING
            public static bool Scale(RectTransform target, Vector3 scaleSize, float lerpSpeed = 1f, float negCondition = 0.1f)
            {
                if (target.localScale.x > scaleSize.x)
                    target.localScale = Vector3.Lerp(target.localScale, scaleSize, lerpSpeed * Time.deltaTime);
                else
                    target.localScale = Vector3.Lerp(scaleSize, target.localScale, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.localScale, scaleSize, negCondition))
                {
                    target.localScale = scaleSize;
                    return true;
                }

                return false;
            }
            public static bool Scale(RectTransform target, float scale, float lerpSpeed = 1f, float negCondition = 0.1f)
            {
                Vector3 scaleSize = new Vector3(scale, scale, scale);
                /*if (target.localScale.x > scale)
                    target.localScale = Vector3.Lerp(target.localScale, scaleSize, lerpSpeed * Time.deltaTime);
                else
                    target.localScale = Vector3.Lerp(scaleSize, target.localScale, lerpSpeed * Time.deltaTime);*/
                target.localScale = Vector3.Lerp(target.localScale, scaleSize, lerpSpeed * Time.deltaTime);

                if (MathUtil.Tolerance(target.localScale, scaleSize, negCondition))
                {
                    target.localScale = scaleSize;
                    return true;
                }

                return false;
            }
            public static void SizeSet(RectTransform target, float scale)
            {
                target.localScale = new Vector3(scale, scale, scale);
            }

            #endregion
        }
        #endregion
 
    }
}