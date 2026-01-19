using MEBS.Editor;
using MEBS.Runtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MEBS.Editor
{
    [InitializeOnLoad]
    public class MEB_E_EvalBlackboardCompareBlock_UI : MEB_UI_BehaviourEditor_ManagerData
    {
        static MEB_E_EvalBlackboardCompareBlock_UI()
        {
            MEB_UI_BehaviourEditor.AddEvalManager(new MEB_E_EvalBlackboardCompareBlock_UI());
        }

        public MEB_E_EvalBlackboardCompareBlock_UI()
        {
            m_name = "MEB_E_EvalBlackboardCompareBlock";
        }

        public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
        {
            MEB_BaseBehaviourData_BlackboardCompareBlockSettings data = new MEB_BaseBehaviourData_BlackboardCompareBlockSettings();
            data.m_class = "MEBS.Runtime." + m_name;
            data.m_displayName = m_name;
            data.m_displayDiscription = "it takes in two inputs A and B and compares then and if the compare is false all the manager 'managers to evalurate' section are blocked from being exacuted not just the first one. \n\nvalid blackboard data: \nA: (numBlackboardKeyAsString) \nB: (numBlackboardKeyAsString)";

            return data;
        }
    }
}
#endif

namespace MEBS.Runtime
{
    public class MEB_BaseBehaviourData_BlackboardCompareBlockSettings : MEB_BaseBehaviourData_ItemSettings
    {
        public bool m_inverted = false;

        public int m_blockRangeStartPoint = 0;
        public int m_blockRangeEndPoint = int.MaxValue;

        public int m_oparationType = 0;
        public int m_numType = 0;

#if UNITY_EDITOR
        public override void OnGUI()
        {
            int spaceY = 8;
            int height = 19;

            GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
            m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

            if (m_displayCustomSettingExpanded == true)
            {
                if (MEB_UI_BehaviourEditor.InRestrictedEditMode() == false)
                {
                    float oldVal = EditorStyles.popup.fixedHeight;
                    EditorStyles.popup.fixedHeight = height;

                    string[] resultList = new string[3] { "int", "float", "double" };
                    m_numType = EditorGUILayout.Popup("num type", m_numType, resultList, GUILayout.Height(height));

                    resultList = new string[5] { ">", "<", ">=", "<=", "==" };
                    m_oparationType = EditorGUILayout.Popup("oparation type", m_oparationType, resultList, GUILayout.Height(height));

                    EditorStyles.popup.fixedHeight = oldVal;

                    GUILayout.Space(spaceY);

                    m_inverted = EditorGUILayout.Toggle("invert logic", m_inverted);
                    GUILayout.Space(spaceY);

                    int.TryParse(EditorGUILayout.TextField("block range start", m_blockRangeStartPoint.ToString()), out m_blockRangeStartPoint);
                    int.TryParse(EditorGUILayout.TextField("block range end", m_blockRangeEndPoint.ToString()), out m_blockRangeEndPoint);
                }
                else
                {
                    MEB_GUI_Styles.BeginLockedTextStyle();
                    EditorGUILayout.TextField("num type", m_numType.ToString());
                    EditorGUILayout.TextField("oparation type", m_oparationType.ToString());
                    MEB_GUI_Styles.EndLockedTextStyle();

                    EditorGUILayout.Toggle("invert logic", m_inverted);
                    GUILayout.Space(spaceY);

                    MEB_GUI_Styles.BeginLockedTextStyle();
                    EditorGUILayout.TextField("block range start", m_blockRangeStartPoint.ToString());
                    EditorGUILayout.TextField("block range end", m_blockRangeEndPoint.ToString());
                    MEB_GUI_Styles.EndLockedTextStyle();
                }
            }

            GUILayout.EndVertical();
        }
#endif
    }

    public class MEB_E_EvalBlackboardCompareBlock : MEB_BaseManager, MEB_I_EvalScoop
    {
        private bool m_inverted = false;
        private int m_blockRangeStartPoint = 0;
        private int m_blockRangeEndPoint = int.MaxValue;
        private int m_oparationType = 0;
        private int m_numType = 0;

        private int m_startPointOfScope = 0;
        private int m_endPointOfScope = 0;
        private string m_Akey = "";
        private string m_Bkey = "";

        public void SetEvaluationScope(int start, int end)
        {
            m_startPointOfScope = start;
            m_endPointOfScope = end;
        }

        public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
        {
            for (int i = 0; i < idenifyers.Count; i++)
            {
                if (idenifyers[i] == "A")
                {
                    m_Akey = keys[i];
                }

                if (idenifyers[i] == "B")
                {
                    m_Bkey = keys[i];
                }
            }
        }

        public override void OnInitialized()
        {
            MEB_BaseBehaviourData_BlackboardCompareBlockSettings settings = (MEB_BaseBehaviourData_BlackboardCompareBlockSettings)m_itemSettings;

            if (settings != null)
            {
                m_inverted = settings.m_inverted;
                m_blockRangeStartPoint = settings.m_blockRangeStartPoint;
                m_blockRangeEndPoint = settings.m_blockRangeEndPoint;
                m_oparationType = settings.m_oparationType;
                m_numType = settings.m_numType;
            }
        }

        private bool Compare()
        {
            int intA = 0;
            int intB = 0;

            float floatA = 0;
            float floatB = 0;

            double doubleA = 0;
            double doubleB = 0;

            switch (m_oparationType)
            {
                case 0: // >
                    switch (m_numType)
                    {
                        case 0: // int
                            intA = (int)m_director.m_blackboard.GetObject(m_Akey);
                            intB = (int)m_director.m_blackboard.GetObject(m_Bkey);
                            return (intA > intB);


                        case 1: // float
                            floatA = (float)m_director.m_blackboard.GetObject(m_Akey);
                            floatB = (float)m_director.m_blackboard.GetObject(m_Bkey);
                            return (floatA > floatB);

                        case 2: // double
                            doubleA = (double)m_director.m_blackboard.GetObject(m_Akey);
                            doubleB = (double)m_director.m_blackboard.GetObject(m_Bkey);
                            return (doubleA > doubleB);

                        default:
                            break;
                    }
                    break;

                case 1: // <
                    switch (m_numType)
                    {
                        case 0: // int
                            intA = (int)m_director.m_blackboard.GetObject(m_Akey);
                            intB = (int)m_director.m_blackboard.GetObject(m_Bkey);
                            return (intA < intB);


                        case 1: // float
                            floatA = (float)m_director.m_blackboard.GetObject(m_Akey);
                            floatB = (float)m_director.m_blackboard.GetObject(m_Bkey);
                            return (floatA < floatB);

                        case 2: // double
                            doubleA = (double)m_director.m_blackboard.GetObject(m_Akey);
                            doubleB = (double)m_director.m_blackboard.GetObject(m_Bkey);
                            return (doubleA < doubleB);

                        default:
                            break;
                    }
                    break;

                case 2: // >=
                    switch (m_numType)
                    {
                        case 0: // int
                            intA = (int)m_director.m_blackboard.GetObject(m_Akey);
                            intB = (int)m_director.m_blackboard.GetObject(m_Bkey);
                            return (intA >= intB);


                        case 1: // float
                            floatA = (float)m_director.m_blackboard.GetObject(m_Akey);
                            floatB = (float)m_director.m_blackboard.GetObject(m_Bkey);
                            return (floatA >= floatB);

                        case 2: // double
                            doubleA = (double)m_director.m_blackboard.GetObject(m_Akey);
                            doubleB = (double)m_director.m_blackboard.GetObject(m_Bkey);
                            return (doubleA >= doubleB);

                        default:
                            break;
                    }
                    break;

                case 3: // <=
                    switch (m_numType)
                    {
                        case 0: // int
                            intA = (int)m_director.m_blackboard.GetObject(m_Akey);
                            intB = (int)m_director.m_blackboard.GetObject(m_Bkey);
                            return (intA <= intB);


                        case 1: // float
                            floatA = (float)m_director.m_blackboard.GetObject(m_Akey);
                            floatB = (float)m_director.m_blackboard.GetObject(m_Bkey);
                            return (floatA <= floatB);

                        case 2: // double
                            doubleA = (double)m_director.m_blackboard.GetObject(m_Akey);
                            doubleB = (double)m_director.m_blackboard.GetObject(m_Bkey);
                            return (doubleA <= doubleB);

                        default:
                            break;
                    }
                    break;

                default: // ==
                    switch (m_numType)
                    {
                        case 0: // int
                            intA = (int)m_director.m_blackboard.GetObject(m_Akey);
                            intB = (int)m_director.m_blackboard.GetObject(m_Bkey);
                            return (intA == intB);


                        case 1: // float
                            floatA = (float)m_director.m_blackboard.GetObject(m_Akey);
                            floatB = (float)m_director.m_blackboard.GetObject(m_Bkey);
                            return (floatA == floatB);

                        case 2: // double
                            doubleA = (double)m_director.m_blackboard.GetObject(m_Akey);
                            doubleB = (double)m_director.m_blackboard.GetObject(m_Bkey);
                            return (doubleA == doubleB);

                        default:
                            break;
                    }
                    break;
            }

            return false;
        }

        public override void EvaluationStart(int index, float delta)
        {
            int arrayLength = (m_endPointOfScope - m_startPointOfScope);
            bool conditionOfEval = Compare();

            if (m_inverted == true)
            {
                conditionOfEval = !conditionOfEval;
            }

            for (int i = 0; i < arrayLength; i++)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i;
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex);

                if (conditionOfEval == false && i >= m_blockRangeStartPoint && i <= m_blockRangeEndPoint)
                {
                    manager.BlockMoveToExecutionForCycle();
                }
            }

        }

        public override void EvaluationEnd(int index, float delta)
        {
            BlockMoveToExecutionForCycle();
        }
    }
}
