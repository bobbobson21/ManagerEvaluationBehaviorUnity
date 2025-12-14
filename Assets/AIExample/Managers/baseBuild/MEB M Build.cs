using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_BBBuild_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_BBBuild_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_BBBuild_UI());
    }

    public UserManger_BBBuild_UI()
    {
        m_name = "UserManger_BBBuild";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_BBBuild";
        data.m_displayName = m_name;
        data.m_displayDiscription = "builds out";

        return data;
    }
}
#endif

public class UserManger_BBBuild : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_getDesiredBuildingTypeFromKey = "";
    private string m_getHasGotEnougthToBuildFrom = "";
    private string m_getBuildListFrom = "";
    private string m_getCityBuiltSoFarFrom = "";

    //finish off buildables

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void EvaluationEnd(int index)
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
        //put update code here
    }
}
