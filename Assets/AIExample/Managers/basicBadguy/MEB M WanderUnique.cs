using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;


public class Manager_WanderUniqueSetting : MEB_BaseBehaviourData_ItemSettings
{
    public float m_maxRadius = 10;
    public float m_minRadius = 5;
    public float m_delayBetweenWandering = 1;

    public int m_uniquePointCount = 6;
    public int m_maxAttemptsForUniquePoint = 12;
    public float m_uniqueRadius = 4;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            if (MEB_UI_BehaviourEditor.InRestrictedEditMode() == false)
            {
                float.TryParse(EditorGUILayout.TextField("max radius", m_maxRadius.ToString()), out m_maxRadius);
                float.TryParse(EditorGUILayout.TextField("min radius", m_minRadius.ToString()), out m_minRadius);
                GUILayout.Space(8);

                float.TryParse(EditorGUILayout.TextField("delay", m_delayBetweenWandering.ToString()), out m_delayBetweenWandering);
                GUILayout.Space(8);

                int.TryParse(EditorGUILayout.TextField("unique point count", m_uniquePointCount.ToString()), out m_uniquePointCount);
                int.TryParse(EditorGUILayout.TextField("unique point attempts", m_maxAttemptsForUniquePoint.ToString()), out m_maxAttemptsForUniquePoint);
                float.TryParse(EditorGUILayout.TextField("unique radius", m_uniqueRadius.ToString()), out m_uniqueRadius);
            }
            else
            {
                MEB_GUI_Styles.BeginLockedTextStyle();

                EditorGUILayout.TextField("max radius", m_maxRadius.ToString());
                EditorGUILayout.TextField("min radius", m_minRadius.ToString());
                GUILayout.Space(8);

                EditorGUILayout.TextField("delay", m_delayBetweenWandering.ToString());
                GUILayout.Space(8);

                EditorGUILayout.TextField("unique point count", m_uniquePointCount.ToString());
                EditorGUILayout.TextField("unique point attempts", m_maxAttemptsForUniquePoint.ToString());
                EditorGUILayout.TextField("unique radius", m_uniqueRadius.ToString());

                MEB_GUI_Styles.EndLockedTextStyle();
            }
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_WanderUnique_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_WanderUnique_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_WanderUnique_UI());
    }

    public UserManger_WanderUnique_UI()
    {
        m_name = "UserManger_WanderUnique";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        Manager_WanderUniqueSetting data = new Manager_WanderUniqueSetting();
        data.m_class = "UserManger_WanderUnique";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Makes the NPC wander around the place at will.\n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_WanderUnique : MEB_BaseManager, MEB_I_IntScoop
{
    private float m_maxRadius = 10;
    private float m_minRadius = 5;
    private float m_delayBetweenWandering = 1;

    private int m_uniquePointCount = 6;
    private int m_maxAttemptsForUniquePoint = 12;
    private float m_uniqueRadius = 15;

    private int m_currentUniquePointCount = 0;
    private List<Vector3> m_uniquePoints = new List<Vector3>();

    private string m_storeTargetLocationInKey = "";
    private float m_currentTimeLeftTillNextWanderCycle = 0;

    public override void SetBlackboardKeys(List<string> idenifyers, List<string> keys)
    {
        for (int i = 0; i < idenifyers.Count; i++)
        {
            if (idenifyers[i] == "storeTargetLocationIn")
            {
                m_storeTargetLocationInKey = keys[i];
            }
        }
    }

    public override void EvaluationEnd(int index, float delta)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        Manager_WanderUniqueSetting settings = (Manager_WanderUniqueSetting)m_itemSettings;

        if (settings != null)
        {
            m_maxRadius = settings.m_maxRadius;
            m_minRadius = settings.m_minRadius;
            m_delayBetweenWandering = settings.m_delayBetweenWandering;

            m_uniquePointCount = settings.m_uniquePointCount;
            m_maxAttemptsForUniquePoint = settings.m_maxAttemptsForUniquePoint;
            m_uniqueRadius = settings.m_uniqueRadius;
        }

        for (int i = 0; i < m_uniquePointCount; i++)
        {
            m_uniquePoints.Add(Vector3.negativeInfinity);
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
        //Debug.Log("wander");

        m_currentTimeLeftTillNextWanderCycle -= delta;

        if (m_currentTimeLeftTillNextWanderCycle < 0)
        {
            Vector3 finalpos = m_director.m_gameObject.transform.position;
            bool foundPos = false;

            for (int i = 0; i < m_maxAttemptsForUniquePoint && foundPos == false; i++)
            {
                Vector3 pos = m_director.m_gameObject.transform.position;
                pos += (new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized) * Random.Range(m_minRadius, m_maxRadius);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(pos, out hit, m_maxRadius, NavMesh.AllAreas))
                {
                    finalpos = hit.position;
                    foundPos = true;

                    for (int o = 0; o < m_uniquePoints.Count; o++)
                    {
                        if ((m_director.m_gameObject.transform.position - m_uniquePoints[o]).magnitude <= m_uniqueRadius)
                        { 
                            foundPos = false;
                            break;
                        }
                    }

                    if (foundPos == true)
                    {
                        if (m_currentUniquePointCount >= m_uniquePointCount)
                        {
                            m_currentUniquePointCount = 0;
                        }

                        m_uniquePoints[m_currentUniquePointCount] = finalpos;
                        m_currentUniquePointCount++;
                    }
                }
            }

            if (foundPos == false && (m_director.m_gameObject.transform.position - m_uniquePoints[0]).magnitude > m_uniqueRadius)
            {
                foundPos = true;
                finalpos = m_uniquePoints[0];
            }

            m_currentTimeLeftTillNextWanderCycle = m_delayBetweenWandering;
            m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, finalpos);
        }
    }

    public int GetIntEvalValue(float delta)
    {
        return 1;
    }
}
