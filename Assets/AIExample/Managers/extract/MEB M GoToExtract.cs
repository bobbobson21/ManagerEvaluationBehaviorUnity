using UnityEditor;
using UnityEngine;

using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;


public class Manager_GoToExtractSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_extractIn = 20;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("extract is in", m_extractIn.ToString()), out m_extractIn);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_GoToExtract_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GoToExtract_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_GoToExtract_UI());
    }

    public UserManger_GoToExtract_UI()
    {
        m_name = "UserManger_GoToExtract";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_GoToExtractSettings data = new Manager_GoToExtractSettings();
        data.m_class = "UserManger_GoToExtract";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the npc move to the exit." +
            "\n\nvaild blackboard data: " +
            "\ngetExtractObjectFrom: (gameObjectBlackboardKeyAsString)" +
            "\nstoreTargetLocationIn: (vector3BlackboardKeyAsString)" +
            "\ngetResourceCountFrom :(intBlackboardKeyAsString)" +
            "\ngetdesiredResourceCountFrom :(intBlackboardKeyAsString)";


        return data;
    }
}
#endif

public class UserManger_GoToExtract : MEB_BaseManager, MEB_I_IntScoop
{
    private float m_extractIn = 0;

    private string m_storeTargetLocationInKey = "";
    private string m_getExtractObjectFromKey = "";
    private string m_getResourceCountFromKey = "";
    private string m_getDesiredResourceCountFromKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }

            if (idenifyers[i] == "getExtractObjectFrom")
            {
                m_getExtractObjectFromKey = keys[i];
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

    public override void EvaluationEnd(int index)
    {
        if (((GameObject)m_director.m_blackboard.GetObject(m_getExtractObjectFromKey)) == null)
        {
            BlockMoveToExecutionForCycle();
        }
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
        GameObject obj = ((GameObject)m_director.m_blackboard.GetObject(m_getExtractObjectFromKey));

        if (obj != null)
        {
            m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, obj.transform.position);
        }
    }

    public int GetIntEvalValue()
    {
        m_extractIn -= Time.deltaTime; //this is cheacky but it dose work if the AI isnt muiltythreaded which it shouldnt because movement dosent support that

        if (m_extractIn < 0 || ((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) >= ((int)m_director.m_blackboard.GetObject(m_getDesiredResourceCountFromKey)))
        {
            return 100;
        }

        return 0;
    }
}
