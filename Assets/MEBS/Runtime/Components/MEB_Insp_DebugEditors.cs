using MEBS.Editor;
using MEBS.Runtime;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class MEB_Insp_DebugEditorMain : Editor
{
    public static void SendToDebug(MEB_BaseBehaviourData behaviour, MEB_DirectorBase director, UnityEngine.Object runtimeObject, string item)
    {
        for (int i = 0; i < director.GetManagerCount(); i++)
        {
            director.GetManagerByIndex(i).m_itemSettings.m_runtimeManager = director.GetManagerByIndex(i);
        }

        behaviour.m_runtimeName = $"({director.m_gameObject.transform.parent.gameObject.name}) -> ({director.m_gameObject.name}) -> ({item})";
        behaviour.m_runtimeObject = runtimeObject;

        MEB_UI_BehaviourEditor.OpenWindow(behaviour);
    }

    public static void SetPauseExecState(MEB_DirectorBase director, bool unpause)
    {
        director.m_canExec = unpause;
    }

    public static void SetPauseEvalState(MEB_DirectorBase director, bool unpause)
    {
        director.m_canEval = unpause;
    }

    public static bool GetIsNotPausedExecState(MEB_DirectorBase director)
    {
        return director.m_canExec;
    }

    public static bool GetIsNotPausedEvalState(MEB_DirectorBase director)
    {
        return director.m_canEval;
    }
}

[CustomEditor(typeof(MEB_C_Directorlinear))]
public class MEB_Insp_DebugEditor_Directorlinear : Editor
{
    public override void OnInspectorGUI()
    {
        int headerSpacingY = 16;

        DrawDefaultInspector();

        if (Application.isPlaying == true)
        {
            MEB_C_Directorlinear directorLinear = (MEB_C_Directorlinear)target;

            GUILayout.Space(headerSpacingY);
            GUILayout.Label("debug", EditorStyles.boldLabel);
            if (GUILayout.Button("open with debugger"))
            {
                MEB_Insp_DebugEditorMain.SendToDebug(directorLinear.m_behaviorSet, directorLinear.m_directorInterface, directorLinear, "MEB_C_Directorlinear");
            }

            string buttonName = "pause evaluration";

            if (MEB_Insp_DebugEditorMain.GetIsNotPausedEvalState(directorLinear.m_directorInterface) == false)
            {
                buttonName = "un-pause evaluration";
            }

            if (GUILayout.Button(buttonName))
            {
                MEB_Insp_DebugEditorMain.SetPauseEvalState(directorLinear.m_directorInterface, !MEB_Insp_DebugEditorMain.GetIsNotPausedEvalState(directorLinear.m_directorInterface));
            }

            buttonName = "pause execution";

            if (MEB_Insp_DebugEditorMain.GetIsNotPausedExecState(directorLinear.m_directorInterface) == false)
            {
                buttonName = "un-pause execution";
            }

            if (GUILayout.Button(buttonName))
            {
                MEB_Insp_DebugEditorMain.SetPauseExecState(directorLinear.m_directorInterface, !MEB_Insp_DebugEditorMain.GetIsNotPausedExecState(directorLinear.m_directorInterface));
            }
        }
    }
}


[CustomEditor(typeof(MEB_C_DirectorLod))]
public class MEB_Insp_DebugEditor_DirectorLod : Editor
{
    public override void OnInspectorGUI()
    {
        int headerSpacingY = 16;

        DrawDefaultInspector();

        if (Application.isPlaying == true)
        {
            MEB_C_DirectorLod directorLod = (MEB_C_DirectorLod)target;

            GUILayout.Space(headerSpacingY);
            GUILayout.Label("debug", EditorStyles.boldLabel);
            if (GUILayout.Button("open with debugger"))
            {
                MEB_Insp_DebugEditorMain.SendToDebug(directorLod.m_behaviorSet, directorLod.m_directorInterface, directorLod, "MEB_C_Directorlod");
            }

            string buttonName = "pause evaluration";

            if (MEB_Insp_DebugEditorMain.GetIsNotPausedEvalState(directorLod.m_directorInterface) == false)
            {
                buttonName = "un-pause evaluration";
            }

            if (GUILayout.Button(buttonName))
            {
                MEB_Insp_DebugEditorMain.SetPauseEvalState(directorLod.m_directorInterface, !MEB_Insp_DebugEditorMain.GetIsNotPausedEvalState(directorLod.m_directorInterface));
            }

            buttonName = "pause execution";

            if (MEB_Insp_DebugEditorMain.GetIsNotPausedExecState(directorLod.m_directorInterface) == false)
            {
                buttonName = "un-pause execution";
            }

            if (GUILayout.Button(buttonName))
            {
                MEB_Insp_DebugEditorMain.SetPauseExecState(directorLod.m_directorInterface, !MEB_Insp_DebugEditorMain.GetIsNotPausedExecState(directorLod.m_directorInterface));
            }
        }
    }
}


[CustomEditor(typeof(MEB_C_DirectorThreaded))]
public class MEB_Insp_DebugEditor_DirectorThreaded : Editor
{
    public override void OnInspectorGUI()
    {
        int headerSpacingY = 16;

        DrawDefaultInspector();

        if (Application.isPlaying == true)
        {
            MEB_C_DirectorThreaded directorThreaded = (MEB_C_DirectorThreaded)target;

            GUILayout.Space(headerSpacingY);
            GUILayout.Label("debug", EditorStyles.boldLabel);
            if (GUILayout.Button("open with debugger"))
            {
                MEB_Insp_DebugEditorMain.SendToDebug(directorThreaded.m_behaviorSet, directorThreaded.m_directorInterface, directorThreaded, "MEB_C_DirectorThreaded");
            }

            string buttonName = "pause evaluration";

            if (MEB_Insp_DebugEditorMain.GetIsNotPausedEvalState(directorThreaded.m_directorInterface) == false)
            {
                buttonName = "un-pause evaluration";
            }

            if (GUILayout.Button(buttonName))
            {
                MEB_Insp_DebugEditorMain.SetPauseEvalState(directorThreaded.m_directorInterface, !MEB_Insp_DebugEditorMain.GetIsNotPausedEvalState(directorThreaded.m_directorInterface));
            }

            buttonName = "pause execution";

            if (MEB_Insp_DebugEditorMain.GetIsNotPausedExecState(directorThreaded.m_directorInterface) == false)
            {
                buttonName = "un-pause execution";
            }

            if (GUILayout.Button(buttonName))
            {
                MEB_Insp_DebugEditorMain.SetPauseExecState(directorThreaded.m_directorInterface, !MEB_Insp_DebugEditorMain.GetIsNotPausedExecState(directorThreaded.m_directorInterface));
            }
        }
    }
}

#endif