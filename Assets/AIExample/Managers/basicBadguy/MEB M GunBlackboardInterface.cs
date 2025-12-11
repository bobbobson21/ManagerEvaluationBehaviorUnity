using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AI;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_GunBlackboardInterface_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_GunBlackboardInterface_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_GunBlackboardInterface_UI());
    }

    public UserManger_GunBlackboardInterface_UI()
    {
        m_name = "UserManger_GunBlackboardInterface";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_GunBlackboardInterface";
        data.m_displayName = m_name;
        data.m_displayDiscription = "\n\nvaild blackboard data: \nstoreAmmoMaxIn: (intBlackboardKeyAsString)\nstoreAmmoTotalIn: (intBlackboardKeyAsString)\nstoreAmmoClipIn: (intBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_GunBlackboardInterface : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_storeAmmoMaxIn = "";
    private string m_storeAmmoTotalIn = "";
    private string m_storeAmmoClipIn = "";

    private AICGun m_gun = null;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeAmmoMaxIn")
            {
                m_storeAmmoMaxIn = keys[i];
            }

            if (idenifyers[i] == "storeAmmoTotalIn")
            {
                m_storeAmmoTotalIn = keys[i];
            }

            if (idenifyers[i] == "storeAmmoClipIn")
            {
                m_storeAmmoClipIn = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        m_gun = m_director.m_gameObject.GetComponentInChildren<AICGun>();

        m_director.m_blackboard.SetObject(m_storeAmmoClipIn, m_gun.m_totalAmmoCountMax);
    }

    public override void OnUpdate(float delta, int index)
    {
        m_director.m_blackboard.SetObject(m_storeAmmoClipIn, m_gun.GetAmmoInClip());
        m_director.m_blackboard.SetObject(m_storeAmmoTotalIn, m_gun.GetTotalAmmo());
    }
}
