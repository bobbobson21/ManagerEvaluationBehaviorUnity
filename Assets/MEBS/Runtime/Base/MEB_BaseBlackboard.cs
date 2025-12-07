using System;
using UnityEngine;

namespace MEBS.Runtime
{
    public class MEB_BaseBlackboard : MonoBehaviour
    {
        public virtual object GetObject(string key)
        {
            return null;
        }

        public virtual void SetObject(string key, object data)
        {

        }

        public virtual Type GetObjectAsType(string key)
        {
            return null;
        }
    }
}
