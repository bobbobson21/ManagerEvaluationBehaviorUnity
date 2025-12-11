using MEBS.Runtime;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal class MEB_C_DirectorThreaded_ThreadData
{
    public List<MEB_BaseManager> m_managersToExcute;
    public MEB_DirectorBase m_director;
    public float m_time;
}

public class MEB_C_DirectorThreaded : MonoBehaviour
{
    public MEB_DirectorBase m_directorInterface = new MEB_DirectorBase();

    public MEB_BaseBehaviourData m_behaviorSet;
    public MEB_BaseBlackboard m_blackboard;

    public int m_threadCount = 0;

    List<List<MEB_BaseManager>> m_sortedManagers = new List<List<MEB_BaseManager>>();

    private void Start()
    {
        m_directorInterface.m_blackboard = m_blackboard;
        m_directorInterface.m_gameObject = gameObject;

        for (int i = 0; i < m_behaviorSet.m_items.Count; i++)
        {
            List<MEB_BaseManager> itemsExposed = m_behaviorSet.m_items[i].Export();

            for (int j = 0; j < itemsExposed.Count; j++)
            {
                m_directorInterface.AddManager(itemsExposed[j]);
            }
        }

        for (int i = 0; i < m_threadCount; i++)
        {
            m_sortedManagers.Add(new List<MEB_BaseManager>());
        }

        int scopeIndex = 0;
        int threadIndex = 0;

        for (int i = 0; i < m_directorInterface.m_managers.Count; i++)
        {
            if (m_directorInterface.m_managers[i].m_chainState == MEB_BaseManager_ChainState.ChainStart)
            {
                scopeIndex++;
            }

            if (m_directorInterface.m_managers[i].m_chainState == MEB_BaseManager_ChainState.ChainStart)
            {
                scopeIndex--;
                if (scopeIndex < 0) { scopeIndex = 0; }
            }

            m_sortedManagers[threadIndex].Add(m_directorInterface.m_managers[i]);

            if (threadIndex == 0)
            {
                threadIndex = threadIndex + 1 % m_threadCount;
            }
        }
    }

    private static void ThreadExacute(object data)
    {
        MEB_C_DirectorThreaded_ThreadData input = (MEB_C_DirectorThreaded_ThreadData)data;

        for (int i = 0; i < input.m_managersToExcute.Count; i++)
        {
            input.m_director.Exacute(input.m_managersToExcute[i], input.m_time, i);
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_directorInterface.m_managers.Count; i++)
        {
            m_directorInterface.Evaluate(m_directorInterface.m_managers[i], i);
        }

        for (int i = 0; i < m_threadCount; i++)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadExacute));

            MEB_C_DirectorThreaded_ThreadData threadData = new MEB_C_DirectorThreaded_ThreadData();
            threadData.m_managersToExcute = m_sortedManagers[i];
            threadData.m_director = m_directorInterface;
            threadData.m_time = Time.deltaTime;

            thread.Start(threadData);
        }
    }
}
