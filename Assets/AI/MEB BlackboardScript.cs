using System;
using MEBS.Runtime;
using UnityEngine;

public class UserBlackboard_BasicBadguy : MEB_BaseBlackboard
{
    public GameObject m_attackerObj = null;
    public Vector3 m_movePos = Vector3.zero;
    public int m_health = 100;
    public int m_ammo = 25;

    public override object GetObject(string key)
    {
        switch (key)
        {
            case "attackerObj":
                return m_attackerObj;

            case "movePos":
                return m_movePos;

            case "health":
                return m_health;

            case "ammo":
                return m_ammo;

            default:
                return null;
        }
    }

    public override void SetObject(string key, object data)
    {
        switch (key)
        {
            case "attackerObj":
                m_attackerObj = (GameObject)data;
                break;

            case "movePos":
                m_movePos = (Vector3)data;
                break;

            case "health":
                m_health = (int)data;
                break;

            case "ammo":
                m_ammo = (int)data;
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

