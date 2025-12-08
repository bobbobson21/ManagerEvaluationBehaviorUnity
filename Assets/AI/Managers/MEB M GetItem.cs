using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
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
        int maxHealth = 100;
        int maxAmmo = 25;

        Vector3 destanation = Vector2.zero;

        GameObject objHealth = ((GameObject)m_director.m_blackboard.GetObject(m_getHealthObjectFromKey));
        GameObject objAmmo = ((GameObject)m_director.m_blackboard.GetObject(m_getAmmoObjectFromKey));

        int health = (int)m_director.m_blackboard.GetObject("health");
        int ammo = (int)m_director.m_blackboard.GetObject("ammo");

        float healthDis = ((m_director.m_gameObject.transform.position - objHealth.transform.position).magnitude) * ((health / 1.5f) / 100);
        float ammoDis = ((m_director.m_gameObject.transform.position - objAmmo.transform.position).magnitude) * ((ammo / 1.0f) / maxAmmo);

        if (health < maxHealth && (healthDis < ammoDis || ammoDis == maxAmmo))
        {
            destanation = objHealth.transform.position;
        }
        else
        {
            destanation = objAmmo.transform.position;
        }

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, destanation);
    }

    public int GetIntEvalValue()
    {
        if (((int)m_director.m_blackboard.GetObject("health")) <= 25 || ((int)m_director.m_blackboard.GetObject("ammo")) <= 5)
        {
            return 30;
        }

        return 0;
    }
}
