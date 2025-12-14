using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_BBPlanBuilding_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_BBPlanBuilding_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_BBPlanBuilding_UI());
    }

    public UserManger_BBPlanBuilding_UI()
    {
        m_name = "UserManger_BBPlanBuilding";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_BBPlanBuilding";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Figures out what building is needed and how much is needed to build it." +
            "\n\nvaild blackboard data: " +
            "\ngetCityBuiltSoFarFrom: (list<InfrstructerObject>BlackboardKeyAsString) " +
            "\ngetResourceCountFrom: (intBlackboardKeyAsString) " +
            "\nstoreDesiredBuildingTypeIn: (intBlackboardKeyAsString)" +
            "\nstoreHasGotEnougthToBuildIn: (boolBlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_BBPlanBuilding : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_storeDesiredBuildingTypeInKey = "";
    private string m_storeHasGotEnougthToBuildInKey = "";
    private string m_getCityBuiltSoFarFromKey = "";
    private string m_getResourceCountFromKey = "";

    private int CalulateNeededBuildingType(List<UserBlackboard_baseBuild_InfrstructerObject> cityData)
    {
        int contentBuildCount_Houses = 0;
        int contentBuildCount_Med = 0;
        int contentBuildCount_Gas = 0;
        int contentBuildCount_Water = 0;
        int contentBuildCount_Elect = 0;

        

        for (int i = 0; i < cityData.Count; i++)
        {
            int roadSpacingCount = 0;

            switch (cityData[i].m_type)
            {
                case 0:
                    for (int j = 0; j < cityData[i].m_buildPoints.Count; j++)
                    {
                        if (cityData[i].m_buildPoints[j].m_gameObject != null && cityData[i].m_buildPoints[j].m_canBuildOn == true)
                        {
                            roadSpacingCount++;
                        }
                    }

                    if(roadSpacingCount > cityData[i].m_buildPoints.Count -2)
                    {
                        return 0; //road type
                    }
                    break;

                case 1:
                    contentBuildCount_Houses++;
                    break;

                case 2:
                    contentBuildCount_Med++;
                    break;

                case 3:
                    contentBuildCount_Gas++;
                    break;

                case 4:
                    contentBuildCount_Water++;
                    break;

                case 5:
                    contentBuildCount_Elect++;
                    break;
            }
        }

        if ((contentBuildCount_Houses + contentBuildCount_Med + contentBuildCount_Water +contentBuildCount_Gas) % 80 == 0 || contentBuildCount_Elect == 0)
        {
            return 5; //build eletrical genarators
        }

        if (contentBuildCount_Houses % 20 == 0 || contentBuildCount_Med == 0)
        {
            return 2; //build medical center
        }

        if ((contentBuildCount_Houses + contentBuildCount_Med) % 40 == 0 || contentBuildCount_Water == 0)
        {
            return 4; //build water storage
        }

        if ((contentBuildCount_Houses + contentBuildCount_Med + contentBuildCount_Water) % 60 == 0 || contentBuildCount_Gas == 0)
        {
            return 3; //build gas storage
        }

        return 1;
    }

    private int CalulateHowMuchResourceIsNeeded(int buildingType)
    {
        int resourcesNeededToBuild_Road = 5;
        int resourcesNeededToBuild_House = 5;
        int resourcesNeededToBuild_Med = 10;
        int resourcesNeededToBuild_Gas = 12;
        int resourcesNeededToBuild_Water = 12;
        int resourcesNeededToBuild_Elect = 8;

        switch (buildingType)
        {
            case 0: //road
                return resourcesNeededToBuild_Road;

            case 1: //house
                return resourcesNeededToBuild_House;

            case 2: //med
                return resourcesNeededToBuild_Med;

            case 3: //gas
                return resourcesNeededToBuild_Gas;

            case 4: //water
                return resourcesNeededToBuild_Water;

            case 5: //eletric
                return resourcesNeededToBuild_Elect;

            default:
                return -1;
        }
    }

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getCityBuiltSoFarFrom")
            {
                m_getCityBuiltSoFarFromKey = keys[i];
            }

            if (idenifyers[i] == "getResourceCountFrom")
            {
                m_getResourceCountFromKey = keys[i];
            }

            if (idenifyers[i] == "storeDesiredBuildingTypeIn")
            {
                m_storeDesiredBuildingTypeInKey = keys[i];
            }

            if (idenifyers[i] == "storeHasGotEnougthToBuildIn")
            {
                m_storeHasGotEnougthToBuildInKey = keys[i];
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

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        List<UserBlackboard_baseBuild_InfrstructerObject> cityData = (List<UserBlackboard_baseBuild_InfrstructerObject>)m_director.m_blackboard.GetObject(m_getCityBuiltSoFarFromKey);

        int buildType = CalulateNeededBuildingType(cityData);
        int howMuchIsNeeded = CalulateHowMuchResourceIsNeeded(buildType);

        m_director.m_blackboard.SetObject(m_storeDesiredBuildingTypeInKey, buildType);
        m_director.m_blackboard.SetObject(m_storeHasGotEnougthToBuildInKey, (((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) >= howMuchIsNeeded));
    }
}
