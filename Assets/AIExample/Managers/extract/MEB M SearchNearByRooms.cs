using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class Manager_SearchNearByRoomsSetting : MEB_BaseBehaviourData_ItemSettings
{
    public float m_randomRadiusMax = 6.0f;
    public float m_randomRadiusMin = 4.0f;
    public float m_closingDistance = 2.0f;
    public float m_extent = 2;
    public float m_maxAttempts = 6;

    public override void OnGUI()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox); //start of blackboard settings
        m_displayCustomSettingExpanded = EditorGUILayout.Foldout(m_displayCustomSettingExpanded, "custom values");

        if (m_displayCustomSettingExpanded == true)
        {
            float.TryParse(EditorGUILayout.TextField("max radius", m_randomRadiusMax.ToString()), out m_randomRadiusMax);
            float.TryParse(EditorGUILayout.TextField("min radius", m_randomRadiusMin.ToString()), out m_randomRadiusMin);
            float.TryParse(EditorGUILayout.TextField("close radius", m_closingDistance.ToString()), out m_closingDistance);
            GUILayout.Space(8);

            float.TryParse(EditorGUILayout.TextField("extent", m_extent.ToString()), out m_extent);
            GUILayout.Space(8);

            float.TryParse(EditorGUILayout.TextField("attempts", m_maxAttempts.ToString()), out m_maxAttempts);
        }

        GUILayout.EndVertical();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class UserManger_SearchNearByRooms_UI : MEB_UI_BehaviourEditor_ManagerData
{
    static UserManger_SearchNearByRooms_UI()
    {
        MEB_UI_BehaviourEditor.AddNormalManager(new UserManger_SearchNearByRooms_UI());
    }

    public UserManger_SearchNearByRooms_UI()
    {
        m_name = "UserManger_SearchNearByRooms";
    }

    public override MEB_BaseBehaviourData_ItemSettings CreateInstance()
    {
        MEB_BaseBehaviourData_ItemSettings data = new MEB_BaseBehaviourData_ItemSettings();
        data.m_class = "UserManger_SearchNearByRooms";
        data.m_displayName = m_name;
        data.m_displayDiscription = "Finds and moves the NPC to nextdoor rooms. \n\nvaild blackboard data: \nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";

        return data;
    }
}
#endif

public class UserManger_SearchNearByRooms : MEB_BaseManager, MEB_I_IntScoop
{
    private string m_storeTargetLocationInKey = "";

    private int m_lastRoomIndex = -1;
    private List<Vector3> m_possibleRoomLocations = new List<Vector3>();
    private NavMeshAgent m_agent = null;
    bool m_doneScan = false;

    private float m_randomRadiusMax = 6.0f;
    private float m_randomRadiusMin = 4.0f;
    private float m_closingDistance = 2.0f;
    private float m_extent = 2;
    private float m_maxAttempts = 6;

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

    public override void EvaluationEnd(int index)
    {
        //put self evaluration code here use BlockMoveToExecutionForCycle if self eval dosent look good
    }

    public override void OnInitialized()
    {
        m_agent = m_director.m_gameObject.GetComponent<NavMeshAgent>();
        Manager_SearchNearByRoomsSetting settings = (Manager_SearchNearByRoomsSetting)m_itemSettings;

        if (settings != null)
        {
            m_randomRadiusMax = settings.m_randomRadiusMax;
            m_randomRadiusMin = settings.m_randomRadiusMin;
            m_closingDistance = settings.m_closingDistance;
            m_extent = settings.m_extent;
            m_maxAttempts = settings.m_maxAttempts;
        }
    }

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
        if (m_lastRoomIndex > 0)
        { 
            m_possibleRoomLocations.RemoveAt(m_lastRoomIndex);
        }
    }

    public override void OnUpdate(float delta, int index)
    {
        FindRooms();
        GoToRoom();
    }

    private void FindRooms()
    {
        m_doneScan = true;

        RaycastHit hitInfo;

        //y+
        Physics.Linecast(m_director.m_gameObject.transform.position, m_director.m_gameObject.transform.position +new Vector3(0,999,0), out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider != null)
        {
            for (int i = 0; i < m_maxAttempts; i++)
            {
                NavMeshHit hit;
                Vector3 pos = hitInfo.point;
                pos += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * Random.Range(m_randomRadiusMin, m_randomRadiusMax);
                pos.y += m_extent;

                if (NavMesh.SamplePosition(pos, out hit, m_randomRadiusMax, NavMesh.AllAreas))
                {
                    m_possibleRoomLocations.Add(hit.position);
                    break;
                }
            }
        }


        //x+
        Physics.Linecast(m_director.m_gameObject.transform.position, m_director.m_gameObject.transform.position + new Vector3(999, 0, 0), out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider != null)
        {
            for (int i = 0; i < m_maxAttempts; i++)
            {
                NavMeshHit hit;
                Vector3 pos = hitInfo.point;
                pos += new Vector3(Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * Random.Range(m_randomRadiusMin, m_randomRadiusMax);
                pos.x += m_extent;

                if (NavMesh.SamplePosition(pos, out hit, m_randomRadiusMax, NavMesh.AllAreas))
                {
                    m_possibleRoomLocations.Add(hit.position);
                    break;
                }
            }
        }


        //x-
        Physics.Linecast(m_director.m_gameObject.transform.position, m_director.m_gameObject.transform.position + new Vector3(-999, 0, 0), out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider != null)
        {
            for (int i = 0; i < m_maxAttempts; i++)
            {
                NavMeshHit hit;
                Vector3 pos = hitInfo.point;
                pos += new Vector3(Random.Range(-1.0f, 0.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized * Random.Range(m_randomRadiusMin, m_randomRadiusMax);
                pos.x -= m_extent;

                if (NavMesh.SamplePosition(pos, out hit, m_randomRadiusMax, NavMesh.AllAreas))
                {
                    m_possibleRoomLocations.Add(hit.position);
                    break;
                }
            }
        }


        //z+
        Physics.Linecast(m_director.m_gameObject.transform.position, m_director.m_gameObject.transform.position + new Vector3(0, 0, 999), out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider != null)
        {
            for (int i = 0; i < m_maxAttempts; i++)
            {
                NavMeshHit hit;
                Vector3 pos = hitInfo.point;
                pos += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(0.0f, 1.0f)).normalized * Random.Range(m_randomRadiusMin, m_randomRadiusMax);
                pos.z += m_extent;

                if (NavMesh.SamplePosition(pos, out hit, m_randomRadiusMax, NavMesh.AllAreas))
                {
                    m_possibleRoomLocations.Add(hit.position);
                    break;
                }
            }
        }


        //z-
        Physics.Linecast(m_director.m_gameObject.transform.position, m_director.m_gameObject.transform.position + new Vector3(0, 0, 999), out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

        if (hitInfo.collider != null)
        {
            for (int i = 0; i < m_maxAttempts; i++)
            {
                NavMeshHit hit;
                Vector3 pos = hitInfo.point;
                pos += new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 0.0f)).normalized * Random.Range(m_randomRadiusMin, m_randomRadiusMax);
                pos.z -= m_extent;

                if (NavMesh.SamplePosition(pos, out hit, m_randomRadiusMax, NavMesh.AllAreas))
                {
                    m_possibleRoomLocations.Add(hit.position);
                    break;
                }
            }
        }
    }

    private void GoToRoom()
    {
        m_lastRoomIndex = -1;

        int ourSearchIndex = -1;
        Vector3 ourSearchPoint = m_director.m_gameObject.transform.position;
        float distance = float.PositiveInfinity;

        for (int i = 0; i < m_possibleRoomLocations.Count; i++) //findes nearest roomk
        {
            float currentDistance = (m_director.m_gameObject.transform.position - m_possibleRoomLocations[i]).magnitude;
            if (currentDistance <= distance)
            {
                ourSearchPoint = m_possibleRoomLocations[i];
                distance = currentDistance;
                ourSearchIndex = i;
            }
        }

        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, ourSearchPoint); //sends it off to movement

        if ((m_director.m_gameObject.transform.position - ourSearchPoint).magnitude <= m_closingDistance && ourSearchIndex > 0) //are we close enougth to remove this room
        {
            m_possibleRoomLocations.RemoveAt(ourSearchIndex);
        }
    }

    public int GetIntEvalValue()
    {
        if (m_agent.velocity.magnitude > 0.5f && m_doneScan == true) { m_doneScan = false; }
        if (m_possibleRoomLocations.Count <= 0 && m_doneScan == true) { return 0; }

        return 2;
    }
}
