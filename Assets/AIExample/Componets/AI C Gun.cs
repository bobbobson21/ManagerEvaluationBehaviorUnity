using MEBS.Runtime;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AICGun : MonoBehaviour
{
    [Header("ammo")]
    public int m_ClipSize = 25;
    public int m_totalAmmoCountMax = 50;
    public int m_maxShotFiredPerFiring = 2;

    public float m_delayBetweenShots = 0.1f;
    public float m_delayBetweenReload = 0.2f;

    private float m_currentdelayBetweenShots = 0;
    private int m_totalAmmoCount = 0;
    private int m_ammoInClip = 0;

    [Header("transform")]
    public GameObject m_parentOfGun = null;
    public float m_objectOffset = 110;
    public float m_aimOffset = 20;
    public float m_bulletMaxTravleDistance = 100;
    public float m_bulletSpread = 2;

    [Header("damage")]
    public int m_minDamage = 1;
    public int m_maxDamage = 4;

    [Header("render")]
    public ParticleSystem m_particleSystem = null;

    private Vector3 m_linePosStartDebug = Vector3.zero;
    private Vector3 m_linePosEndDebug = Vector3.zero;
    private Vector3 m_linePosHitPosDebug = Vector3.zero;
    private bool m_renderAsSuccessfulDebug = true;

    public void AddAmmoToTotalClip(int ammo)
    {
        m_totalAmmoCount += ammo;

        if (ammo > m_totalAmmoCountMax)
        {
            m_totalAmmoCount = m_totalAmmoCountMax;
        }
    }

    public void Reload()
    {
        while (m_totalAmmoCount > 0 && m_ammoInClip < m_ClipSize)
        {
            m_ammoInClip++;
            m_totalAmmoCount--;

            m_currentdelayBetweenShots = m_delayBetweenReload;
        }
    }

    public void RotateGun(Vector3 rotateTowards, float speed)
    { 
        Vector3 currentNormal = (transform.position -m_parentOfGun.transform.position).normalized;
        Vector3 thereNormal = (rotateTowards - m_parentOfGun.transform.position).normalized;

        currentNormal.y = 0;
        thereNormal.y = 0;

        Vector3 res = Vector3.Lerp(currentNormal, thereNormal, speed);

        transform.position = m_parentOfGun.transform.position +(res * m_objectOffset);
        transform.rotation = Quaternion.LookRotation(new Vector3(0, 1, 0), res);
    }

    public void FireGun()
    {
        if (m_currentdelayBetweenShots > 0)
        {
            return;
        }

        m_currentdelayBetweenShots = m_delayBetweenShots;

        for (int i = 0; i < m_maxShotFiredPerFiring; i++)
        {
            if (m_ammoInClip > 0)
            {
                m_ammoInClip--;
                Vector3 currentNormal = (transform.position -m_parentOfGun.transform.position).normalized;
                Vector3 firePosStart = m_parentOfGun.transform.position + (currentNormal * (m_objectOffset + m_aimOffset));

                Vector3 firePosEnd = firePosStart + (currentNormal * m_bulletMaxTravleDistance);
                firePosEnd += ((new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized) * Random.Range(0.0f, m_bulletSpread));

                m_linePosStartDebug = firePosStart;
                m_linePosEndDebug = firePosEnd;

                m_linePosHitPosDebug = Vector3.zero;
                m_renderAsSuccessfulDebug = false;

                RaycastHit hitInfo;
                Physics.Linecast(firePosStart, firePosEnd, out hitInfo, Physics.AllLayers, QueryTriggerInteraction.Ignore);

                m_linePosHitPosDebug = hitInfo.point;

                if (hitInfo.collider != null)
                {
                    MEB_BaseBlackboard baseBlackboard = hitInfo.collider.gameObject.GetComponent<MEB_BaseBlackboard>();

                    if (baseBlackboard != null)
                    {
                        m_renderAsSuccessfulDebug = true;

                        baseBlackboard.SetObject("attackerObj", m_parentOfGun);
                        baseBlackboard.SetObject("health", ((int)baseBlackboard.GetObject("health")) - Random.Range(m_minDamage, m_maxDamage));

                        if ((int)baseBlackboard.GetObject("health") <= 0)
                        {
                            MEB_BaseBlackboard parentBlackboard = m_parentOfGun.GetComponent<MEB_BaseBlackboard>();

                            if(parentBlackboard != null)
                            {
                                parentBlackboard.SetObject("resourceCount", ((int)parentBlackboard.GetObject("resourceCount")) + ((int)baseBlackboard.GetObject("resourceCount")));
                            }
                        }
                    }
                }
            }
        }

        m_particleSystem.Play();
    }

    public bool CanFire()
    { 
        return ((m_currentdelayBetweenShots < 0) && (m_ammoInClip > 0));
    }

    public int GetAmmoInClip()
    {
        return m_ammoInClip;
    }

    public int GetTotalAmmo()
    {
        return m_totalAmmoCount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_totalAmmoCount = m_totalAmmoCountMax;
        Reload();

        m_totalAmmoCount = m_totalAmmoCountMax;
        m_currentdelayBetweenShots = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_currentdelayBetweenShots -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = new Color(1f, 1f, 0f, 1f); // Yellow with custom alpha

        Gizmos.DrawLine(m_linePosStartDebug, m_linePosEndDebug);

        Gizmos.color = new Color(1f, 0f, 0f, 1f); // Yellow with custom alpha

        if (m_renderAsSuccessfulDebug == true)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 1f); // Yellow with custom alpha
        }

        if (m_linePosHitPosDebug != Vector3.zero)
        {
            Gizmos.DrawSphere(m_linePosHitPosDebug, 0.1f);
        }

    }
}
