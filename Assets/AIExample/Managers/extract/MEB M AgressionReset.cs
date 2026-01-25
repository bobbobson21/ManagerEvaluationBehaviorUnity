using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_AgressionReset_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_AgressionReset_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManagerWithScopeRequirement(new UserManger_AgressionReset_UI());
    }

    public UserManger_AgressionReset_UI()
    {
        m_name = "UserManger_AgressionReset";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_AgressionReset";
        data.m_displayName = m_name;
        data.m_displayDiscription = "resets agression levels" +
            "\n\nvaild blackboard data: " +
            "\nstoreIsAgressiveIn: (boolBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_AgressionReset : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_storeIsAgressiveInKey = "";

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
        m_director.m_blackboard.SetObject(m_storeIsAgressiveInKey, false);
    }
}
