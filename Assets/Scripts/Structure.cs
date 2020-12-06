using System;
using UnityEngine;

namespace Game
{
    public enum Ownership
    {
        Public,
        Private,
    }

    public enum Budget
    {
        Low,
        Medium,
        High,
        Count,
    }

    public class Structure : MonoBehaviour
    {
        [Header("Structure")]
        public string displayName;
        public string description;
        public int cost;
        public int upkeepCost;
        public Budget budget;
        public Ownership ownership;

        public bool canBeDestroyed;

        public Coverage influence;
        
        [Range(0, 5)]
        public int influenceAmount;

        [Range(0, 5)]
        public int influenceRange;

        public SocialClass socialClass;
        public float[] socialClassValueTable = 
        {
            0.3f,
            0.7f,
            1.0f,
            1.2f,
            2.0f,
        };

        [NonSerialized]
        public int revenue = 0;

        [NonSerialized]
        public City city = null;

        [NonSerialized]
        public Vector2Int position;

        public virtual void Awake()
        {
            ChangeInfluence(influenceAmount, influenceRange);
        }

        public virtual void Destroy()
        {
            ChangeInfluence(-influenceAmount, influenceRange);
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

        public void ChangeBudget(Budget newBudget)
        {
            Budget oldBudget = budget;

            budget = newBudget;

            ChangeInfluence(-influenceAmount, influenceRange);
            ChangeInfluence(influenceAmount*((int)budget + 1), influenceRange*((int)budget + 1));

            OnChangeBudget(oldBudget, newBudget);
        }

        public virtual void OnChangeBudget(Budget oldBudget, Budget newBudget)
        {
        }

        public void ChangeInfluence(int newInfluenceAmount, int newRangeAmount)
        {
            if(influence == Coverage.None)
            {
                return;
            }

            for(int i = -newRangeAmount; i < newRangeAmount; ++i)
            {
                for(int j = -newRangeAmount; j < newRangeAmount; ++j)
                {
                    Vector2Int tilePosition = new Vector2Int(position.y + i, position.x + j);

                    if(tilePosition.x < 0 || tilePosition.x >= city.gridWidth || tilePosition.y < 0 || tilePosition.y >= city.gridHeight)
                    {
                        continue;
                    }

                    ref Tile tile = ref city.GetTile(tilePosition);

                    float multiplier = ((float)Vector2Int.Distance(position, tilePosition))/newRangeAmount;

                    tile.ChangeCoverage(influence, (int)(newInfluenceAmount*multiplier));
                }
            }

            influenceAmount = newInfluenceAmount;
            influenceRange = newRangeAmount;
        }

        public virtual void OnChangeCoverage(Coverage coverage, int from, int to)
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

        public int GetUpkeepCost()
        {
            return upkeepCost*((int)budget + 1);
        }

        public float GetSocialClassValue()
        {
            return socialClassValueTable[(int)socialClass];
        }
    }
}