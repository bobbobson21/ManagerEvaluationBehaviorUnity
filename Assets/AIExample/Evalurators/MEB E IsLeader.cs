using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_Eval_IsLeader_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_Eval_IsLeader_UI()
    {
        MEB_UI_BehaviourEditor.AddEvalManager(new UserManger_Eval_IsLeader_UI());
    }

    public UserManger_Eval_IsLeader_UI()
    {
        m_name = "UserManger_Eval_IsLeader";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_Eval_IsLeader";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Allows exacuteion if the AI is the leader of its team.";

        return data;
    }
}
#endif

public class UserManger_Eval_IsLeader : MEB_BaseManager, MEB_I_EvalScoop
{
    private int m_startPointOfScope = 0;
    private int m_endPointOfScope = 0;

    private AICTeamOparator m_teamOparator = null;

    public void SetEvaluationScope(int start, int end)
    {
        m_startPointOfScope = start;
        m_endPointOfScope = end;
    }

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void OnInitialized()
    {
        m_teamOparator = m_director.m_gameObject.GetComponent<AICTeamOparator>();
    }

    public override void EvaluationStart(int index, float delta)
    { 
        int arrayLength = (m_endPointOfScope - m_startPointOfScope);
        bool isLeader = m_teamOparator.IsLeader();

        for (int i = 0; i < arrayLength; i++)
        {
            int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i; //calulates the managers true index
            MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex); //gets the manager

            if (isLeader == false)
            {
                manager.BlockMoveToExecutionForCycle();
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        BlockMoveToExecutionForCycle();
    }
}
