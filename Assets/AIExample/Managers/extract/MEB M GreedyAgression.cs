using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Manager_GreedyAgressionSettings : MEB_BaseBehaviourData_ItemSettings
{
    public int m_greedAmount = 45;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            int.TryParse(EditorGUILayout.TextField("greedy limit", m_greedAmount.ToString()), out m_greedAmount);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_GreedyAgression_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GreedyAgression_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManagerWithScopeRequirement(new UserManger_GreedyAgression_UI());
    }

    public UserManger_GreedyAgression_UI()
    {
        m_name = "UserManger_GreedyAgression";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_GreedyAgressionSettings data = new Manager_GreedyAgressionSettings();
        data.m_class = "UserManger_GreedyAgression";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI agressive if the amout of items needed is to high." +
            "\n\nvaild blackboard data: " +
            "\ngetResourceCountFrom :(intBlackboardKeyAsString)" +
            "\ngetdesiredResourceCountFrom :(intBlackboardKeyAsString)" +
            "\nstoreIsAgressiveIn: (boolBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_GreedyAgression : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_getResourceCountFromKey = "";
    private string m_getdesiredResourceCountFromKey = "";
    private string m_storeIsAgressiveInKey = "";

    private int m_greedAmount = 45;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getResourceCountFrom")
            {
                m_getResourceCountFromKey = keys[i];
            }

            if (idenifyers[i] == "getdesiredResourceCountFrom")
            {
                m_getdesiredResourceCountFromKey = keys[i];
            }

            if (idenifyers[i] == "storeIsAgressiveIn")
            {
                m_storeIsAgressiveInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        Manager_GreedyAgressionSettings settings = (Manager_GreedyAgressionSettings)m_itemSettings;

        if (settings != null)
        {
            m_greedAmount = settings.m_greedAmount;
        }
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        if (((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) <= m_greedAmount) { return; }
        if (((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) >= ((int)m_director.m_blackboard.GetObject(m_getdesiredResourceCountFromKey))) { return; }

        if (((int)m_director.m_blackboard.GetObject("health")) <= 50 || ((int)m_director.m_blackboard.GetObject("ammoCurrentClip")) <= ((int)m_director.m_blackboard.GetObject("ammoMax")) /3) { return; }

        m_director.m_blackboard.SetObject(m_storeIsAgressiveInKey, true);
    }
}
