using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_WantsToExtractEarly_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_WantsToExtractEarly_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_WantsToExtractEarly_UI());
    }

    public UserManger_WantsToExtractEarly_UI()
    {
        m_name = "UserManger_WantsToExtractEarly";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_GoToExtractSettings data = new Manager_GoToExtractSettings();
        data.m_class = "UserManger_WantsToExtractEarly";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the npc want to exit early if the require resource count is met." +
            "\n\nvaild blackboard data: " +
            "\ngetResourceCountFrom :(intBlackboardKeyAsString)" +
            "\ngetDesiredResourceCountFrom :(intBlackboardKeyAsString)";

        data.m_lable = "early extract in";

        return data;
    }
}
#endif

public class UserManger_WantsToExtractEarly : MEB_BaseManager//, MEB_I_IntScoop
{
    private float m_extractIn = 0;

    private string m_getResourceCountFromKey = "";
    private string m_getDesiredResourceCountFromKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getResourceCountFrom")
            {
                m_getResourceCountFromKey = keys[i];
            }

            if (idenifyers[i] == "getDesiredResourceCountFrom")
            {
                m_getDesiredResourceCountFromKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        if (((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) >= ((int)m_director.m_blackboard.GetObject(m_getDesiredResourceCountFromKey)))
        {
            m_extractIn -= delta;
            if (m_extractIn < 0)
            {
                return;
            }
        }

        BlockMoveToExecutionForCycle();
    }

    public override void OnInitialized()
    {
        Manager_GoToExtractSettings settings = (Manager_GoToExtractSettings)m_itemSettings;

        if (settings != null)
        {
            m_extractIn = settings.m_extractIn;
        }
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
        //put update code here
    }
}
