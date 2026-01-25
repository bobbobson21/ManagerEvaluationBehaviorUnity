#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace MEBS.Editor
{
    public class MEB_GUI_Styles
    {
        private static Color m_oldCol_Norm = Color.white;
        private static Color m_oldCol_Hover = Color.white;
        private static Color m_oldCol_Act = Color.white;

        private static Color m_oldCol_OnNorm = Color.white;
        private static Color m_oldCol_OnHover = Color.white;
        private static Color m_oldCol_OnAct = Color.white;

        public static void BeginTextStyle(Color col)
        {
            m_oldCol_Norm = EditorStyles.textField.normal.textColor;
            EditorStyles.textField.normal.textColor = col;

            m_oldCol_Hover = EditorStyles.textField.hover.textColor;
            EditorStyles.textField.hover.textColor = col;

            m_oldCol_Act = EditorStyles.textField.active.textColor;
            EditorStyles.textField.active.textColor = col;

            m_oldCol_OnNorm = EditorStyles.textField.onNormal.textColor;
            EditorStyles.textField.onNormal.textColor = col;

            m_oldCol_OnHover = EditorStyles.textField.onHover.textColor;
            EditorStyles.textField.onHover.textColor = col;

            m_oldCol_OnAct = EditorStyles.textField.onActive.textColor;
            EditorStyles.textField.onActive.textColor = col;
        }

        public static void EndTextStyle()
        {
            EditorStyles.textField.normal.textColor = m_oldCol_Norm;
            EditorStyles.textField.hover.textColor = m_oldCol_Hover;
            EditorStyles.textField.active.textColor = m_oldCol_Act;

            EditorStyles.textField.onNormal.textColor = m_oldCol_OnNorm;
            EditorStyles.textField.onHover.textColor = m_oldCol_OnHover;
            EditorStyles.textField.onActive.textColor = m_oldCol_OnAct;
        }

        public static void BeginTextStyleWithLockedColor()
        {
            BeginTextStyle(Color.gray);
        }

        public static void BeginTextStyleWithGameObjectColor()
        {
            BeginTextStyle(Color.cyan);
        }

        public static void BeginTextStyleWithVectorColor()
        {
            BeginTextStyle(Color.yellow);
        }

        public static void BeginTextStyleWithBoolColor()
        {
            BeginTextStyle(Color.darkRed);
        }

        public static void BeginTextStyleWithNuberColor()
        {
            BeginTextStyle(Color.green);
        }

        public static GUIStyle BarStyle() //the title style used on the main page
        {
            GUIStyle style = new GUIStyle();

            style.normal.background = new Texture2D(1, 1);
            style.normal.background.SetPixel( 0, 0, new Color(0, 0, 0, 0.2f));
            style.normal.background.Apply();

            return style;
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