using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Manager_InitalAgressionSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_allowAgressionInMin = 4;
    public float m_allowAgressionInMax = 6;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            if (MEB_UI_BehaviourEditor.InRestrictedEditMode() == false)
            {
                float.TryParse(EditorGUILayout.TextField("allow agression in min", m_allowAgressionInMin.ToString()), out m_allowAgressionInMin);
                float.TryParse(EditorGUILayout.TextField("allow agression in max", m_allowAgressionInMax.ToString()), out m_allowAgressionInMax);
            }
            else
            {
                MEB_GUI_Styles.BeginTextStyleWithLockedColor();
                EditorGUILayout.TextField("allow agression in min", m_allowAgressionInMin.ToString());
                EditorGUILayout.TextField("allow agression in max", m_allowAgressionInMax.ToString());
                MEB_GUI_Styles.EndTextStyle();
            }
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_InitalAgression_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_InitalAgression_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_InitalAgression_UI());
    }

    public UserManger_InitalAgression_UI()
    {
        m_name = "UserManger_InitalAgression";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_InitalAgressionSettings data = new Manager_InitalAgressionSettings();
        data.m_class = "UserManger_InitalAgression";
        data.m_displayName = m_name;
        data.m_displayDiscription = "makes us not agressive untill a certain amout of time has passed." +
            "\n\nvaild blackboard data: " +
            "\nstoreIsAgressiveIn: (boolBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_InitalAgression : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_storeIsAgressiveInKey = "";

    private float m_allowAgressionInTime = 4;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeIsAgressiveIn")
            {
                m_storeIsAgressiveInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        m_allowAgressionInTime -= delta;

        if (m_allowAgressionInTime <= 0)
        {
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        Manager_InitalAgressionSettings settings = (Manager_InitalAgressionSettings)m_itemSettings;

        if (settings != null)
        {
            m_allowAgressionInTime = Random.Range(settings.m_allowAgressionInMin, settings.m_allowAgressionInMax);
        }
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters Executeion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        m_director.m_blackboard.SetObject(m_storeIsAgressiveInKey, false);
    }
}
