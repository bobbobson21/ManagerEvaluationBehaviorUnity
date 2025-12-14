using MEBS.Runtime;
using System.Collections.Generic;
using System;
using UnityEngine;
using NUnit.Framework;

public class UserBlackboard_baseBuild : MEB_BaseBlackboard
{
    public int m_desiredBuildingType = 0;
    public bool m_hasGotEnougthToBuild = false;

    public int m_mineCount = 0;
    public GameObject m_mineObj = null;
    public Vector3 m_movePos = Vector3.zero;

    public List<GameObject> m_buildableList = new List<GameObject>();

    public static List<UserBlackboard_baseBuild_InfrstructerObject> m_cityBuiltSoFar = new List<UserBlackboard_baseBuild_InfrstructerObject>();

    public override object GetObject(string key)
    {
        switch (key)
        {
            case "desiredBuildingType":
                return m_desiredBuildingType;

            case "hasGotEnougthToBuild":
                return m_hasGotEnougthToBuild;

            case "mineCount":
                return m_mineCount;

            case "mineObj":
                return m_mineObj;

            case "movePos":
                return m_movePos;

            case "buildableList":
                return m_buildableList;

            case "cityBuiltSoFar":
                return m_cityBuiltSoFar;

            default:
                return null;
        }
    }

    public override void SetObject(string key, object data)
    {
        switch (key)
        {
            case "desiredBuildingType":
                m_desiredBuildingType = (int)data;
                break;

            case "hasGotEnougthToBuild":
                m_hasGotEnougthToBuild = (bool)data;
                break;

            case "mineCount":
                m_mineCount = (int)data;
                break;

            case "mineObj":
                m_mineObj = (GameObject)data;
                break;

            case "movePos":
                m_movePos = (Vector3)data;
                break;

            case "buildableList":
                m_buildableList = (List<GameObject>)data;
                break;

            case "cityBuiltSoFar":
                m_cityBuiltSoFar = (List<UserBlackboard_baseBuild_InfrstructerObject>)data;
                break;

            default:
                break;
        }
    }

    public override Type GetObjectAsType(string key)
    {
        return null;
    }
}

public class UserBlackboard_baseBuild_InfrstructerObject
{
    public GameObject m_gameObject = null;
    public int m_type = 0;
    public bool m_canBuildOn = false;

    public List<UserBlackboard_baseBuild_InfrstructerObject> m_buildPoints = new List<UserBlackboard_baseBuild_InfrstructerObject>();
}
