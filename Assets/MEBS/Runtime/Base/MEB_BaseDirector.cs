using UnityEngine;
using System.Collections.Generic;

namespace MEBS.Runtime
{
    public class MEB_DirectorBase
    {
        private List<MEB_BaseManager> m_managers = new List<MEB_BaseManager>();
        public MEB_BaseBlackboard m_blackboard = null;
        public GameObject m_gameObject = null;

        public bool m_canEval = true;
        public bool m_canExec = true;

        public void Evaluate(int index, float delta)
        {
            if (m_canEval == false) { return; }

            m_managers[index].ResetMoveState();

            m_managers[index].EvaluationStart(index, delta);
            m_managers[index].EvaluationEnd(index, delta);
        }

        public void Execute(int index, float delta)
        {
            if (m_canExec == false) { return; }

            m_managers[index].AssessMoveResponce();

            if (m_managers[index].IsAllowedToExecute() == true)
            {
                m_managers[index].OnUpdate(delta, index);
            }
        }
        
        public bool IsManagerUnderDirector(MEB_BaseManager manager)
        {
            return m_managers.Contains(manager);
        }

        public int AddManager(MEB_BaseManager manager)
        {
            manager.InitLoad(this);
            m_managers.Add(manager);

            return m_managers.Count -1;
        }

        /*public void RemoveManager(int index) //dont remove managers post runtime (unless you are in non garbage collection coding lang) it can be bad but the code is here incase you really really want to but you have been warned
        {
            m_managers.RemoveAt(index);
        }

        public void RemoveAllManagers()
        { 
            m_managers.Clear();
        }*/

        public MEB_BaseManager GetManagerByIndex(int index)
        {
            if (index < 0 || index >= m_managers.Count) { return null; }
            return m_managers[index];
        }

        public int GetIndexOfManager(MEB_BaseManager manager)
        {
            for (int i = 0; i < m_managers.Count; i++)
            {
                if (m_managers[i] == manager)
                {
                    return i;
                }
            }

            return int.MinValue;
        }

        public int GetManagerCount()
        { 
            return m_managers.Count;
        }

        public virtual void ForceRedoEval()
        { 
        
        }
    }
}
