using MEBS.Runtime;
using UnityEngine;

public class AI_C_EyeSensor :MonoBehaviour
{
    [Header("blackboard settings")]
    public MEB_BaseBlackboard m_blackboardToInputDataInto;
    public string m_inputLocation;

    [Header("search settings")]
    public string m_searchingFor = "";
    public float m_activationTime = 0.1f;
    public float m_deactivationTime = 9999.0f;



}
