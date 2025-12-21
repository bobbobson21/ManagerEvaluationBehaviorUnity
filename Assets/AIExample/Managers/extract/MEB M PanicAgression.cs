using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Manager_PanicAgressionSettings : MEB_BaseBehaviourData_ItemSettings
{
    public float m_panicIn = 20;
    public float  m_panicIfFraction= 20;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("panic in", m_panicIn.ToString()), out m_panicIn);
            float.TryParse(EditorGUILayout.TextField("panic if %", m_panicIfFraction.ToString()), out m_panicIfFraction);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_PanicAgression_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_PanicAgression_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_PanicAgression_UI());
    }

    public UserManger_PanicAgression_UI()
    {
        m_name = "UserManger_PanicAgression";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_PanicAgressionSettings data = new Manager_PanicAgressionSettings();
        data.m_class = "UserManger_PanicAgression";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the AI agressive if the amout of items needed is to high." +
            "\n\nvaild blackboard data: " +
            "\ngetResourceCountFrom :(intBlackboardKeyAsString)" +
            "\ngetDesiredResourceCountFrom :(intBlackboardKeyAsString)" +
            "\nstoreIsAgressiveIn: (boolBlackboardKeyAsString)";


        return data;
    }
}
#endif

public class UserManger_PanicAgression : MEB_BaseManager//, MEB_I_IntScoop
{
    private string m_getDesiredResourceCountFromKey = "";
    private string m_getResourceCountFromKey = "";
    private string m_storeIsAgressiveInKey = "";

    private float m_panicIn = 20;
    private float  m_panicIfFraction= 20;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "getDesiredResourceCountFrom")
            {
                m_getDesiredResourceCountFromKey = keys[i];
            }

            if (idenifyers[i] == "getResourceCountFrom")
            {
                m_getResourceCountFromKey = keys[i];
            }

            if (idenifyers[i] == "storeIsAgressiveIn")
            {
                m_storeIsAgressiveInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        Manager_PanicAgressionSettings settings = (Manager_PanicAgressionSettings)m_itemSettings;

        if(settings != null)
        {
            m_panicIn = settings.m_panicIn;
            m_panicIfFraction = settings.m_panicIfFraction;     
        }
    }

    /*public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
    }*/

    public override void OnUpdate(float delta, int index)
    {
        float data = (float)((int)m_director.m_blackboard.GetObject(m_getResourceCountFromKey)) / (float)((int)m_director.m_blackboard.GetObject(m_getDesiredResourceCountFromKey));
        m_panicIn -= delta;

        if(m_panicIn <= 0 && data <= m_panicIfFraction)
        {
            m_director.m_blackboard.SetObject(m_storeIsAgressiveInKey, true);
        }
    }
}
