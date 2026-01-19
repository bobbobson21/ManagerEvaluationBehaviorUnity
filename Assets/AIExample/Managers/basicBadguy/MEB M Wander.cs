using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;


public class Manager_WanderSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_minRadius = 5;
    public float m_radius = 10;
    public float m_delayBetweenWandering = 1;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            if (MEB_UI_BehaviourEditor.InRestrictedEditMode() == false)
            {
                float.TryParse(EditorGUILayout.TextField("radius", m_radius.ToString()), out m_radius);
                float.TryParse(EditorGUILayout.TextField("min radius", m_minRadius.ToString()), out m_minRadius);
                GUILayout.Space(8);

                float.TryParse(EditorGUILayout.TextField("delay", m_delayBetweenWandering.ToString()), out m_delayBetweenWandering);
            }
            else
            {
                MEB_GUI_Styles.BeginLockedTextStyle();

                EditorGUILayout.TextField("radius", m_radius.ToString());
                EditorGUILayout.TextField("min radius", m_minRadius.ToString());
                GUILayout.Space(8);

                EditorGUILayout.TextField("delay", m_delayBetweenWandering.ToString());

                MEB_GUI_Styles.EndLockedTextStyle();
            }
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
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
        Manager_WanderSettings data = new Manager_WanderSettings();
        data.m_class = "UserManger_Wander";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the NPC wander around the place at will.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_Wander : MEB_BaseManager, MEB_I_IntScoop
{
    private float m_radius = 10;
    private float m_minRadius = 5;
    private float m_delayBetweenWandering = 1;

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

    public override void EvaluationEnd(int index, float delta)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        Manager_WanderSettings settings = (Manager_WanderSettings)m_itemSettings;

        if (settings != null)
        { 
            m_radius = settings.m_radius;
            m_minRadius = settings.m_minRadius;
            m_delayBetweenWandering = settings.m_delayBetweenWandering;
        }

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
        //Debug.Log("wander");

        m_currentTimeLeftTillNextWanderCycle -= delta;

        if (m_currentTimeLeftTillNextWanderCycle < 0)
        {
            m_currentTimeLeftTillNextWanderCycle = m_delayBetweenWandering;

            Vector3 finalpos = m_director.m_gameObject.transform.position;
            bool foundPos = false;
            int maxAttemptsToFindPos = 8;

            for (int i = 0; i < maxAttemptsToFindPos && foundPos == false; i++)
            {
                Vector3 pos = m_director.m_gameObject.transform.position;
                pos += (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized) * Random.Range(m_minRadius, m_radius);

                NavMeshHit hit;
                Vector3 finalPosition = Vector3.zero;
                if (NavMesh.SamplePosition(pos, out hit, m_radius, NavMesh.AllAreas))
                {
                    finalpos = hit.position;
                    foundPos = true;
                }
            }

            m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, finalpos);
        }
    }

    public int GetIntEvalValue(float delta)
    {
        return 1;
    }
}
