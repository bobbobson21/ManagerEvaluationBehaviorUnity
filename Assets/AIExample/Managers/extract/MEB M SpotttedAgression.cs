using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_SpottedAgression_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_SpottedAgression_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_SpottedAgression_UI());
    }

    public UserManger_SpottedAgression_UI()
    {
        m_name = "UserManger_SpottedAgression";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_SpottedAgression";
        data.m_displayName = m_name;
        data.m_displayDiscription = "gets agressive if spotted by our attacker object." +
            "\n\nvaild blackboard data: " +
            "\ngetAttackObjectFrom: (gameObjectBlackboardKeyAsString)" +
            "\nstoreIsAgressiveIn: (boolBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_SpottedAgression : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_getAttackObjectFromKey = "";
    private string m_storeIsAgressiveInKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getAttackObjectFrom")
            {
                m_getAttackObjectFromKey = keys[i];
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
        //put on loaded into game code here
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        GameObject them = (GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey);

        if (them != null)
        {
            MEB_BaseBlackboard themData = them.GetComponent<MEB_BaseBlackboard>();

            if (themData != null)
            {
                GameObject us = (GameObject)themData.GetObject(m_getAttackObjectFromKey);

                if (us == m_director.m_gameObject)
                {
                    m_director.m_blackboard.SetObject(m_storeIsAgressiveInKey, true);
                    return;
                }
            }
        }
    }
}
