using MEBS.Runtime;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

internal class MEB_C_DirectorThreaded_ThreadData
{
    public List<int> m_managersToExcute;
    public MEB_DirectorBase m_director;
    public float m_time;
}

public class MEB_C_DirectorThreaded : MonoBehaviour
{
    public MEB_DirectorBase m_directorInterface = new MEB_DirectorBase();

    public MEB_BaseBehaviourData m_behaviorSet;
    public MEB_BaseBlackboard m_blackboard;

    [Header("threads")]
    public int m_threadCount = 0;

    private List<int> m_mainThreadManagerList = new List<int>();
    private List<List<int>> m_threadedManagersList = new List<List<int>>();

    [Header("debug")]
    [Tooltip("If true this manager will be able to send debug data back to the UI window. DO NOT USE ON MORE THAN ONE AI USING THE SAME SET.")]
    public bool m_exportManagersWithDebugConnection = false;

    private void Start()
    {
        m_directorInterface.m_blackboard = m_blackboard;
        m_directorInterface.m_gameObject = gameObject;

        if (m_exportManagersWithDebugConnection == true)
        {
            m_behaviorSet.m_runtimeName = $"({gameObject.transform.parent.gameObject.name}) -> ({gameObject.name}) -> (MEB_C_DirectorThreaded)";
        }

        List<int> threadedManagers = new List<int>();

        for (int i = 0; i < m_behaviorSet.m_items.Count; i++)
        {
            List<MEB_BaseManager> itemsExposed = m_behaviorSet.m_items[i].Export(m_exportManagersWithDebugConnection);

            for (int j = 0; j < itemsExposed.Count; j++)//sorts managers based on if its for the main thread or not
            {
                int currentItemId = m_directorInterface.AddManager(itemsExposed[j]);

                if (m_behaviorSet.m_items[i].m_isForMainThread == false)
                {
                    threadedManagers.Add(currentItemId);
                }
                else
                {
                    m_mainThreadManagerList.Add(currentItemId);
                }
            }
        }

        for (int i = 0; i < m_threadCount; i++) //creates room
        {
            m_threadedManagersList.Add(new List<int>());
        }

        int scopeIndex = 0;
        int threadIndex = 0;

        for (int i = 0; i < threadedManagers.Count; i++) //the threaded managers are then sorted into this directors diffrent threads
        {
            if (m_directorInterface.GetManagerByIndex(threadedManagers[i]).m_chainState == MEB_BaseManager_ChainState.ChainStart) //used to find out what level of scope we are currently in
            {
                scopeIndex++;
            }

            if (m_directorInterface.GetManagerByIndex(threadedManagers[i]).m_chainState == MEB_BaseManager_ChainState.ChainEnd)
            {
                scopeIndex--;
                if (scopeIndex < 0) { scopeIndex = 0; }
            }

            m_threadedManagersList[threadIndex].Add(threadedManagers[i]);

            if (scopeIndex == 0) //if we are in no scope/top level scope altanate what thread is used
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
            input.m_director.Exacute(input.m_managersToExcute[i], input.m_time);
        }
    }

    private void Update()
    {
        for (int i = 0; i < m_directorInterface.GetManagerCount(); i++)
        {
            m_directorInterface.Evaluate(i, Time.deltaTime);
        }

        for (int i = 0; i < m_mainThreadManagerList.Count; i++)
        {
           m_directorInterface.Exacute(i, Time.deltaTime);
        }

        for (int i = 0; i < m_threadCount; i++)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadExacute));

            MEB_C_DirectorThreaded_ThreadData threadData = new MEB_C_DirectorThreaded_ThreadData();
            threadData.m_managersToExcute = m_threadedManagersList[i];
            threadData.m_director = m_directorInterface;
            threadData.m_time = Time.deltaTime;

            thread.Start(threadData);
        }
    }
}
