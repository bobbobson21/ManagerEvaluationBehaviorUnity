#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MEBS.Editor
{
    internal class MEB_GUI_Styles
    {
        public static GUIStyle TitleTextStyle() //the title style used on the main page
        {
            GUIStyle textStyle = new GUIStyle();

            textStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);
            textStyle.fontSize = 24;
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.alignment = TextAnchor.UpperLeft;
            textStyle.contentOffset = new Vector2(4, 2);
            textStyle.wordWrap = true;

            if (EditorGUIUtility.isProSkin == false) //light mode color
            {
                textStyle.normal.textColor = Color.black;
            }

            return textStyle;
        }

        public static GUIStyle NormalTextStyle() //issue details font
        {
            GUIStyle textStyle = new GUIStyle();

            textStyle.normal.textColor = new Color(0.80f, 0.80f, 0.80f);
            textStyle.fontSize = 12;
            textStyle.fontStyle = FontStyle.Normal;
            textStyle.alignment = TextAnchor.UpperLeft;
            textStyle.contentOffset = new Vector2(4, 2);
            textStyle.wordWrap = true;

            if (EditorGUIUtility.isProSkin == false) //light mode color
            {
                textStyle.normal.textColor = Color.black;
            }

            return textStyle;
        }
    }
}

#endif