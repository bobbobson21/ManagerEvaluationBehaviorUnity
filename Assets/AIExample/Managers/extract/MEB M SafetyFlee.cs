using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_SafetyFlee_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_SafetyFlee_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_SafetyFlee_UI());
    }

    public UserManger_SafetyFlee_UI()
    {
        m_name = "UserManger_SafetyFlee";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_SafetyFlee";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI flee if it see the npc only.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)\ngetAttackObjectFrom: (gameObjectBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_SafetyFlee : MEB_BaseManager, MEB_I_IntScoop
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

    public override void EvaluationEnd(int index, float delta)
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
        //Debug.Log("safty flee");

        GameObject obj = ((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey));
        Vector3 destanation = m_director.m_gameObject.transform.position;

        destanation = destanation + ((m_director.m_gameObject.transform.position -obj.transform.position).normalized * 2);

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, destanation);
    }

    public int GetIntEvalValue(float delta)
    {
        int importance = 0;
        
        GameObject target = ((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey));

        if (target != null)
        {
            importance = 3;

            RaycastHit hitInfo;
            Physics.Linecast(m_director.m_gameObject.transform.position + new Vector3(0, 0, 2), target.transform.position, out hitInfo);

            if(hitInfo.collider != null && hitInfo.collider.gameObject != null)
            {
                if (hitInfo.collider.gameObject == target)
                {
                    importance = 40;
                }
            }
        }
        
        return importance;
    }
}
