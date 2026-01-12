using MEBS.Runtime;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AICTeamOparator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string m_teamId = "";
    public MEB_BaseBlackboard m_blackboard;

    private static Dictionary<string,List<GameObject>> m_teamDataObject = new Dictionary<string,List<GameObject>>();
    private static Dictionary<string, List<MEB_BaseBlackboard>> m_teamDataBlackboard = new Dictionary<string, List<MEB_BaseBlackboard>>();


    public GameObject GetLeader()
    {
        for (int i = 0; i < m_teamDataObject[m_teamId].Count; i++)
        {
            if (m_teamDataObject[m_teamId][i] != null)
            {
                return m_teamDataObject[m_teamId][i];
            }
        }

        return null;
    }

    public bool IsLeader()
    {
        return (GetLeader() == gameObject);
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
