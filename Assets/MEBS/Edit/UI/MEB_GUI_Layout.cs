#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace MEBS.Editor
{
    public class MEB_GUI_Layout
    {
        public static void LockedInputStyle(string text, GUIStyle style = null)
        {
            int inputHeight = 16;

            if (style == null)
            {
                style = EditorStyles.textField;
            }

            GUILayout.BeginVertical(style, GUILayout.ExpandWidth(true), GUILayout.Height(inputHeight));
            EditorGUILayout.SelectableLabel(text, MEB_GUI_Styles.GetLockedStyle(), GUILayout.ExpandWidth(false), GUILayout.MinWidth(0), GUILayout.Height(inputHeight));
            GUILayout.EndVertical();
        }

        public static void LockedInputStyle(string lable, string text, GUIStyle style = null)
        {
            int lableWidth = 148;
            int inputHeight = 16;

            if (style == null)
            {
                style = EditorStyles.textField;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(lable, GUILayout.Width(lableWidth), GUILayout.MinWidth(lableWidth), GUILayout.MaxWidth(lableWidth));

            GUILayout.BeginVertical(style, GUILayout.ExpandWidth(true), GUILayout.Height(inputHeight));
            EditorGUILayout.SelectableLabel(text, MEB_GUI_Styles.GetLockedStyle(), GUILayout.ExpandWidth(false), GUILayout.MinWidth(0), GUILayout.Height(inputHeight));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        public static void BeginAffectBox(Color col, params GUILayoutOption[] options)
        {
            col.a = 0.4f;

            int xPadding = 2;
            int yPadding = 2;

            GUIStyle style = new GUIStyle();
            style.normal.background = new Texture2D(1, 1);
            style.normal.background.SetPixel(0, 0, col);
            style.normal.background.Apply();

            GUILayout.BeginHorizontal();
            GUILayout.Space(xPadding);

            GUILayout.BeginVertical(style);
            GUILayout.Space(yPadding *4);
        }

        public static void EndAffectBox()
        {
            int xPadding = 2;
            int yPadding = 2;

            GUILayout.Space(yPadding);
            GUILayout.EndVertical();

            GUILayout.Space(xPadding);
            GUILayout.EndHorizontal();

            GUILayout.Space(yPadding);
        }
    }
}

#endif