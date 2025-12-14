using UnityEngine;

public class BBCRemoveBuildPointField : MonoBehaviour
{
    private int m_currentPoint = 0;
    Collider m_collider;

    public void Start()
    {
        m_collider = GetComponent<Collider>();
    }

    public void Update()
    {
        while (m_currentPoint <= UserBlackboard_baseBuild.m_cityBuiltSoFar.Count)
        {
            for (int i = 0; i < UserBlackboard_baseBuild.m_cityBuiltSoFar[m_currentPoint].m_buildPoints.Count; i++)
            {
                GameObject obj = UserBlackboard_baseBuild.m_cityBuiltSoFar[m_currentPoint].m_buildPoints[i].m_gameObject;

                if (obj != null && obj != gameObject && m_collider.bounds.Contains(obj.transform.position) == true)
                {
                    Destroy(obj);
                }
            }


            m_currentPoint++;
        }
    }
}
