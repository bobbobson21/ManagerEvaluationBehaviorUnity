using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

#if UNITY_EDITOR
public class WanderSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_radius = 10;
    public float m_delayBetweenWandering = 1;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("radius", m_radius.ToString()), out m_radius);
            float.TryParse(EditorGUILayout.TextField("delay", m_delayBetweenWandering.ToString()), out m_delayBetweenWandering);
        }

        GUILayout.EndVertical();
    }
}

[InitializeOnLoad]
public class UserManger_Wander_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_Wander_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_Wander_UI());
    }

    public UserManger_Wander_UI()
    {
        m_name = "UserManger_Wander";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        WanderSettings data = new WanderSettings();
        data.m_class = "UserManger_Wander";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the NPC wander around the place at will.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_Wander : MEB_BaseManager, MEB_I_IntScoop
{
    public float m_radius = 10;
    public float m_delayBetweenWandering = 1;

    private string m_storeTargetLocationInKey = "";

    private float m_currentTimeLeftTillNextWanderCycle = 0;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        WanderSettings settings = (WanderSettings)m_itemSettings;

        if (settings != null)
        { 
            m_radius = settings.m_radius;
            m_delayBetweenWandering = settings.m_delayBetweenWandering;
        }

        //put on loaded into game code here
    }

    /*public virtual void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public virtual void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        m_currentTimeLeftTillNextWanderCycle -= delta;

        if (m_currentTimeLeftTillNextWanderCycle < 0)
        {
            m_currentTimeLeftTillNextWanderCycle = m_delayBetweenWandering;

            Vector3 pos = m_director.m_gameObject.transform.position;
            pos += (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized) * Random.Range(0.0f, m_radius);

            m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, pos);
        }
    }

    public int GetIntEvalValue()
    {
        return 1;
    }
}
