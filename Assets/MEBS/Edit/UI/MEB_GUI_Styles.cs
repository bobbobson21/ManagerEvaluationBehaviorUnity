#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MEBS.Editor
{
    public class MEB_GUI_Styles
    {
        private static Color m_oldCol_Norm = Color.white;
        private static Color m_oldCol_Hover = Color.white;
        private static Color m_oldCol_Act = Color.white;

        public static void BeginLockedTextStyle()
        {
            m_oldCol_Norm = EditorStyles.textField.normal.textColor;
            EditorStyles.textField.normal.textColor = Color.gray;

            m_oldCol_Hover = EditorStyles.textField.hover.textColor;
            EditorStyles.textField.hover.textColor = Color.gray;

            m_oldCol_Act = EditorStyles.textField.active.textColor;
            EditorStyles.textField.active.textColor = Color.gray;
        }

        public static void EndLockedTextStyle()
        {
            EditorStyles.textField.normal.textColor = m_oldCol_Norm;
            EditorStyles.textField.hover.textColor = m_oldCol_Hover;
            EditorStyles.textField.active.textColor = m_oldCol_Act;
        }

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