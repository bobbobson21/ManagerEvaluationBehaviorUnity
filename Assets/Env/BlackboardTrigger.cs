using MEBS.Runtime;
using UnityEngine;

public class BlackboardTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string keyOfIntValueToMinulate = "";
    public int addToBlackboardValue = 0;

    public int minCap = 0;
    public int MaxCap = 0;

    public bool m_removeOnTrigger = true;

    private void OnCollisionEnter(Collision other)
    {
        MEB_BaseBlackboard baseBlackboard = other.gameObject.GetComponent<MEB_BaseBlackboard>();

        if (baseBlackboard != null)
        {
            int newValue = ((int)baseBlackboard.GetObject(keyOfIntValueToMinulate)) + addToBlackboardValue;

            if (newValue < minCap) { newValue = minCap; }
            if (newValue > MaxCap) { newValue = MaxCap; }

            baseBlackboard.SetObject(keyOfIntValueToMinulate, newValue);

            if (m_removeOnTrigger == true)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
