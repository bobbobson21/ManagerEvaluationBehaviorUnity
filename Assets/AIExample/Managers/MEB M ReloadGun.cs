using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_ReloadGun_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_ReloadGun_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_ReloadGun_UI());
    }

    public UserManger_ReloadGun_UI()
    {
        m_name = "UserManger_ReloadGun";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_ReloadGun";
        data.m_displayName = m_name;
        data.m_displayDiscription = "reloads the gun";

        return data;
    }
}
#endif

public class UserManger_ReloadGun : MEB_BaseManager, MEB_I_IntScoop
{
    private AICGun m_gunObject = null;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void EvaluationEnd(int index)
    {
        if (m_gunObject.GetAmmoInClip() > 0 || m_gunObject.GetTotalAmmo() <= 0)
        { 
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        m_gunObject = m_director.m_gameObject.GetComponentInChildren<AICGun>();
    }

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
        m_gunObject.Reload();
    }

    public override void OnEnd()
    {
    }

    public override void OnUpdate(float delta, int index)
    {
        //put update code here
    }

    public int GetIntEvalValue()
    {
        return 1;
    }
}
