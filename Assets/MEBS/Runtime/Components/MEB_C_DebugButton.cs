using MEBS.Editor;
using MEBS.Runtime;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(MEB_C_Directorlinear))]
public class MEB_C_DebugButton : Editor
{
    private static void SendToDebug(MEB_BaseBehaviourData behaviour, MEB_DirectorBase director, UnityEngine.Object runtimeObject, string item)
    {
        for (int i = 0; i < director.GetManagerCount(); i++)
        {
            director.GetManagerByIndex(i).m_itemSettings.m_runtimeManager = director.GetManagerByIndex(i);
        }

        behaviour.m_runtimeName = $"({director.m_gameObject.transform.parent.gameObject.name}) -> ({director.m_gameObject.name}) -> ({item})";
        behaviour.m_runtimeObject = runtimeObject;

        MEB_UI_BehaviourEditor.OpenWindow(behaviour);
    }

    public override void OnInspectorGUI()
    {
        int headerSpacingY = 16;

        DrawDefaultInspector();

        if (Application.isPlaying == true)
        {
            GUILayout.Space(headerSpacingY);
            GUILayout.Label("debug", EditorStyles.boldLabel);
            if (GUILayout.Button("open with debugger"))
            {
                MEB_C_Directorlinear directorLinear = (MEB_C_Directorlinear)target;

                if (directorLinear != null)
                {
                    SendToDebug(directorLinear.m_behaviorSet, directorLinear.m_directorInterface, directorLinear, "MEB_C_Directorlinear"); 
                    return;
                }

                MEB_C_DirectorLod directorLOD = (MEB_C_DirectorLod)target;

                if (directorLOD != null)
                {
                    SendToDebug(directorLOD.m_behaviorSet, directorLOD.m_directorInterface, directorLOD, "MEB_C_DirectorLod");
                    return;
                }

                MEB_C_DirectorThreaded directorThreaded = (MEB_C_DirectorThreaded)target;

                if (directorThreaded != null)
                {
                    SendToDebug(directorThreaded.m_behaviorSet, directorThreaded.m_directorInterface, directorThreaded, "MEB_C_DirectorThreaded");
                    return;
                }
            }
        }
    }
}

#endif