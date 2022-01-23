using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Magthylius.Attributes
{
    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }

    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var previousGUIState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = previousGUIState;
        }
    }

    [CustomPropertyDrawer(typeof(MinMaxAttribute))]
    public class MinMaxAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            MinMaxAttribute minMaxAttribute = (MinMaxAttribute)attribute;
            float minCap = minMaxAttribute.range.x;
            float maxCap = minMaxAttribute.range.y;

            Rect controlRect = EditorGUI.PrefixLabel(position, label);
            Rect[] splitRect = SplitRect(controlRect, 3);

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                float minVal = property.vector2Value.x;
                float maxVal = property.vector2Value.y;

                EditorGUI.BeginChangeCheck();
                minVal = EditorGUI.FloatField(splitRect[0], minVal);
                maxVal = EditorGUI.FloatField(splitRect[2], maxVal);

                EditorGUI.MinMaxSlider(splitRect[1], ref minVal, ref maxVal, minCap, maxCap);
                minVal = Mathf.Max(minCap, minVal);
                maxVal = Mathf.Min(maxCap, maxVal);

                Vector2 propertySet = new Vector2(minVal < maxVal ? minVal : maxVal, maxVal);
                if (EditorGUI.EndChangeCheck()) property.vector2Value = propertySet;
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                float minVal = property.vector2IntValue.x;
                float maxVal = property.vector2IntValue.y;

                EditorGUI.BeginChangeCheck();
                minVal = EditorGUI.FloatField(splitRect[0], minVal);
                maxVal = EditorGUI.FloatField(splitRect[2], maxVal);

                EditorGUI.MinMaxSlider(splitRect[1], ref minVal, ref maxVal, minCap, maxCap);
                minVal = Mathf.Max(minCap, minVal);
                maxVal = Mathf.Min(maxCap, maxVal);

                Vector2Int propertySet = new Vector2Int(Mathf.FloorToInt(minVal < maxVal ? minVal : maxVal), Mathf.FloorToInt(maxVal));
                if (EditorGUI.EndChangeCheck()) property.vector2IntValue = propertySet;
            }
        }

        Rect[] SplitRect(Rect rectToSplit, int n)
        {
            Rect[] rects = new Rect[n];

            for (int i = 0; i < n; i++)
            {
                rects[i] = new Rect(rectToSplit.position.x + (i * rectToSplit.width / n), rectToSplit.position.y, rectToSplit.width / n, rectToSplit.height);
            }

            int padding = (int)rects[0].width - 40;
            int space = 5;

            rects[0].width -= padding + space;
            rects[2].width -= padding + space;

            rects[1].x -= padding;
            rects[1].width += padding * 2;

            rects[2].x += padding + space;

            return rects;
        }
    }


}

