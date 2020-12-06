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
            //TODO: Upgrade/Downgrade to a higher/lower social class if more than 85% of the tenants are from a higher/lower level

            base.Tick();
        }

        public void UpdateSocialClass(SocialClass newSocialClass)
        {
            if(city.availableResidences[(int)socialClass].Contains(this))
            {
                city.availableResidences[(int)socialClass].Remove(this);
            }

            city.availableResidences[(int)newSocialClass].Add(this);

            List<Person> family = new List<Person>();

            foreach(Person person in residents)
            {
                if(person.GetSocialClass() < newSocialClass)
                {
                    continue;
                }
                else if(person.GetSocialClass() > newSocialClass)
                {
                    person.isLookingForBetterPlace = true;
                }

                if(person.lifeStage == LifeStage.Adult || person.lifeStage == LifeStage.Senior)
                {
                    family.Add(person);

                    if(person.relationshipPartner != null && person.relationshipPartner.residence == person.residence)
                    {
                        family.Add(person.relationshipPartner);
                    }

                    for(int childIndex = 0; childIndex < person.children.Count; ++childIndex)
                    {
                        Person child = person.children[childIndex];

                        if(child.lifeStage == LifeStage.Infant || child.lifeStage == LifeStage.Youth)
                        {
                            family.Add(child);
                        }
                    }
                }
            }

            List<Person> evicted = new List<Person>();

            foreach(Person person in evicted)
            {
                Evict(person);
            }
        }

        public void RentFor(Person person)
        {
            if(person.residence != null)
            {
                person.residence.Evict(person);
            }

            residents.Add(person);

            person.residence = this;
            person.happiness += (int)(10*(float)socialClass*1.5f);
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
            person.happiness -= (int)(10*(float)socialClass*1.5f);
            person.isLookingForBetterPlace = false;

            if(!city.availableResidences[(int)socialClass].Contains(this))
            {
                city.availableResidences[(int)socialClass].Add(this);
            }
        }
    }
}