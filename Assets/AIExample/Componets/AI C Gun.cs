using System.Security.Cryptography;
using UnityEngine;

public class AICGun : MonoBehaviour
{
    [Header("ammo")]
    public int m_ClipSize = 25;
    public int m_totalAmmoCount = 50;
    public int m_maxShotFiredPerFiring = 2;

    public float m_delayBetweenShots = 0.1f;

    private float m_currentdelayBetweenShots = 0;
    private int m_ammoInClip = 0;

    [Header("transform")]
    public GameObject m_parentOfGun = null;
    public float m_objectOffset = 110;
    public float m_aimOffset = 20;
    public float m_bulletMaxTravleDistance = 100;

    public void Reload()
    { 
        m_totalAmmoCount -= m_ClipSize;
        m_ammoInClip += m_ClipSize - m_totalAmmoCount;

        if (m_totalAmmoCount < 0)
        {
            m_totalAmmoCount = 0;
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
                Vector3 currentNormal = (transform.position - m_parentOfGun.transform.position).normalized;
                Vector3 firePosStart = m_parentOfGun.transform.position + (currentNormal * (m_objectOffset + m_aimOffset));

                Vector3 firePosEnd = firePosStart + (currentNormal * m_bulletMaxTravleDistance);
            }
        }
    }

    public bool CanFire()
    { 
        return ((m_currentdelayBetweenShots < 0) && (m_ammoInClip > 0));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_currentdelayBetweenShots -= Time.deltaTime;

        //RotateGun(Vector2.zero, 10.0f * Time.deltaTime);
    }
}
