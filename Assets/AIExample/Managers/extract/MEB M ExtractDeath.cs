using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_FailedToExtractDeath_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_FailedToExtractDeath_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_FailedToExtractDeath_UI());
    }

    public UserManger_FailedToExtractDeath_UI()
    {
        m_name = "UserManger_FailedToExtractDeath";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_GoToExtractSettings data = new Manager_GoToExtractSettings();
        data.m_class = "UserManger_FailedToExtractDeath";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Kills the AI after the time has passed if it has not extracted.";
        data.m_lable = "kill AI in";

        return data;
    }
}
#endif

public class UserManger_FailedToExtractDeath : MEB_BaseManager//, MEB_I_IntScoop
{
    private float m_lifeTime = 20.0f;
    private bool m_doneJob = false;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void EvaluationEnd(int index, float delta)
    {
        if (m_doneJob == true) { BlockMoveToExecutionForCycle(); }
    }

    public override void OnInitialized()
    {
        Manager_GoToExtractSettings settings = (Manager_GoToExtractSettings)m_itemSettings;

        if (settings != null)
        {
            m_lifeTime = settings.m_extractIn;
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
        m_lifeTime -= delta;

        if (m_lifeTime <= 0)
        {
            Debug.Log($"AI ({m_director.m_gameObject.transform.parent.gameObject}) failed to extract in time and they will now die.");
            m_director.m_blackboard.SetObject("health", 0);
            m_doneJob = true;
        }
    }
}
