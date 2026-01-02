using System.Collections.Generic;
using UnityEngine;

public class AICAvoidanceAreas : MonoBehaviour
{
    public static List<AICAvoidanceAreas> m_totalAvoidanceAreas = new List<AICAvoidanceAreas>();
    public Vector3 m_avoidArea = Vector3.one;
    public Vector3 m_avoidPoint = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_totalAvoidanceAreas.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0, 0, 1.0f);
        Gizmos.DrawWireCube(transform.position, m_avoidArea);

        Gizmos.DrawSphere(transform.position + m_avoidPoint, 1);


    }
}
