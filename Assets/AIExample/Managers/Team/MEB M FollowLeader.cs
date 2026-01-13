using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_FollowLeader_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_FollowLeader_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_FollowLeader_UI());
    }

    public UserManger_FollowLeader_UI()
    {
        m_name = "UserManger_FollowLeader";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_FollowLeader";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI follow its team leader.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_FollowLeader : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";

    private AICTeamOparator m_teamOparator = null;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        m_teamOparator = m_director.m_gameObject.GetComponent<AICTeamOparator>();
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        GameObject obj = m_teamOparator.GetLeader();
        Vector3 destanation = m_director.m_gameObject.transform.position;

        if ((obj.transform.position - m_director.m_gameObject.transform.position).magnitude > 6)
        {
            destanation = destanation + ((obj.transform.position - m_director.m_gameObject.transform.position).normalized * 2);
        }

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, destanation);

    }

    public int GetIntEvalValue(float delta)
    {
        return 1;
    }
}
