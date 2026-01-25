using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_TeamAgression_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_TeamAgression_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_TeamAgression_UI());
    }

    public UserManger_TeamAgression_UI()
    {
        m_name = "UserManger_TeamAgression";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_TeamAgression";
        data.m_displayName = m_name;
        data.m_displayDiscription = "makesus agressive some one sees us or our teammates" +
            "\n\nvaild blackboard data: " +
            "\nstoreIsAgressiveIn: (boolBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_TeamAgression : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_storeIsAgressiveInKey = "";

    private AICTeamOparator m_teamOparator = null;
    private UserBlackboard_BasicBadguy m_ourBlackboard = null;

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
        for (int i = 0; i < m_teamOparator.GetAllOnMyTeam().Count; i++)
        {
            UserBlackboard_BasicBadguy teamMate = (UserBlackboard_BasicBadguy)m_teamOparator.GetBlackboardOfTeamMate(i);
            if (m_ourBlackboard.m_attackerObj != null && m_teamOparator.GetObjectOfTeamMate(i) != m_director.m_gameObject && teamMate.m_attackerObj == m_ourBlackboard.m_attackerObj && m_director.m_gameObject.tag == teamMate.gameObject.tag)
            {
                UserBlackboard_BasicBadguy themData = m_ourBlackboard.m_attackerObj.GetComponent<UserBlackboard_BasicBadguy>();
                if (themData.m_attackerObj == m_director.m_gameObject || themData.m_attackerObj == teamMate.m_attackerObj)
                { 
                    return;
                }
            }
        }

        BlockMoveToExecutionForCycle();
    }

    public override void OnInitialized()
    {
        m_teamOparator = m_director.m_gameObject.GetComponent<AICTeamOparator>();
        m_ourBlackboard = (UserBlackboard_BasicBadguy)m_director.m_blackboard;
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters Executeion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        m_director.m_blackboard.SetObject(m_storeIsAgressiveInKey, true);
    }
}
