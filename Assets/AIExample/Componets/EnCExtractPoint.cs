using UnityEngine;

public class EnCExtractPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OnTriggered(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggered(collision.gameObject);
    }

    private void OnTriggered(GameObject obj)
    {
        UserBlackboard_BasicBadguy npcData = obj.GetComponent<UserBlackboard_BasicBadguy>();

        if (npcData != null && npcData.m_wantsToExtract == true)
        {
            if (npcData.m_resourceCount >= npcData.m_desiredResourceCount)
            {
                Debug.Log($"npc {obj.name} completed there mission");
                Debug.LogWarning($"npc {obj.name} completed there mission with {npcData.m_resourceCount}/{npcData.m_desiredResourceCount}");
            }
            else
            {
                Debug.Log($"npc {obj.name} failed to complete there mission");
                Debug.LogWarning($"npc {obj.name} failed to complete there mission with {npcData.m_resourceCount}/{npcData.m_desiredResourceCount}");
            }

            Destroy(obj);
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
