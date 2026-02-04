using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_DeathHandler_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_DeathHandler_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_DeathHandler_UI());
    }

    public UserManger_DeathHandler_UI()
    {
        m_name = "UserManger_DeathHandler";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_DeathHandler";
        data.m_displayName = m_name;
        data.m_displayDiscription = "make the npc die correctly";

        return data;
    }
}
#endif

public class UserManger_DeathHandler : MEB_BaseManager//, MEB_I_IntScoop
{
    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void EvaluationEnd(int index, float delta)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        //put on loaded into game code here
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters Executeion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        bool hadNoHealth = (((int)m_director.m_blackboard.GetObject("health")) <= 0);

        if (hadNoHealth == false)
        {
            return;
        }

        Debug.Log($"AI {m_director.m_gameObject} has died.");

        m_director.m_gameObject.tag = "deadAI";

        MEB_C_Directorlinear directior = m_director.m_gameObject.GetComponent<MEB_C_Directorlinear>();
        MEB_C_DirectorLod directiorlod = m_director.m_gameObject.GetComponent<MEB_C_DirectorLod>();

        NavMeshAgent agent = m_director.m_gameObject.GetComponent<NavMeshAgent>();
        Rigidbody solidBody = m_director.m_gameObject.GetComponent<Rigidbody>();
        Collider solidCollider = m_director.m_gameObject.GetComponent<Collider>();

        if (directior != null) { Object.Destroy(directior); }
        if (directiorlod != null) { Object.Destroy(directiorlod); }

        if (agent != null) { Object.Destroy(agent); }
        if (solidBody != null) { Object.Destroy(solidBody); }
        if (solidCollider != null) { Object.Destroy(solidCollider); }
    }
}
