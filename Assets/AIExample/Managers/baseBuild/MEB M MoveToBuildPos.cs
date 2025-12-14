using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_BBMoveToBuildPos_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_BBMoveToBuildPos_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_BBMoveToBuildPos_UI());
    }

    public UserManger_BBMoveToBuildPos_UI()
    {
        m_name = "UserManger_BBMoveToBuildPos";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_BBMoveToBuildPos";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Moves us towards a buildable location. \n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)\ngetDesiredBuildingTypeFrom: (intBlackboardKeyAsString)\ngetCityBuiltSoFarFrom: (list<InfrstructerObject>BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_BBMoveToBuildPos : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_getDesiredBuildingTypeFromKey = "";
    private string m_getCityBuiltSoFarFromKey = "";
    private string m_storeTargetLocationInKey = "";

    private float m_holdTimeMax = 0.25f;
    private float m_holdTimeCurrent = 0;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getDesiredBuildingTypeFrom")
            {
                m_getDesiredBuildingTypeFromKey = keys[i];
            }

            if (idenifyers[i] == "getCityBuiltSoFarFrom")
            {
                m_getCityBuiltSoFarFromKey = keys[i];
            }

            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
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

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {

    }

    public override void OnEnd()
    {
    }

    public override void OnUpdate(float delta, int index)
    {
        m_holdTimeCurrent += delta;

        if (m_holdTimeCurrent > m_holdTimeMax)
        {
            m_holdTimeCurrent = 0;

            List<UserBlackboard_baseBuild_InfrstructerObject> cityData = (List<UserBlackboard_baseBuild_InfrstructerObject>)m_director.m_blackboard.GetObject(m_getCityBuiltSoFarFromKey);
            int desiredBuildingType = (int)m_director.m_blackboard.GetObject(m_getDesiredBuildingTypeFromKey);

            GameObject nearestBuildPoint = null;
            float distanceToNearestBuildPoint = float.MaxValue;

            if (cityData != null && cityData.Count > 0)
            {
                for (int i = 0; i < cityData.Count; i++)
                {
                    for (int j = 0; j < cityData[i].m_buildPoints.Count; j++)
                    {
                        if (cityData[i].m_buildPoints[j].m_type < 0 || cityData[i].m_buildPoints[j].m_type == desiredBuildingType)
                        {
                            if (cityData[i].m_buildPoints[j].m_gameObject != null && cityData[i].m_buildPoints[j].m_canBuildOn == true)
                            {
                                float currentDistance = (cityData[i].m_buildPoints[j].m_gameObject.transform.position - m_director.m_gameObject.transform.position).magnitude;

                                if (currentDistance < distanceToNearestBuildPoint)
                                {
                                    distanceToNearestBuildPoint = currentDistance;
                                    nearestBuildPoint = cityData[i].m_buildPoints[j].m_gameObject;
                                }
                            }
                        }
                    }
                }
            }

            if (nearestBuildPoint != null)
            {
                m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, nearestBuildPoint.transform.position);
            }
        }
    }

    public int GetIntEvalValue()
    {
        List<UserBlackboard_baseBuild_InfrstructerObject> cityData = (List<UserBlackboard_baseBuild_InfrstructerObject>)m_director.m_blackboard.GetObject(m_getCityBuiltSoFarFromKey);

        if (cityData != null && cityData.Count > 0)
        {
            return 20;
        }

        return 0;
    }
}
