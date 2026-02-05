using System;
using MEBS.Runtime;
using UnityEngine;


/*#if MEB_BLACKBOARD_UI_INTERACTOR
    ClassName, UserBlackboard_BasicBadguy;

     GameObject, m_attackerObj, null;
     GameObject, m_healthObject, null;
     GameObject, m_ammoObject, null;
     GameObject, m_eyeObject, null;
     GameObject, m_resourceObject, null;
     GameObject, m_extractObject, null;

     Vector3, m_movePos, 0, 0, 0;
     int, m_healthMax, 100;
     int, m_health, 100;

     int, m_ammoMax, 50;
     int, m_ammoTotal, 50;
     int, m_ammoClip, 25;

     int, m_resourceCount, 0;
     int, m_desiredResourceCount, 0;
     bool, m_wantsToExtract, false;
     bool, m_extraAgressive, false;
#endif*/


public class UserBlackboard_BasicBadguy : MEB_BaseBlackboard
{
    public GameObject m_attackerObj = null;
    public GameObject m_healthObject = null;
    public GameObject m_ammoObject = null;
    public GameObject m_eyeObject = null;
    public GameObject m_resourceObject = null;
    public GameObject m_extractObject = null;

    public Vector3 m_movePos = Vector3.zero;
    public int m_healthMax = 100;
    public int m_health = 100;

    public int m_ammoMax = 50;
    public int m_ammoTotal = 50;
    public int m_ammoClip = 25;

    public int m_resourceCount = 0;
    public int m_desiredResourceCount = 0;
    public bool m_wantsToExtract = false;
    public bool m_extraAgressive = false;

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

            case "resourceObject":
                return m_resourceObject;

            case "extractObject":
                return m_extractObject;

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

            case "resourceCount":
                return m_resourceCount;

            case "desiredResourceCount":
                return m_desiredResourceCount;

            case "wantsToExtract":
                return m_wantsToExtract;

            case "extraAgressive":
                return m_extraAgressive;

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

            case "resourceObject":
                m_resourceObject = (GameObject)data;
                break;

            case "extractObject":
                m_extractObject = (GameObject)data;
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

            case "resourceCount":
                m_resourceCount = (int)data;
                break;

            case "desiredResourceCount":
                m_desiredResourceCount = (int)data;
                break;

            case "wantsToExtract":
                m_wantsToExtract = (bool)data;
                break;

            case "extraAgressive":
                m_extraAgressive = (bool)data;
                break;

            default:
                break;
        }
    }

    public override Type GetObjectAsType(string key)
    {
        return null;
    }

    private void Start()
    {
        m_desiredResourceCount = UnityEngine.Random.Range(0, 60);
    }
}

