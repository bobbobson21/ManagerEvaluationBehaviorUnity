using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;


#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_GetItem_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GetItem_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_GetItem_UI());
    }

    public UserManger_GetItem_UI()
    {
        m_name = "UserManger_GetItem";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_GetItem";
        data.m_displayName = m_name;
        data.m_displayDiscription = "moves the npc to the location of an item \n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)\ngetHealthObjectFrom: (gameObjectBlackboardKeyAsString)\ngetAmmoObjectFromKey: (gameObjectBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_GetItem : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";

    private string m_getHealthObjectFromKey = "";
    private string m_getAmmoObjectFromKey = "";

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }

            if (idenifyers[i] == "getHealthObjectFrom")
            {
                m_getHealthObjectFromKey = keys[i];
            }

            if (idenifyers[i] == "getAmmoObjectFrom")
            {
                m_getAmmoObjectFromKey = keys[i];
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
        //Debug.Log("getItem");

        int maxHealth = ((int)m_director.m_blackboard.GetObject("healthMax"));
        int maxAmmo = ((int)m_director.m_blackboard.GetObject("ammoMax"));

        int health = (int)m_director.m_blackboard.GetObject("health");
        int ammo = (int)m_director.m_blackboard.GetObject("ammoTotal");

        Vector3 destanation = Vector2.zero;

        GameObject objHealth = ((GameObject)m_director.m_blackboard.GetObject(m_getHealthObjectFromKey));
        GameObject objAmmo = ((GameObject)m_director.m_blackboard.GetObject(m_getAmmoObjectFromKey));

        float healthDis = float.MaxValue;
        float ammoDis = float.MaxValue;

        if (objHealth != null)
        {
            healthDis = ((m_director.m_gameObject.transform.position - objHealth.transform.position).magnitude) * ((health / 1.5f) / 100);
        }

        if (objAmmo != null)
        {
            ammoDis = ((m_director.m_gameObject.transform.position - objAmmo.transform.position).magnitude) * ((ammo / 1.0f) / maxAmmo);
        }

        if (health < maxHealth && objHealth != null && (healthDis < ammoDis || ammoDis == maxAmmo))
        {
            destanation = objHealth.transform.position;
        }
        else if(objAmmo != null)
        {
            destanation = objAmmo.transform.position;
        }

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, destanation);
    }

    public int GetIntEvalValue(float delta)
    {
        int maxHealth = (int)m_director.m_blackboard.GetObject("healthMax");
        int maxAmmo = (int)m_director.m_blackboard.GetObject("ammoMax");

        GameObject objHealth = ((GameObject)m_director.m_blackboard.GetObject(m_getHealthObjectFromKey));
        GameObject objAmmo = ((GameObject)m_director.m_blackboard.GetObject(m_getAmmoObjectFromKey));


        if ((((int)m_director.m_blackboard.GetObject("health")) < (maxHealth /4) && objHealth != null) || (((int)m_director.m_blackboard.GetObject("ammoTotal")) < (maxAmmo /4) && objAmmo != null))
        {
            return 35;
        }

        if ((((int)m_director.m_blackboard.GetObject("health")) < maxHealth && objHealth != null) || (((int)m_director.m_blackboard.GetObject("ammoTotal")) < maxAmmo && objAmmo != null))
        {
            return 30;
        }

        return 0;
    }
}
