using System;
using MEBS.Runtime;
using UnityEngine;

public class UserBlackboard_BasicBadguy : MEB_BaseBlackboard
{
    public GameObject m_attackerObj = null;
    public GameObject m_healthObject = null;
    public GameObject m_ammoObject = null;
    public GameObject m_eyeObject = null;

    public Vector3 m_movePos = Vector3.zero;
    public int m_healthMax = 100;
    public int m_health = 100;

    public int m_ammoMax = 50;
    public int m_ammoTotal = 50;
    public int m_ammoClip = 25;

    public override object GetObject(string key)
    {
        switch (key)
        {
            case "attackerObj":
                return m_attackerObj;

            case "healthObject":
                return m_healthObject;

            case "ammoObject":
                return m_ammoObject;

            case "movePos":
                return m_movePos;

            case "healthMax":
                return m_healthMax;

            case "health":
                return m_health;

            case "ammoTotal":
                return m_ammoTotal;

            case "ammoMax":
                return m_ammoMax;

            case "ammoCurrentClip":
                return m_ammoClip;

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

            case "eyeObject":
                m_eyeObject = (GameObject)data;
                break;

            case "healthObject":
                m_healthObject = (GameObject)data;
                break;

            case "ammoObject":
                m_ammoObject = (GameObject)data;
                break;

            case "movePos":
                m_movePos = (Vector3)data;
                break;

            case "healthMax":
                m_healthMax = (int)data;
                break;

            case "health":
                m_health = (int)data;
                break;

            case "ammoTotal":
                m_ammoTotal = (int)data;
                break;

            case "ammoMax":
                m_ammoMax = (int)data;
                break;

            case "ammoCurrentClip":
                m_ammoClip = (int)data;
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

