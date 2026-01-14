using System.Collections.Generic;
using UnityEngine;

public class AICRoomPoints : MonoBehaviour
{
    public static List<AICRoomPoints> m_totalRoomPoints = new List<AICRoomPoints>();
    public float m_clearRadius = 1.0f;
    public float m_reachRadius = 6.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_totalRoomPoints.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0.1f, 0, 1);
        Gizmos.DrawWireSphere(transform.position, m_clearRadius);

        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = new Color(0, 0.5f, 1, 1);
        Gizmos.DrawWireSphere(transform.position, m_reachRadius);
    }
}
