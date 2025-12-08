using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

#if UNITY_EDITOR

public class Manager_GetCloseSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_saftyRadius = 10;
    public float m_minimumVelocity = 0.1f;

    public float m_randMoveRadius = 4.0f;
    
    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("safty radius", m_saftyRadius.ToString()), out m_saftyRadius);
            float.TryParse(EditorGUILayout.TextField("minimum Velocity", m_minimumVelocity.ToString()), out m_minimumVelocity);
            float.TryParse(EditorGUILayout.TextField("rand move radius", m_randMoveRadius.ToString()), out m_randMoveRadius);
        }

        GUILayout.EndVertical();
    }
}

[InitializeOnLoad]
public class UserManger_GetClose_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GetClose_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_GetClose_UI());
    }

    public UserManger_GetClose_UI()
    {
        m_name = "UserManger_GetClose";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_GetCloseSettings data = new Manager_GetCloseSettings();
        data.m_class = "UserManger_GetClose";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Gets us close to the target if there is one. \n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)\ngetAttackObjectFrom: (gameObjectBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_GetClose : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";
    private string m_getAttackObjectFromKey = "";

    private float m_saftyRadius = 10;
    private float m_minimumVelocity = 0.1f;

    private float m_randMoveRadius = 4.0f;

    private Vector3 m_randomOffset = Vector3.zero;
    private NavMeshAgent m_navMeshAgent;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }

            if (idenifyers[i] == "getAttackObjectFrom")
            {
                m_getAttackObjectFromKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        Manager_GetCloseSettings settings = (Manager_GetCloseSettings)m_itemSettings;

        if (settings != null)
        {
            m_saftyRadius = settings.m_saftyRadius;
            m_minimumVelocity = settings.m_minimumVelocity;
            m_randMoveRadius = settings.m_randMoveRadius;
        }

        m_navMeshAgent = m_director.m_gameObject.GetComponent<NavMeshAgent>();
    }

    /*public virtual void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public virtual void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        GameObject obj = ((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey));
        Vector3 destanation = obj.transform.position;

        destanation = destanation + ((m_director.m_gameObject.transform.position - destanation).normalized * m_saftyRadius);

        if ((m_director.m_gameObject.transform.position - destanation).magnitude < m_randMoveRadius)
        {
            if (m_navMeshAgent.velocity.magnitude < m_minimumVelocity)
            {
                m_randomOffset = (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized) * Random.Range(0.0f, m_randMoveRadius);
            }

            destanation = destanation + m_randomOffset;
        }

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, destanation);
    }

    public int GetIntEvalValue()
    {
        if (((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey)) != null)
        {
            return 10;
        }

        return 0;
    }
}

