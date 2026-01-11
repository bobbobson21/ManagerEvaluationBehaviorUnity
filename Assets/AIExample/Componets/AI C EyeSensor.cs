using MEBS.Runtime;
using System.Collections.Generic;
using UnityEngine;


public enum AI_C_EyeSensor_ReturnType
{
    Nearest,
    Farest,
    First,
    Last,
}

public class AI_C_EyeSensor : MonoBehaviour
{
    [Header("blackboard settings")]
    public MEB_BaseBlackboard m_blackboardToInputDataInto;
    public string m_inputLocation;

    [Header("search settings")]
    public string m_searchingFor = "";
    public Vector3 m_eyePosition = Vector3.zero;
    public bool m_removeFromBlackboardIfNotVisible = false;
    public float m_removeObjectAfterTime = 1f;
    public float m_beginSearchForObjestAfterXTime = 0;
    public AI_C_EyeSensor_ReturnType m_returnType = AI_C_EyeSensor_ReturnType.Nearest;

    [Header("team")]
    public AICTeamOparator m_teamOparator = null;
    public bool m_doTeamBlackbaordCheck = true;

    private float m_visiblityTime = 0f;
    private GameObject m_returnObject = null;


    private void OnTriggerEnter(Collider collision)
    {
        if (m_beginSearchForObjestAfterXTime > 0)
        {
            return;
        }

        if (collision.gameObject.tag == m_searchingFor && collision.gameObject.activeSelf == true)
        {
            if (m_teamOparator != null && m_teamOparator.IsOnSameTeam(collision.gameObject) == true)
            {
                return;
            }

            if (m_teamOparator != null && m_doTeamBlackbaordCheck == true)
            {
                for (int i = 0; i < m_teamOparator.GetAllOnMyTeam().Count; i++)
                {
                    if (m_teamOparator.GetBlackboardOfTeamMate(i) != null && ((GameObject)m_teamOparator.GetBlackboardOfTeamMate(i).GetObject(m_inputLocation)) == collision.gameObject)
                    { 
                        return;
                    }
                }
            }

            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, collision.gameObject.transform.position, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            float newDist = 0;

            if (hitInfo.collider != null && hitInfo.collider.gameObject != null && hitInfo.collider.gameObject == collision.gameObject)
            {
                switch (m_returnType)
                {
                    case AI_C_EyeSensor_ReturnType.Nearest:
                        newDist = (collision.gameObject.transform.position - transform.position).sqrMagnitude;
                        if (m_returnObject == null || newDist < (m_returnObject.transform.position - transform.position).sqrMagnitude)
                        {
                            m_returnObject = collision.gameObject;
                            m_visiblityTime = m_removeObjectAfterTime;
                        }

                        break;
                    case AI_C_EyeSensor_ReturnType.Farest:
                        newDist = (collision.gameObject.transform.position - transform.position).sqrMagnitude;
                        if (m_returnObject == null || newDist > (m_returnObject.transform.position - transform.position).sqrMagnitude)
                        {
                            m_returnObject = collision.gameObject;
                            m_visiblityTime = m_removeObjectAfterTime;
                        }

                        break;
                    case AI_C_EyeSensor_ReturnType.First:
                        if (m_returnObject == null)
                        {
                            m_returnObject = collision.gameObject;
                        }

                        break;
                    case AI_C_EyeSensor_ReturnType.Last:
                        m_returnObject = collision.gameObject;

                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {

    }

    private void Start()
    {
    }

    private void Update()
    {
        m_beginSearchForObjestAfterXTime -= Time.deltaTime;
        if (m_beginSearchForObjestAfterXTime > 0)
        {
            return;
        }

        GameObject returnObject = null;

        if(m_returnObject != null && m_returnObject.tag != m_searchingFor)
        {
            m_returnObject = null;
        }

        if (m_returnObject != null)
        {
            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, m_returnObject.transform.position, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if ((m_removeFromBlackboardIfNotVisible == false) || (hitInfo.collider != null && hitInfo.collider.gameObject == m_returnObject))
            { 
                returnObject = m_returnObject;
            }

            if (hitInfo.collider == null || hitInfo.collider.gameObject != m_returnObject)
            {
                m_visiblityTime -= Time.deltaTime;

                if (m_visiblityTime <= 0)
                {
                    m_returnObject = null;
                }
            }
        }

        m_blackboardToInputDataInto.SetObject(m_inputLocation, returnObject);
    }

}
