using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_GoToExtractAsTeam_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GoToExtractAsTeam_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_GoToExtractAsTeam_UI());
    }

    public UserManger_GoToExtractAsTeam_UI()
    {
        m_name = "UserManger_GoToExtractAsTeam";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_GoToExtractSettings data = new Manager_GoToExtractSettings();
        data.m_class = "UserManger_GoToExtractAsTeam";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI go to extract.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_GoToExtractAsTeam : MEB_BaseManager//, MEB_I_IntScoop
{
    private float m_extractIn = 0;

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
        bool shouldBlock = false;
        bool foundAnExit = false;
        m_extractIn -= delta;

        for (int i = 0; i < m_teamOparator.GetAllOnMyTeam().Count; i++)
        {
            UserBlackboard_BasicBadguy teammate = (UserBlackboard_BasicBadguy)m_teamOparator.GetBlackboardOfTeamMate(i);

            if (teammate.m_extractObject != null)
            {
                foundAnExit = true;
            }

            if (teammate.m_resourceCount <= teammate.m_desiredResourceCount)
            {
                shouldBlock = true;
                return;
            }
        }

        if (m_extractIn <= 0)
        {
            shouldBlock = false;
        }

        if (shouldBlock == true || foundAnExit == false)
        {
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        m_teamOparator = m_director.m_gameObject.GetComponent<AICTeamOparator>();
    }

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
        m_director.m_blackboard.SetObject("wantsToExtract", true);
    }

    public override void OnEnd()
    {
    }

    public override void OnUpdate(float delta, int index)
    {
        float dist = float.MaxValue;
        GameObject extractPoint = null;

        for (int i = 0; i < m_teamOparator.GetAllOnMyTeam().Count; i++)
        {
            UserBlackboard_BasicBadguy teammate = (UserBlackboard_BasicBadguy)m_teamOparator.GetBlackboardOfTeamMate(i);

            if(teammate)

        }
    }
}
