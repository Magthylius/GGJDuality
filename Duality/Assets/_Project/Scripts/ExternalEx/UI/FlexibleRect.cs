using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    namespace UI
    {
        [System.Serializable]
        public class FlexibleRect : TickerObject
        {
            public RectTransform rectTransform;
            public Vector2 originalPosition;
            public Vector2 targetPosition;

            public LerpSettings positionLerpSettings = new LerpSettings(5f, 0.1f);

            public Action DestinationReachedEvent;
            
            Vector2 _endPosition;
            bool _isMovingAway = true;

            ///<summary> For easy movement of RectTransform with lerp utilities. </summary>
            public FlexibleRect(RectTransform rectTr)
            {
                rectTransform = rectTr;
                originalPosition = center;

                targetPosition = Vector2.zero;
                _endPosition = Vector2.zero;
                
                _isMovingAway = true;
            }
            public FlexibleRect(RectTransform rectTr, Vector2 targetPos)
            {
                rectTransform = rectTr;
                originalPosition = center;

                targetPosition = targetPos;
                _endPosition = Vector2.zero;

                _isMovingAway = true;
            }

            public void Step(float speed, float precision = 0.1f)
            {
                
            }

            public override void Update()
            {
                if (positionLerpSettings.AllowTransition)
                {
                    positionLerpSettings.AllowTransition = !LerpPosition(_endPosition, positionLerpSettings.Speed * Time.deltaTime, positionLerpSettings.Precision);
                }
            }

            //! Lerp
            public void StartLerp()
            {
                positionLerpSettings.AllowTransition = true;
                _isMovingAway = !_isMovingAway;
                DetermineEndPosition();
            }

            public void StartLerp(bool movingAway)
            {
                positionLerpSettings.AllowTransition = true;
                _isMovingAway = movingAway;

                DetermineEndPosition();
            }
            public void StartLerp(Vector2 endPos)
            {
                positionLerpSettings.AllowTransition = true;
                _endPosition = endPos;

                _isMovingAway = true;
            }

            void DetermineEndPosition()
            {
                if (_isMovingAway) _endPosition = targetPosition;
                else _endPosition = originalPosition;
            }

            public void EndLerp()
            {
                positionLerpSettings.AllowTransition  = false;
            }

            public bool LerpPosition(Vector2 targetPosition, float speed, float precision = 0.1f)
            {
                Vector2 destination = Vector2.Lerp(center, targetPosition, speed);

                if ((targetPosition - destination).sqrMagnitude <= precision * precision)
                {
                    MoveTo(targetPosition);
                    DestinationReachedEvent?.Invoke();
                    return true;
                }

                MoveTo(destination);
                return false;
            }

            /*public bool LerpSize(Vector2 targetSize, float speed, float precision = 0.1f)
            {
                Vector2 destination = Vector2.Lerp(rectTransform.sizeDelta, targetSize, speed);

                if ((targetSizeDelta - destination).sqrMagnitude <= precision * precision)
                {
                    //MoveTo(targetPosition);
                    rectTransform.sizeDelta = targetSize;
                    return true;
                }

                rectTransform.sizeDelta = destination;
                return false;
            }*/
            
            public void LerpUnsnapped(Vector2 targetPosition, float progress)
            {
                Vector2 destination = Vector2.Lerp(center, targetPosition, progress);

                MoveTo(destination);
            }

            //! Movement
            public void MoveTo(Vector2 targetPosition)
            {
                Vector2 diff = targetPosition - center;
                rectTransform.offsetMax += diff;
                rectTransform.offsetMin += diff;
            }

            public void MoveToStart() => MoveTo(originalPosition);
            public void MoveToEnd() => MoveTo(targetPosition);
            
            //! Size
            public void Resize(Vector2 newSize) => rectTransform.sizeDelta = newSize;
            public void ResizeX(float newSizeX) => rectTransform.sizeDelta = new Vector2(newSizeX, rectTransform.sizeDelta.y);
            public void ResizeY(float newSizeY) => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newSizeY);

            //! Setters
            public void SetMovingAway(bool status) => _isMovingAway = status;
            public void ToggleMovement() => _isMovingAway = !_isMovingAway;
            public void SetTargetPosition(Vector2 targetPos) => targetPosition = targetPos;
            public void SetEndPosition(Vector2 targetPos) => _endPosition = targetPos;

            //! Queries
            public Vector2 GetBodyOffset(Vector2 direction)
            {
                return new Vector2(originalPosition.x + (direction.x * width), originalPosition.y + (direction.y * height));
            }
            public Vector2 GetBodyOffset(Vector2 direction, float degreeOfSelf)
            {
                return new Vector2(originalPosition.x + (direction.normalized.x * degreeOfSelf * halfWidth), originalPosition.y + (direction.normalized.y * degreeOfSelf * halfHeight));
            }
            
            //! Statics
            public static Vector2 GetCenterPos(RectTransform otherTransform)
            {
                return (otherTransform.offsetMax + otherTransform.offsetMin) * 0.5f;
            }

            public float AngleFromOriginRad => Mathf.Atan2(center.y - originalPosition.y, center.x - originalPosition.x);
            public float AngleFromOriginDeg => AngleFromOriginRad * Mathf.Rad2Deg;
            public float DistanceFromOrigin => Vector2.Distance(center, originalPosition);
            public Vector2 ofMin => rectTransform.offsetMin;
            public Vector2 ofMax => rectTransform.offsetMax;
            public Vector2 center => (ofMin + ofMax) * 0.5f;
            public Vector2 centerPivoted => (ofMin + ofMax) * 0.5f * rectTransform.pivot;
            public float width => rectTransform.rect.width;
            public float height => rectTransform.rect.height;
            public float halfWidth => width * 0.5f;
            public float halfHeight => height * 0.5f;
            public virtual bool IsTransitioning => positionLerpSettings.AllowTransition;
            public bool IsMovingAway => _isMovingAway;
        }

    }
}