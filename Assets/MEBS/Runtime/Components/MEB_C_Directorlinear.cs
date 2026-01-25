using UnityEngine;
using System.Collections.Generic;

using MEBS.Runtime;

public class MEB_C_Directorlinear : MonoBehaviour
{
    public MEB_DirectorBase m_directorInterface = new MEB_DirectorBase();

    public MEB_BaseBehaviourData m_behaviorSet;
    public MEB_BaseBlackboard m_blackboard;

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
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < m_directorInterface.GetManagerCount(); i++)
        {
            m_directorInterface.Evaluate(i, Time.deltaTime);
        }

        for (int i = 0; i < m_directorInterface.GetManagerCount(); i++)
        {
            m_directorInterface.Execute(i, Time.deltaTime);
        }
    }
}
