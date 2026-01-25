using MEBS.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class MEB_C_DirectorLodInterface : MEB_DirectorBase
{
    private MEB_C_DirectorLod m_parent = null;

    public MEB_C_DirectorLodInterface(MEB_C_DirectorLod parent)
    { 
        m_parent = parent;
    }

    public override void ForceRedoEval()
    {
        m_parent.ResetEval();
    }
}

public class MEB_C_DirectorLod : MonoBehaviour
{
    public MEB_C_DirectorLodInterface m_directorInterface = null;

    public MEB_BaseBehaviourData m_behaviorSet;
    public MEB_BaseBlackboard m_blackboard;

    [Header("lod")]
    [Tooltip("The object whos distance to us determins the level of detail/ lod.")]
    public GameObject m_player = null;

    public float m_ThinkDelayTimesDistance = 0.1f;
    public float m_maxThinkDelay = 1.0f;

    private float m_currentDelay = 0;

    public void ResetEval()
    { 
        m_currentDelay = 0;
    }

    private void Start()
    {
        m_directorInterface = new MEB_C_DirectorLodInterface(this);

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

    private void Update()
    {
        m_currentDelay -= Time.deltaTime;

        if (m_currentDelay <= 0)
        {
            m_currentDelay = (m_player.transform.position - transform.position).magnitude *m_currentDelay;
            if (m_currentDelay > m_maxThinkDelay)
            {
                m_currentDelay = m_maxThinkDelay;
            }

            for (int i = 0; i < m_directorInterface.GetManagerCount(); i++)
            {
                m_directorInterface.Evaluate(i, m_currentDelay + Time.deltaTime);
            }
        }

        for (int i = 0; i < m_directorInterface.GetManagerCount(); i++)
        {
            m_directorInterface.Execute(i, Time.deltaTime);
        }
    }
}
