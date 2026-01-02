using MEBS.Editor;
using MEBS.Runtime;

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        data.m_displayDiscription = "finds rooms that are near by and searchs them" +
            "\n\nvaild blackboard data: " +
            "\nstoreTargetLocationIn: (vector3BlackboardKeyAsString)";
        return data;
    }
}
#endif

public class UserManger_SearchNearByRooms : MEB_BaseManager, MEB_I_IntScoop
{
    private AICRoomPoints m_currentRoom = null;
    private List<AICRoomPoints> m_clearedRooms = new List<AICRoomPoints>();
    private string m_storeTargetLocationInKey = "";
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
        AICRoomPoints destanationRoom = null;
        float destanationDist = float.MaxValue;

        if(m_currentRoom != null)
        {
            return;
        }

        for (int i = 0; i < AICRoomPoints.m_totalRoomPoints.Count; i++)
        {
            if(m_clearedRooms.Contains(AICRoomPoints.m_totalRoomPoints[i]) == false)
            {
                float currentDist = (m_director.m_gameObject.transform.position - AICRoomPoints.m_totalRoomPoints[i].gameObject.transform.position).magnitude;

                if(currentDist <= destanationDist && currentDist <= AICRoomPoints.m_totalRoomPoints[i].m_reachRadius)
                {
                    destanationDist = currentDist;
                    destanationRoom = AICRoomPoints.m_totalRoomPoints[i];
                }
            }
        }

        m_currentRoom = destanationRoom;

        if(m_currentRoom == null)
        {
            BlockMoveToExecutionForCycle();
        }
    }

    public override void OnInitialized()
    {
        //put on loaded into game code here
    }

    public override void OnStart() //put stuff in these if you need something to happen when the manager leaves or enters exacuteion
    {
    }

    public override void OnEnd()
    {
        m_currentRoom = null;
    }

    public override void OnUpdate(float delta, int index)
    {
        m_director.m_blackboard.SetObject(m_storeTargetLocationInKey, m_currentRoom.transform.position);

        if((m_director.m_gameObject.transform.position - m_currentRoom.gameObject.transform.position).magnitude <= m_currentRoom.m_clearRadius)
        {
            m_clearedRooms.Add(m_currentRoom);
            m_currentRoom = null;
        }
    }

    public int GetIntEvalValue()
    {
        return 2;
    }
}
