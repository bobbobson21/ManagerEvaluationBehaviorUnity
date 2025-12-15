using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Manager_FireGunSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_shootScope = 0.9f;
    public float m_shootDistance = 4;

    public float m_speed = 10;
    public float m_speedMedium = 12;
    public float m_speedFast = 14;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("shoot if in scope of", m_shootScope.ToString()), out m_shootScope);
            float.TryParse(EditorGUILayout.TextField("shoot if in scope of", m_shootDistance.ToString()), out m_shootDistance);
            GUILayout.Space(8);
            float.TryParse(EditorGUILayout.TextField("gun movement speed", m_speed.ToString()), out m_speed);
            float.TryParse(EditorGUILayout.TextField("speed at 50% health", m_speedMedium.ToString()), out m_speedMedium);
            float.TryParse(EditorGUILayout.TextField("speed at 25% health", m_speedFast.ToString()), out m_speedFast);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_FireGun_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_FireGun_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_FireGun_UI());
    }

    public UserManger_FireGun_UI()
    {
        m_name = "UserManger_FireGun";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_FireGunSettings data = new Manager_FireGunSettings();
        data.m_class = "UserManger_FireGun";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Fires a gun towards the attacker. \n\nvaild blackboard data: \ngetAttackObjectFrom: (gameObjectBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_FireGun : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_getAttackObjectFromKey = "";
    private AICGun m_gunObject = null;

    private float m_shootScope = 0.9f;
    private float m_shootDistance = 4.0f;

    private float m_speed = 10;
    private float m_speedMedium = 12;
    private float m_speedFast = 14;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getAttackObjectFrom")
            {
                m_getAttackObjectFromKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        if (m_gunObject == null || m_gunObject.GetAmmoInClip() <= 0)
        { 
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        m_gunObject = m_director.m_gameObject.GetComponentInChildren<AICGun>();

        Manager_FireGunSettings settings = (Manager_FireGunSettings)m_itemSettings;

        if (settings != null)
        {
            m_shootScope = settings.m_shootScope;
            m_shootDistance = settings.m_shootDistance;

            m_speed = settings.m_speed;
            m_speedMedium = settings.m_speedMedium;
            m_speedFast = settings.m_speedFast;
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
        GameObject obj = ((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey));

        if (obj == null)
        {
            return;
        }

        int health = ((int)m_director.m_blackboard.GetObject("health"));
        int maxHealth = ((int)m_director.m_blackboard.GetObject("healthMax"));

        float speed = m_speed;

        if (health <= maxHealth / 2)
        { 
            speed = m_speedMedium;
        }

        if (health <= maxHealth / 4)
        { 
            speed = m_speedFast;
        }

        m_gunObject.RotateGun(obj.transform.position, speed * delta);

        if (m_gunObject.CanFire() == true && (m_gunObject.gameObject.transform.position  - obj.transform.position).magnitude < m_shootDistance)
        {
           
            //Debug.Log(Vector3.Dot((m_gunObject.gameObject.transform.position - m_director.m_gameObject.transform.position).normalized, (obj.transform.position - m_director.m_gameObject.transform.position).normalized));

            float dotScope = Vector3.Dot((m_gunObject.gameObject.transform.position - m_director.m_gameObject.transform.position).normalized, (obj.transform.position - m_director.m_gameObject.transform.position).normalized);

            if (dotScope >= m_shootScope)
            {
                m_gunObject.FireGun();
            }
        }
    }

    public int GetIntEvalValue()
    {
        GameObject obj = ((GameObject)m_director.m_blackboard.GetObject(m_getAttackObjectFromKey));

        if (obj != null)
        {
            return 20;
        }

        return 0;
    }
}
