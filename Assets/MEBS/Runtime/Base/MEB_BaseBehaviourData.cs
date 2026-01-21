using System;
using System.Collections.Generic;
using UnityEngine;

namespace MEBS.Runtime
{
    [Serializable]
    public class MEB_BaseBehaviourData_ItemSettings //settings for any given manager + class
    {
        [SerializeField]
        public string m_class = "INVALID";

        [SerializeField]
        public string m_displayName = "INVALID";

        [SerializeField]
        public string m_displayDiscription = "INVALID";

        [SerializeReference]
        public List<string> m_blackboardIdenifyers;

        [SerializeReference]
        public List<string> m_blackboardKeys;

        [SerializeField]
        public bool m_displayBlackboardSettingExpanded = false;

        [SerializeField]
        public bool m_displayCustomSettingExpanded = false;

        [SerializeField]
        public bool m_displayDebugSettingsExpanded = false;

        [NonSerialized]
        public MEB_BaseManager m_runtimeManager;

        public virtual void OnGUI()
        {
            
        }
    }

    [Serializable]
    public class MEB_BaseBehaviourData_Item //for handling evaluration scopes and individual managers
    {
        [SerializeReference]
        public List<MEB_BaseBehaviourData_ItemSettings> m_useInEval;

        [SerializeReference]
        public List<MEB_BaseBehaviourData_ItemSettings> m_evalurators;

        [SerializeReference]
        public MEB_BaseBehaviourData_ItemSettings m_noneEvalurationManager;

        [SerializeField]
        public bool m_isNormalManager = true;

        [SerializeField]
        public bool m_nonColapsed = true;

        public List<MEB_BaseManager> Export()
        {
            List<MEB_BaseManager> returnList = new List<MEB_BaseManager>();

            if (m_isNormalManager == true)
            {
                try
                {
                    Type type = Type.GetType(m_noneEvalurationManager.m_class);
                    MEB_BaseManager instance = (MEB_BaseManager)Activator.CreateInstance(type);
                    instance.SetBlackboardKeys(m_noneEvalurationManager.m_blackboardIdenifyers, m_noneEvalurationManager.m_blackboardKeys);
                    instance.m_chainState = MEB_BaseManager_ChainState.ChainMiddle;
                    instance.m_itemSettings = m_noneEvalurationManager;

                    returnList.Add(instance);
                }
                catch
                {
                    Debug.LogWarning($"MEB_BBD_I_E_TC==F: manager ({m_noneEvalurationManager.m_class}) invalid name is wrong");
                }
            }
            else
            {
                int failureCount = 0;

                for (int i = 0; i < m_useInEval.Count; i++)
                {
                    try
                    {
                        Type type = Type.GetType(m_useInEval[i].m_class);
                        MEB_BaseManager instance = (MEB_BaseManager)Activator.CreateInstance(type);
                        instance.SetBlackboardKeys(m_useInEval[i].m_blackboardIdenifyers, m_useInEval[i].m_blackboardKeys);
                        instance.m_chainState = MEB_BaseManager_ChainState.ChainMiddle;
                        instance.m_itemSettings = m_useInEval[i];

                        returnList.Add(instance);
                    }
                    catch
                    {
                        failureCount++;
                        Debug.LogWarning($"MEB_BBD_I_E_UIETC==F: manager ({i}, {m_useInEval[i].m_class}) invalid name is wrong");
                    }
                }

                for (int i = 0; i < m_evalurators.Count; i++)
                {
                    try
                    {
                        int scopeStart = -((m_useInEval.Count - failureCount) + i);
                        int scopeEnd = -(i);

                        Type type = Type.GetType(m_evalurators[i].m_class);
                        MEB_BaseManager instance = (MEB_BaseManager)Activator.CreateInstance(type);

                        ((MEB_I_EvalScoop)instance).SetEvaluationScope(scopeStart, scopeEnd);

                        instance.SetBlackboardKeys(m_evalurators[i].m_blackboardIdenifyers, m_evalurators[i].m_blackboardKeys);
                        instance.m_chainState = MEB_BaseManager_ChainState.ChainMiddle;
                        instance.m_itemSettings = m_evalurators[i];

                        returnList.Add(instance);
                    }
                    catch
                    {
                        Debug.LogWarning($"MEB_BBD_I_E_ETC==F: manager ({i}, {m_evalurators[i].m_class}) invalid name is wrong");
                    }
                }

                returnList[0].m_chainState = MEB_BaseManager_ChainState.ChainStart;
                returnList[returnList.Count -1].m_chainState = MEB_BaseManager_ChainState.ChainEnd;

                if (returnList.Count == 1)
                {
                    returnList[0].m_chainState = MEB_BaseManager_ChainState.None;
                }

            }

            return returnList;
        }
    }

    [Serializable]
    public class MEB_BaseBehaviourData_ChainScopeItemWapper //for applying chain scopes if there are any
    {
        [SerializeReference]
        public List<MEB_BaseBehaviourData_Item> m_items = new List<MEB_BaseBehaviourData_Item>();

        [SerializeField]
        public bool m_isScoped = false;

        [SerializeField]
        public bool m_isForMainThread = false;

        [SerializeField]
        public bool m_nonColapsed = true;

        public List<MEB_BaseManager> Export()
        {
            List<MEB_BaseManager> returnList = new List<MEB_BaseManager>();

            for (int i = 0; i < m_items.Count; i++)
            {
                List<MEB_BaseManager> data = m_items[i].Export();

                for (int j = 0; j < data.Count; j++)
                {
                    if (m_isScoped == true)
                    {
                        data[j].m_chainState = MEB_BaseManager_ChainState.ChainMiddle;
                    }

                    returnList.Add(data[j]);
                }
            }

            if (m_isScoped == true)
            {
                returnList[0].m_chainState = MEB_BaseManager_ChainState.ChainStart;
                returnList[returnList.Count - 1].m_chainState = MEB_BaseManager_ChainState.ChainEnd;
            }

            return returnList;
        } //exporting MEB_BaseBehaviourData_Item data out here will modify the scope data of all evaluratiors bellow it such that to scop will go and only the chain scope remains. is faster
    }

    [CreateAssetMenu(fileName = "new behavior set", menuName = "MEB/create behavior set")]
    [Serializable]
    public class MEB_BaseBehaviourData : ScriptableObject
    {
        [HideInInspector]
        public List<MEB_BaseBehaviourData_ChainScopeItemWapper> m_items;

        //[NonSerialized]
        [HideInInspector]
        public string m_editorId = ""; 

        [NonSerialized]
        [HideInInspector]
        public string m_runtimeName = "";

        [NonSerialized]
        [HideInInspector]
        public UnityEngine.Object m_runtimeObject = null; //if equal to null all run time debug data is not observed
    }
}
