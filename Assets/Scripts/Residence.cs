using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Residence : Structure
    {
        public int maxResidents;

        [NonSerialized]
        public HashSet<Person> residents = new HashSet<Person>();

        [NonSerialized]
        public int bonusHappiness = 0;

        public override void Awake()
        {
            city.availableResidences[(int)socialClass].Add(this);

            base.Awake();
        }

        public override void Destroy()
        {
            foreach(Person person in residents)
            {
                Evict(person);
            }

            base.Destroy();
        }

        public override void Tick()
        {
            base.Tick();

            //TODO: Upgrade/Downgrade to a higher/lower social class if more than 85% of the tenants are from a higher/lower level. Also upgrade the building models.
        }

        public void UpdateSocialClass(SocialClass newSocialClass)
        {
            if(city.availableResidences[(int)socialClass].Contains(this))
            {
                city.availableResidences[(int)socialClass].Remove(this);
            }

            city.availableResidences[(int)newSocialClass].Add(this);

            List<Person> evicted = new List<Person>();

            foreach(Person person in residents)
            {
                if(person.GetSocialClass() > newSocialClass)
                {
                    RecalculateResidentHappinessBasedOnSocialClass(person, socialClass, newSocialClass);

                    person.isLookingForBetterPlace = true;
                }
                else if(person.GetSocialClass() < newSocialClass)
                {
                    if(person.lifeStage == LifeStage.Adult || person.lifeStage == LifeStage.Senior)
                    {
                        evicted.Add(person);

                        if(person.relationshipPartner != null && person.relationshipPartner.residence == person.residence)
                        {
                            evicted.Add(person.relationshipPartner);
                        }

                        foreach(Person child in person.children)
                        {
                            if(child.lifeStage == LifeStage.Infant || child.lifeStage == LifeStage.Youth)
                            {
                                evicted.Add(child);
                            }
                        }
                    }
                }
                else
                {
                    RecalculateResidentHappinessBasedOnSocialClass(person, socialClass, newSocialClass);

                    person.isLookingForBetterPlace = false;
                }
            }

            foreach(Person person in evicted)
            {
                Evict(person);
            }

            socialClass = newSocialClass;
        }

        public void RentFor(Person person)
        {
            if(person.residence != null)
            {
                person.residence.Evict(person);
            }

            residents.Add(person);

            person.residence = this;
            person.happiness += (int)(10*GetSocialClassValue());

            foreach(Person resident in residents)
            {
                ref Tile tile = ref city.GetTile(position);

                foreach(int coverage in tile.coverages)
                {
                    resident.happiness += coverage;
                }
            }

            person.isLookingForBetterPlace = false;

            if(maxResidents - residents.Count <= 0)
            {
                city.availableResidences[(int)socialClass].Remove(this);
            }
        }

        public void Evict(Person person)
        {
            residents.Remove(person);

            person.residence = null;
            person.happiness -= (int)(10*GetSocialClassValue());

            foreach(Person resident in residents)
            {
                ref Tile tile = ref city.GetTile(position);

                foreach(int coverage in tile.coverages)
                {
                    resident.happiness -= coverage;
                }
            }

            person.isLookingForBetterPlace = false;

            if(!city.availableResidences[(int)socialClass].Contains(this))
            {
                city.availableResidences[(int)socialClass].Add(this);
            }
        }

        public void RecalculateResidentHappinessBasedOnSocialClass(Person resident, SocialClass oldSocialClass, SocialClass newSocialClass)
        {
            resident.happiness -= (int)(10*socialClassValueTable[(int)oldSocialClass]);
            resident.happiness += (int)(10*socialClassValueTable[(int)newSocialClass]);
        }

        public override void OnChangeCoverage(Coverage coverage, int from, int to)
        {
            base.OnChangeCoverage(coverage, from, to);

            foreach(Person resident in residents)
            {
                resident.happiness -= from;
                resident.happiness += to;
            }
        }
    }
}