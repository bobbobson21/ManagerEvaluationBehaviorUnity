using MEBS.Runtime;
using System.Collections.Generic;
using UnityEngine;


public class AI_C_EyeSensorList : MonoBehaviour
{
    [Header("blackboard settings")]
    public MEB_BaseBlackboard m_blackboardToInputDataInto;
    public string m_inputLocation;

    [Header("search settings")]
    public string m_searchingFor = "";
    public Vector3 m_eyePosition = Vector3.zero;
    public float m_removeObjectAfterTime = 1f;
    public float m_beginSearchForObjestAfterXTime = 0;
    public AI_C_EyeSensor_ReturnType m_returnType = AI_C_EyeSensor_ReturnType.Nearest;

    private List<AI_C_EyeSensorList_dataPoint> m_totalOblects = new List<AI_C_EyeSensorList_dataPoint>();

    private void OnTriggerEnter(Collider collision)
    {
        if (m_beginSearchForObjestAfterXTime > 0)
        {
            return;
        }

        if (collision.gameObject.tag == m_searchingFor && collision.gameObject.activeSelf == true)
        {
            for (int i = 0; i < m_totalOblects.Count; i++)
            {
                if (collision.gameObject == m_totalOblects[i].m_gameObject)
                {
                    return;
                }
            }

            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, collision.gameObject.transform.position, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if (hitInfo.collider != null && hitInfo.collider.gameObject != null && hitInfo.collider.gameObject == collision.gameObject)
            {
                m_totalOblects.Add(new AI_C_EyeSensorList_dataPoint(collision.gameObject, 2));
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {

    }

    private void Update()
    {
        m_beginSearchForObjestAfterXTime -= Time.deltaTime;
        if (m_beginSearchForObjestAfterXTime > 0)
        {
            return;
        }

        for (int i = 0; i < m_totalOblects.Count; i++)
        {
            if (m_totalOblects[i].m_gameObject == null || m_totalOblects[i].m_gameObject.tag != m_searchingFor)
            {
                m_totalOblects.RemoveAt(i);

                i--;
                if (i < 0) { i = 0; }
                if (i >= m_totalOblects.Count) { return; }
            }

            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, m_totalOblects[i].m_gameObject.transform.position, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if (hitInfo.collider == null || hitInfo.collider.gameObject != m_totalOblects[i].m_gameObject)
            {
                m_totalOblects[i].m_lifeTimeLeft -= Time.deltaTime;

                if (m_totalOblects[i].m_lifeTimeLeft <= 0)
                {
                    m_totalOblects.RemoveAt(i);

                    i--;
                    if (i < 0) { i = 0; }
                    if (i >= m_totalOblects.Count) { return; }
                }
            }
        }

        GameObject returnObject = null;
        float currentDist = 0;
        float testDist = 0;

        switch (m_returnType)
        {
            case AI_C_EyeSensor_ReturnType.Nearest:
                currentDist = float.MaxValue;
                for (int i = 0; i < m_totalOblects.Count; i++)
                {
                    testDist = (gameObject.transform.position - m_totalOblects[i].m_gameObject.transform.position).sqrMagnitude;
                    if (testDist < currentDist)
                    { 
                        currentDist = testDist;
                        returnObject = m_totalOblects[i].m_gameObject;
                    }    
                }

                break;
            case AI_C_EyeSensor_ReturnType.Farest:
                currentDist = float.MinValue;
                for (int i = 0; i < m_totalOblects.Count; i++)
                {
                    testDist = (gameObject.transform.position - m_totalOblects[i].m_gameObject.transform.position).sqrMagnitude;
                    if (testDist > currentDist)
                    {
                        currentDist = testDist;
                        returnObject = m_totalOblects[i].m_gameObject;
                    }
                }

                break;
            case AI_C_EyeSensor_ReturnType.First:
                if (m_totalOblects.Count > 0)
                {
                    returnObject = m_totalOblects[0].m_gameObject;
                }

                break;
            case AI_C_EyeSensor_ReturnType.Last:
                if (m_totalOblects.Count > 0)
                {
                    returnObject = m_totalOblects[m_totalOblects.Count -1].m_gameObject;
                }

                break;
            default:
                break;
        }

        m_blackboardToInputDataInto.SetObject(m_inputLocation, returnObject);
    }

}

public class AI_C_EyeSensorList_dataPoint
{
    public AI_C_EyeSensorList_dataPoint(GameObject obj, float time)
    {
        m_gameObject = obj;
        m_lifeTimeLeft = time;
    }

    public GameObject m_gameObject = null;
    public float m_lifeTimeLeft = 0;
}