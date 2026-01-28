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
            int lableWidth = 400;
            int inputHeight = 16;

            if (style == null)
            {
                style = EditorStyles.textField;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(text, GUILayout.Width(lableWidth), GUILayout.MinWidth(lableWidth), GUILayout.MaxWidth(lableWidth));

            GUILayout.BeginVertical(style, GUILayout.ExpandWidth(true), GUILayout.Height(inputHeight));
            EditorGUILayout.SelectableLabel(text, MEB_GUI_Styles.GetLockedStyle(), GUILayout.ExpandWidth(false), GUILayout.MinWidth(0), GUILayout.Height(inputHeight));
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }
}

#endif