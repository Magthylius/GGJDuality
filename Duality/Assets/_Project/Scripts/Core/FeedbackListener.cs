using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Duality.Core
{
    public class FeedbackListener : MonoBehaviour
    {
        public MMFeedbackFloatingText floatingText;

        public void SetFloatingTextPosition(Vector3 position)
        {
            floatingText.DirectSetPosition(position);
        }
    }
}
