using MEBS.Runtime;
using UnityEngine;

public class AmmoTrigger : MonoBehaviour
{
    public int m_maxAmmo = 10;
    public int m_minAmmo = 2;

    private void OnCollisionEnter(Collision other)
    {
        AICGun gun = other.gameObject.GetComponentInChildren<AICGun>();

        if (gun != null)
        {
            gun.AddAmmoToTotalClip(Random.Range(m_minAmmo, m_maxAmmo));

            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
