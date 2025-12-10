using MEBS.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class AI_C_EyeSensor : MonoBehaviour
{
    [Header("blackboard settings")]
    public MEB_BaseBlackboard m_blackboardToInputDataInto;
    public string m_inputLocation;

    [Header("search settings")]
    public string m_searchingFor = "";
    public Vector3 m_eyePosition = Vector3.zero;

    private GameObject m_nearestObject = null;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == m_searchingFor && collision.gameObject.activeSelf == true)
        {
            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, collision.gameObject.transform.position, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if (hitInfo.collider.gameObject == collision.gameObject)
            {
                float newDist = (collision.gameObject.transform.position - transform.position).magnitude;
                if (m_nearestObject == null || newDist < (m_nearestObject.transform.position - transform.position).magnitude)
                {
                    m_nearestObject = collision.gameObject;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {

    }

    private void Update()
    {
        GameObject returnObject = null;

        if (m_nearestObject != null)
        {
            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, m_nearestObject.transform.position, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            if (hitInfo.collider.gameObject == m_nearestObject)
            { 
                returnObject = m_nearestObject;
            }

            m_blackboardToInputDataInto.SetObject(m_inputLocation, returnObject);
        }
    }

}
