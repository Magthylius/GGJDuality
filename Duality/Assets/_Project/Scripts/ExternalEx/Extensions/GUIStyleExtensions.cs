using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magthylius
{
    public static class GUIStyleEx
    {
        public static GUIStyle SetFontColor(this GUIStyle style, Color fontColor)
        {
            style.normal.textColor = fontColor;
            return style;
        }
        
        public static GUIStyle Header
        {
            get
            {
                GUIStyle header = new GUIStyle();
                header.fontStyle = FontStyle.Bold;
                header.normal.textColor = Color.white;
                header.fontSize = 15;

                return header;
            }
        }
        
        public static GUIStyle Comment
        {
            get
            {
                GUIStyle header = new GUIStyle();
                header.fontStyle = FontStyle.Italic;
                header.normal.textColor = Color.white;
                header.fontSize = 10;

                return header;
            }
        }
    }
}
