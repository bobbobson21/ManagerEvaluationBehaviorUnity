using MEBS.Runtime;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AICTeamOparator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string m_teamId = "";
    public MEB_BaseBlackboard m_blackboard;

    private static Dictionary<string, GameObject> m_teamLeaderData = new Dictionary<string, GameObject>();
    private static Dictionary<string, List<GameObject>> m_teamDataObject = new Dictionary<string, List<GameObject>>();
    private static Dictionary<string, List<MEB_BaseBlackboard>> m_teamDataBlackboard = new Dictionary<string, List<MEB_BaseBlackboard>>();

    public GameObject GetLeaderOfTeam(string team)
    {
        if (m_teamLeaderData.ContainsKey(m_teamId) == true && m_teamLeaderData[team] != null)
        {
            return m_teamLeaderData[team];
        }
        else
        {

            for (int i = 0; i < m_teamDataObject[team].Count; i++)
            {
                if (m_teamDataObject[team][i] != null)
                {
                    m_teamLeaderData[team] = m_teamDataObject[team][i];
                    return m_teamLeaderData[team];
                }
            }
        }

        return null;
    }

    public GameObject GetMyLeader()
    {
        if (m_teamLeaderData.ContainsKey(m_teamId) == true && m_teamLeaderData[m_teamId] != null)
        {
            return m_teamLeaderData[m_teamId];
        }
        else
        {
            for (int i = 0; i < m_teamDataObject[m_teamId].Count; i++)
            {
                if (m_teamDataObject[m_teamId][i] != null)
                {
                    m_teamLeaderData[m_teamId] = m_teamDataObject[m_teamId][i];
                    return m_teamLeaderData[m_teamId];
                }
            }
        }

        return null;
    }

    public void SetMyLeader(GameObject leader)
    {
        m_teamLeaderData[m_teamId] = leader;
    }

    public void SetLeaderOfTeam(string team, GameObject leader)
    {
        m_teamLeaderData[team] = leader;
    }

    public bool IsMyLeader()
    {
        return (GetMyLeader() == gameObject);
    }

    public bool IsOnSameTeam(GameObject ai)
    {
        return m_teamDataObject[m_teamId].Contains(ai);
    }

    public List<GameObject> GetAllOnMyTeam()
    {
        return m_teamDataObject[m_teamId];
    }

    public List<GameObject> GetAllOnTeam(string teamId)
    {
        return m_teamDataObject[teamId];
    }

    public MEB_BaseBlackboard GetBlackboardOfTeamMate(GameObject ai)
    {
        return m_teamDataBlackboard[m_teamId][m_teamDataObject[m_teamId].IndexOf(ai)];
    }

    public MEB_BaseBlackboard GetBlackboardOfTeamMate(int ai)
    {
        return m_teamDataBlackboard[m_teamId][ai];
    }

    public GameObject GetObjectOfTeamMate(int ai)
    {
        return m_teamDataObject[m_teamId][ai];
    }

    public string GetTeamOfObject(GameObject ai)
    {
        foreach (var item in m_teamDataObject)
        {
            if (item.Value.Contains(ai) == true)
            { 
                return item.Key;
            }
        }

        return null;
    }

    void Start()
    {
        if (m_teamDataObject.ContainsKey(m_teamId) == false)
        {
            m_teamDataObject[m_teamId] = new List<GameObject>();
        }

        if (m_teamDataBlackboard.ContainsKey(m_teamId) == false)
        {
            m_teamDataBlackboard[m_teamId] = new List<MEB_BaseBlackboard>();
        }

        m_teamDataObject[m_teamId].Add(gameObject);
        m_teamDataBlackboard[m_teamId].Add(m_blackboard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
