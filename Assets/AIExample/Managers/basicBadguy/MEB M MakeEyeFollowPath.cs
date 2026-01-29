using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class Manager_MakeEyeFollowPathSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_speed = 10;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            if (MEB_UI_BehaviourEditor.InRestrictedEditMode() == false)
            {
                float.TryParse(EditorGUILayout.TextField("gun movement speed", m_speed.ToString()), out m_speed);
            }
            else
            {
                MEB_GUI_Layout.LockedInputStyle("gun movement speed", m_speed.ToString());
            }
        }

        GUILayout.EndVertical();
    }
}


#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_MakeEyeFollowPath_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_MakeEyeFollowPath_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_MakeEyeFollowPath_UI());
    }

    public UserManger_MakeEyeFollowPath_UI()
    {
        m_name = "UserManger_MakeEyeFollowPath";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_MakeEyeFollowPathSettings data = new Manager_MakeEyeFollowPathSettings();
        data.m_class = "UserManger_MakeEyeFollowPath";
        data.m_displayName = m_name;
        data.m_displayDiscription = "makes the eye follow the path currently being walked";

        return data;
    }
}
#endif

public class UserManger_MakeEyeFollowPath : MEB_BaseManager, MEB_I_IntScoop
{
    private float m_speed = 10;

    private AICGun m_gunObject = null;
    private NavMeshAgent m_agent = null;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void EvaluationEnd(int index, float delta)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        m_gunObject = m_director.m_gameObject.GetComponentInChildren<AICGun>();
        m_agent = m_director.m_gameObject.GetComponent<NavMeshAgent>();

        Manager_MakeEyeFollowPathSettings settings = (Manager_MakeEyeFollowPathSettings)m_itemSettings;

        if (settings != null)
        {
            m_speed = settings.m_speed;
        }
    }

    public override void OnUpdate(float delta, int index)
    {
        if (m_agent.velocity != Vector3.zero)
        {
            m_gunObject.RotateGun(m_director.m_gameObject.transform.position + m_agent.velocity, m_speed * delta);
        }
    }

    public int GetIntEvalValue(float delta)
    {
        return 1;
    }
}
