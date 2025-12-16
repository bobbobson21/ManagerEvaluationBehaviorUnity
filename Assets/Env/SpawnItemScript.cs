using System.Collections.Generic;
using UnityEngine;

public class SpawnItemScript : MonoBehaviour
{
    public float m_respawnDelay = 1.5f;
    public float m_oddsOfSpawning = 0.75f;

    private float m_currentDelay = 0;

    public List<GameObject> m_items = new List<GameObject>();
    private GameObject m_lastitem = null;

    void Start()
    {
        MeshRenderer render = gameObject.GetComponent<MeshRenderer>();

        if (render != null)
        { 
            Destroy(render);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_lastitem == null)
        {
            m_currentDelay += Time.deltaTime;

            if (m_currentDelay >= m_respawnDelay)
            {
                if (Random.Range(0.0f, 1.0f) <= m_oddsOfSpawning)
                {
                    int index = Random.Range(0, m_items.Count);

                    m_lastitem = Instantiate(m_items[index], transform);
                }

                m_currentDelay = 0;
            }
        }
    }
}
