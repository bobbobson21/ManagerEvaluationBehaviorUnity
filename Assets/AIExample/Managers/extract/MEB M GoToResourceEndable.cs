using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_GoToResourceEndable_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GoToResourceEndable_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_GoToResourceEndable_UI());
    }

    public UserManger_GoToResourceEndable_UI()
    {
        m_name = "UserManger_GoToResourceEndable";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_GoToResourceEndable";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the npc move to a resource" +
            "\n\nvaild blackboard data: " +
            "\ngetResourceObjectFrom: (gameObjectBlackboardKeyAsString)" +
            "\ngetResourceCountFrom :(intBlackboardKeyAsString)" +
            "\ngetDesiredResourceCountFrom :(intBlackboardKeyAsString)" +
            "\nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_GoToResourceEndable : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";
    private string m_getResourceObjectFromKey = "";
    private string m_getResourceCountFromKey = "";
    private string m_getDesiredResourceCountFromKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getResourceObjectFrom")
            {
                m_getResourceObjectFromKey = keys[i];
            }

            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }

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
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        //put on loaded into game code here
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        GameObject obj = (GameObject)m_director.m_blackboard.GetObject(m_getResourceObjectFromKey);

        if (obj != null)
        {
            m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, obj.transform.position);
        }
    }

    public int GetIntEvalValue(float delta)
    {
        if (((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) < ((int)m_director.m_blackboard.GetObject(m_getDesiredResourceCountFromKey)))
        {
            if (((GameObject)m_director.m_blackboard.GetObject(m_getResourceObjectFromKey)) != null)
            {
                return 32;
            }
        }

        return 0;
    }
}
