using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_Flee_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_Flee_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_Flee_UI());
    }

    public UserManger_Flee_UI()
    {
        m_name = "UserManger_Flee";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_Flee";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI flee.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)\ngetAttackObjectFrom: (gameObjectBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_Flee : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";
    private string m_getAttackObjectFromKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }

            if (idenifyers[i] == "getAttackObjectFrom")
            {
                m_getAttackObjectFromKey = keys[i];
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

    /*public virtual void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public virtual void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        //put update code here
    }

    public int GetIntEvalValue()
    {
        if (((int)m_director.m_blackboard.GetObject("health")) <= 25 || ((int)m_director.m_blackboard.GetObject("ammo")) <= 5)
        {
            if (((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey)) != null)
            {
                return 100;
            }
        }

        return 0;
    }
}
