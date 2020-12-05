using System;
using UnityEngine;

namespace Game
{
    public enum Ownership
    {
        Public,
        Private,
    }

    public class Structure : MonoBehaviour
    {
        public string displayName;
        public string description;
        public SocialClass socialClass;
        public int cost;
        public int upkeepCost;
        public Ownership ownership;

        public bool canBeDestroyed;

        [NonSerialized]
        public int revenue;

        [NonSerialized]
        public City city;

        [NonSerialized]
        public Vector2Int position;

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

        public virtual void Tick()
        {
        }

        public void BecomeForSale()
        {
            city.forSaleStructures[(int)socialClass].Add(this);

            //TODO: Add for sale icon
        }

        public void BecomeProperty()
        {
            city.forSaleStructures[(int)socialClass].Remove(this);

            //TODO: Remove for sale icon
        }

        public bool IsForSale()
        {
            return city.forSaleStructures[(int)socialClass].Contains(this);
        }
    }
}