using System.Collections.Generic;
using UnityEngine;

namespace MEBS.Runtime
{
    public enum MEB_BaseManager_ChainState
    { 
        None, ///dose not rely on the order of managers around it
        ChainStart, ///relys on the managers after it but not the ones before it
        ChainMiddle, ///relys on the order of the managers around it
        ChainEnd, ///rely on the order of managers before it
    }

    public class MEB_BaseManager
    {
        private bool m_runInExecutionStage = true;
        private bool m_lastAssessmentOfActiveness = false;

        protected MEB_DirectorBase m_director;

        public MEB_BaseManager_ChainState m_chainState = MEB_BaseManager_ChainState.None;
        public MEB_BaseBehaviourData_ItemSettings m_itemSettings = null;

        public void InitLoad(MEB_DirectorBase director)
        {
            m_director = director;
            OnInitialized();
        }

        /// <summary>
        /// disables this manager for a cycle; block will last till next evaluration cycle
        /// </summary>
        public void BlockMoveToExecutionForCycle()
        {
            m_runInExecutionStage = false;
        }

        /// <summary>
        /// after a cycle this gets exacuted to reenable the manager, it must always be re-exacuted after or before the start of an evaluation the cycle
        /// </summary>
        public void ResetMoveState()
        {
	        m_runInExecutionStage = true;
        }

        /// <summary>
        /// it will run OnStart, and OnEnd if needed dependeing on if we are allowed to excute and if it was allowed to exacute on the prior cycle
        /// </summary>
        public void AssessMoveResponce()
        {
            if (m_runInExecutionStage == true && m_lastAssessmentOfActiveness == false)
            {
                OnStart();
            }

            if (m_runInExecutionStage == false && m_lastAssessmentOfActiveness == true)
            {
                OnEnd();
            }

            m_lastAssessmentOfActiveness = m_runInExecutionStage;
        }

        /// <summary>
        /// finds out if this manager is allowed to exacute but not nessasayly if its exacuteing
        /// </summary>
        /// <returns>is the manager active</returns>
        public bool IsAllowedToExecute()
        {
            return m_runInExecutionStage;
        }

        /// <summary>
        /// gives the manager a list of blackboard keys it needs to access
        /// </summary>
        /// <param name="idenifyers">tells you what the key is for</param>
        /// <param name="keys">the list of keys the manager needs</param>
        public virtual void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
        {

        }

        /// <summary>
        /// use this function to set if this manager should be active or not and the returned value is set to the evaluration level 
        /// </summary>
        /// <returns>importance of this function</returns>
        /// <param name="index">our index in the directors vector</param>
        public virtual void EvaluationStart(int index)
        {
        }

        /// <summary>
        /// use this in evalurating mannagers stage
        /// </summary>
        /// <param name="index">our index in the directors vector</param>
        public virtual void EvaluationEnd(int index)
        {
        }

        /// <summary>
        /// use this to tell a manager they were the result of an evaluration
        /// </summary>
        public virtual void EvaluationResponce()
        {
        }

        /// <summary>
        /// gets exacuted when the manager is loaded in; only gets exacuted once
        /// </summary>
        public virtual void OnInitialized()
        {
        }

        /// <summary>
        /// gets exacuted if we have not been deativated on this cycle but we were on the prior ones; DONE IN EVALUATION STAGE
        /// </summary>
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// gets exatuted if we get set to inactive but we were active on prior cycles; DONE IN EVALUATION STAGE
        /// </summary>
        public virtual void OnEnd()
        {
        }
    
        /// <summary>
        /// gets exacuted if active; DONE IN EXACUTEION STAGE
        /// </summary>
        /// <param name="delta">delta time between exacuteions</param>
        /// <param name="index">our index in the directors vector</param>
        public virtual void OnUpdate(float delta, int index)
        {
        }
    }
}