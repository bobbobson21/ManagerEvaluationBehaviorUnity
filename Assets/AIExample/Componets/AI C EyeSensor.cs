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
    public float m_activationTime = 0.1f;
    public float m_deactivationTime = 120.0f;
    public Vector3 m_eyePosition = Vector3.zero;

    private float m_returnObjectInTime = 0;
    private float m_endReturnOfObjectInTime = 0;

    private bool m_nearestObjectIsVisible = false;
    private GameObject m_nearestObject = null;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == m_searchingFor && collision.gameObject.activeSelf == true)
        {
            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, collision.gameObject.transform.position, out hitInfo);

            if (hitInfo.collider.gameObject == collision.gameObject)
            {
                float newDist = (collision.gameObject.transform.position - transform.position).magnitude;
                if (m_nearestObject == null || newDist < (m_nearestObject.transform.position - transform.position).magnitude)
                {
                    m_nearestObject = collision.gameObject;
                    m_nearestObjectIsVisible = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == m_searchingFor && collision.gameObject == m_nearestObject)
        {
            m_nearestObjectIsVisible = false;
        }
    }

    private void Update()
    {
        GameObject returnObject = null;

        if (m_nearestObject != null)
        {
            RaycastHit hitInfo;
            Physics.Linecast(gameObject.transform.position + m_eyePosition, m_nearestObject.transform.position, out hitInfo);

            m_nearestObjectIsVisible = (hitInfo.collider.gameObject == m_nearestObject);

            if (m_nearestObjectIsVisible == true)
            {
                m_returnObjectInTime += Time.deltaTime;
            }
            else
            {
                m_returnObjectInTime -= Time.deltaTime;

                if (m_returnObjectInTime < 0)
                {
                    m_returnObjectInTime = 0;
                }
            }

            if (m_nearestObjectIsVisible == false)
            {
                m_endReturnOfObjectInTime += Time.deltaTime;
            }
            else
            {
                m_endReturnOfObjectInTime -= Time.deltaTime;

                if (m_endReturnOfObjectInTime < 0)
                {
                    m_endReturnOfObjectInTime = 0;
                }
            }
        }

        if (m_returnObjectInTime >= m_activationTime)
        {
            returnObject = m_nearestObject;
        }

        if (m_endReturnOfObjectInTime >= m_deactivationTime)
        {
            m_nearestObject = null;
        }

        m_blackboardToInputDataInto.SetObject(m_inputLocation, returnObject);
    }

}
