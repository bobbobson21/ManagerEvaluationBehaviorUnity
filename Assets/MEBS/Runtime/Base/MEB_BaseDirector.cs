using UnityEngine;
using System.Collections.Generic;

namespace MEBS.Runtime
{
    public class MEB_DirectorBase
    {
        public List<MEB_BaseManager> m_managers = new List<MEB_BaseManager>();
        public MEB_BaseBlackboard m_blackboard = null;
        public GameObject m_gameObject = null;

        public void Evaluate(MEB_BaseManager manager, int index)
        {
            manager.ResetMoveState();

            manager.EvaluationStart(index);
            manager.EvaluationEnd(index);
        }

        public void Exacute(MEB_BaseManager manager, float delta, int index)
        {
            manager.AssessMoveResponce();

            if (manager.IsAllowedToExecute() == true)
            {
                manager.OnUpdate(delta, index);
            }
        }

        public void AddManager(MEB_BaseManager manager)
        {
            manager.InitLoad(this);
            m_managers.Add(manager);
        }

        public void RemoveManager(MEB_BaseManager manager)
        {
            m_managers.Remove(manager);
        }
        public void RemoveAllManagers()
        { 
            m_managers.Clear();
        }

        public bool IsManagerUnderDirector(MEB_BaseManager manager)
        { 
            return m_managers.Contains(manager);
        }
        public MEB_BaseManager GetManagerByIndex(int index)
        { 
            return m_managers[index];
        }

        public virtual void ForceRedoEval()
        { 
        
        }
    }
}
