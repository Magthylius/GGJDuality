using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    /// <summary>Attribute class for layer properties.</summary>
    public class LayerAttribute : PropertyAttribute
    {
    }

    /// <summary>Attribute class for tag properties.</summary>
    public class TagAttribute : PropertyAttribute
    {
    }

    /// <summary>Attribute class for read-only properties.</summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }

    public class MinMaxAttribute : PropertyAttribute
    {
        public readonly Vector2 range;
        public MinMaxAttribute(Vector2 minMaxRange)
        {
            range = minMaxRange;
        }

        public MinMaxAttribute(float minimum, float maximum)
        {
            range = new Vector2(minimum, maximum);
        }
    }
}