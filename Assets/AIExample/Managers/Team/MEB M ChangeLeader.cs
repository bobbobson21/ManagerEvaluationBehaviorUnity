using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_ChangeTeamLeader_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_ChangeTeamLeader_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_ChangeTeamLeader_UI());
    }

    public UserManger_ChangeTeamLeader_UI()
    {
        m_name = "UserManger_ChangeTeamLeader";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_ChangeTeamLeader";
        data.m_displayName = m_name;
        data.m_displayDiscription = "changes the leader of this AIs team base on given factors";

        return data;
    }
}
#endif

public class UserManger_ChangeTeamLeader : MEB_BaseManager//, MEB_I_IntScoop
{
    private AICTeamOparator m_teamOparator = null;
    private UserBlackboard_BasicBadguy m_ourBlackboard = null;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void EvaluationEnd(int index, float delta)
    {
        if (m_ourBlackboard.m_health <= m_ourBlackboard.m_healthMax /4)
        {
            return;
        }

        if (m_ourBlackboard.m_resourceCount < m_ourBlackboard.m_desiredResourceCount)
        {
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        m_teamOparator = m_director.m_gameObject.GetComponent<AICTeamOparator>();
        m_ourBlackboard = (UserBlackboard_BasicBadguy)m_director.m_blackboard;
    }

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
        for (int i = 0; i < m_teamOparator.GetAllOnMyTeam().Count; i++)
        {
            UserBlackboard_BasicBadguy teamMate = (UserBlackboard_BasicBadguy)m_teamOparator.GetBlackboardOfTeamMate(i);

            if (teamMate.m_resourceCount < teamMate.m_desiredResourceCount && m_director.m_gameObject.tag == teamMate.gameObject.tag)
            {
                m_teamOparator.SetMyLeader(teamMate.gameObject);
                return;
            }
        }
    }

    public override void OnEnd()
    {
    }

    public override void OnUpdate(float delta, int index)
    {
        //put update code here
    }
}
