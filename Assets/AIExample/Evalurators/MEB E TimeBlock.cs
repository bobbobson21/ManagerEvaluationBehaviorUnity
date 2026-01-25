using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserManger_Eval_TimeBlockSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_blockInStart = 0;
    public float m_blockInEnd = float.MaxValue;

    public int m_blockRangeStartPoint = 0;
    public int m_blockRangeEndPoint = int.MaxValue;

    public override void OnGUI()
    {
        int spaceY = 8;

        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("block in start", m_blockInStart.ToString()), out m_blockInStart);
            float.TryParse(EditorGUILayout.TextField("block in end", m_blockInEnd.ToString()), out m_blockInEnd);
            GUILayout.Space(spaceY);

            int.TryParse(EditorGUILayout.TextField("block range start", m_blockRangeStartPoint.ToString()), out m_blockRangeStartPoint);
            int.TryParse(EditorGUILayout.TextField("block range end", m_blockRangeEndPoint.ToString()), out m_blockRangeEndPoint);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_Eval_TimeBlock_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_Eval_TimeBlock_UI()
    {
        MEB_UI_BehaviourEditor.AddEvalManager(new UserManger_Eval_TimeBlock_UI());
    }

    public UserManger_Eval_TimeBlock_UI()
    {
        m_name = "UserManger_Eval_TimeBlock";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        UserManger_Eval_TimeBlockSettings data = new UserManger_Eval_TimeBlockSettings();
        data.m_class = "UserManger_Eval_TimeBlock";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Blocks manager in evaluration from Executeing after X amount of time.";

        return data;
    }
}
#endif

public class UserManger_Eval_TimeBlock : MEB_BaseManager, MEB_I_EvalScoop
{
    private float m_blockInStart = 0;
    private float m_blockInEnd = float.MaxValue;
    private int m_blockRangeStartPoint = 0;
    private int m_blockRangeEndPoint = int.MaxValue;

    private int m_startPointOfScope = 0;
    private int m_endPointOfScope = 0;

    public void SetEvaluationScope(int start, int end)
    {
        m_startPointOfScope = start;
        m_endPointOfScope = end;
    }

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {

    }

    public override void OnInitialized()
    {
        UserManger_Eval_TimeBlockSettings settings = (UserManger_Eval_TimeBlockSettings)m_itemSettings;

        if (settings != null)
        {
            m_blockInStart = settings.m_blockInStart;
            m_blockInEnd = settings.m_blockInEnd;
            m_blockRangeStartPoint = settings.m_blockRangeStartPoint;
            m_blockRangeEndPoint = settings.m_blockRangeEndPoint;
        }
    }

    public override void EvaluationStart(int index, float delta)
    {
        m_blockInStart -= delta;
        m_blockInEnd -= delta;

        if (m_blockInStart > 0) //if less than 0 then the timeblock will be active
        {
            return;
        }

        if (m_blockInEnd < 0 && m_blockInEnd > m_blockInStart) //if the end point time is less then 0 and its higher than start point block will be made inactive
        {
            return;
        }

        //Debug.Log("running time block");

        int arrayLength = (m_endPointOfScope - m_startPointOfScope);
        
        for (int i = 0; i < arrayLength; i++)
        {
            if (i >= m_blockRangeStartPoint && i <= m_blockRangeEndPoint)
            {
                int otherManagerIndex = ((index + m_endPointOfScope) - arrayLength) + i; //calulates the managers true index
                MEB_BaseManager manager = m_director.GetManagerByIndex(otherManagerIndex); //gets the manager

                manager.BlockMoveToExecutionForCycle();
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        BlockMoveToExecutionForCycle();
    }
}
