using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Manager_BBMineSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_mineSpeed = 2.0f;
    public float m_cooldownSpeed = 0.3f;
    public float m_mineRadius = 2.0f;

    public string m_mineTag = "mineable";

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            m_mineTag = EditorGUILayout.TextField("mine tag", m_mineTag);
            GUILayout.Space(8);

            float.TryParse(EditorGUILayout.TextField("mine radius", m_mineRadius.ToString()), out m_mineRadius);
            float.TryParse(EditorGUILayout.TextField("mine speed", m_mineSpeed.ToString()), out m_mineSpeed);
            float.TryParse(EditorGUILayout.TextField("cooldown speed", m_cooldownSpeed.ToString()), out m_cooldownSpeed);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_BBMine_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_BBMine_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_BBMine_UI());
    }

    public UserManger_BBMine_UI()
    {
        m_name = "UserManger_BBMine";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_BBMineSettings data = new Manager_BBMineSettings();
        data.m_class = "UserManger_BBMine";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Destroys trees near NPC. \n\nvaild blackboard data: \nstoreWoodCountIn: (intBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_BBMine : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_mineTag = "";
    private float m_mineSpeed = 2.0f;
    private float m_cooldownSpeed = 0.3f;
    private float m_mineRadius = 2.0f;

    private float m_currentMineProgress = 0;
    private float m_currentCooldownProgress = 0;

    private string m_storeWoodCountInKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeWoodCountIn")
            {
                m_storeWoodCountInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        Manager_BBMineSettings settings = (Manager_BBMineSettings)m_itemSettings;

        if (settings != null)
        {
            m_mineTag = settings.m_mineTag;
            m_mineSpeed = settings.m_mineSpeed;
            m_cooldownSpeed = settings.m_cooldownSpeed;
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
        if (m_currentCooldownProgress > 0)
        {
            m_currentCooldownProgress -= delta;
            return;
        }

        Collider[]colliders = Physics.OverlapSphere(m_director.m_gameObject.transform.position, m_mineRadius, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == m_mineTag)
            {
                m_currentMineProgress += delta;

                if (m_currentMineProgress >= m_mineSpeed)
                {
                    Object.Destroy(colliders[i].gameObject);

                    m_director.m_blackboard.SetObject(m_storeWoodCountInKey, ((int)m_director.m_blackboard.GetObject(m_storeWoodCountInKey)) + 1);
                    m_currentMineProgress = 0;
                }

                return;
            }
        }

        m_currentMineProgress = 0;
        
    }
}
