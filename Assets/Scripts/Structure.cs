using UnityEngine;

namespace Game
{
    public class Structure : MonoBehaviour
    {
        public Vector2Int position;
        public string displayName;
        public string description;
        public int cost;
        public int upkeepCost;

        public virtual void Awake()
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual void Buy()
        {
        }

        public virtual void Sell()
        {
        }

        public virtual void Demolish()
        {
        }
    }
}