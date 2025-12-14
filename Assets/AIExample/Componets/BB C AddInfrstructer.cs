using System.Collections.Generic;
using UnityEngine;

public class BBCAddInfrstructer : MonoBehaviour
{
    public int m_type = 0;
    public int m_subType = -1;

    public List<GameObject> m_buildPoints = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserBlackboard_baseBuild_InfrstructerObject newObj = new UserBlackboard_baseBuild_InfrstructerObject();

        newObj.m_type = m_type;
        newObj.m_gameObject = gameObject;
        newObj.m_canBuildOn = false;

        for (int i = 0; i < m_buildPoints.Count; i++)
        {
            UserBlackboard_baseBuild_InfrstructerObject buildPoints = new UserBlackboard_baseBuild_InfrstructerObject();
            buildPoints.m_gameObject = m_buildPoints[i];
            buildPoints.m_type = m_subType;
            buildPoints.m_canBuildOn = true;

            newObj.m_buildPoints.Add(buildPoints);
        }

        UserBlackboard_baseBuild.m_cityBuiltSoFar.Add(newObj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
