using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Modifier
    {
        public string name;
        public string description;
    }

    public class Modifiers
    {
        Dictionary<string, Modifier> data;

        public void AddModifier(Modifier modifier)
        {
        }

        public void RemoveModifier(Modifier modifier)
        {
        }
    }

    public enum LifeStage
    {
        Infant,
        Youth,
        Adult,
        Senior,
        Count
    }

    public enum Gender
    {
        CisMan,
        CisWoman,
        TransMan,
        TransWoman,
        NonBinary,
        Other,
        Count
    }

    public enum ColorOrRace
    {
        White,
        Black,
        Brown,
        Yellow,
        Indigenous,
        Other,
        Count
    }

    public enum Education
    {
        Illiterate,
        Primary,
        Secondary,
        Higher,
        Count
    }

    public enum SocialClass
    {
        Poor,
        Working,
        LowerMiddle,
        UpperMidle,
        Upper,
        Count
    }

    [Serializable]
    public class Person
    {
        public string name;
        public int age;
        public LifeStage lifeStage;
        public Gender gender;
        public ColorOrRace colorOrRace;
        public Education education;
        public SocialClass socialClass;

        public bool canDie;

        [NonSerialized]
        public City city = null;

        [NonSerialized]
        public List<Person> parents = new List<Person>();

        [NonSerialized]
        public List<Person> children = new List<Person>();

        [NonSerialized]
        public Person relationshipPartner = null;

        [NonSerialized]
        public int relationshipProgress = 0;

        [NonSerialized]
        public SocialClass minimumSocialClass = SocialClass.Poor;//Used in case the person is unemployed but is very rich

        [NonSerialized]
        public Job job = null;

        [NonSerialized]
        public bool isLookingForBetterJob = false;

        [NonSerialized]
        public Residence residence = null;

        [NonSerialized]
        public bool isLookingForBetterPlace = false;

        [NonSerialized]
        public School school = null;

        [NonSerialized]
        public int eudcationProgress = 0;

        [NonSerialized]
        public Hospital hospital = null;

        [NonSerialized]
        public int sicknessProgress = 0;

        [NonSerialized]
        public List<Structure> propertiesOwned = new List<Structure>();

        [NonSerialized]
        public int happiness = 0;

        public void Born()
        {
        }

        public void Death()
        {
            city.people.Remove(this);

            if(job != null)
            {
                job.workplace.Fire(this);
            }

            if(residence != null)
            {
                residence.Evict(this);
            }

            if(school != null)
            {
                school.Dismiss(this);
            }

            if(hospital != null)
            {
                hospital.DischargePatient(this);
            }

            RemoveRelationshipParter();

            foreach(Person parent in parents)
            {
                parent.children.Remove(this);

                parent.happiness -= 3;
            }

            foreach(Person child in children)
            {
                child.parents.Remove(this);

                if(child.lifeStage == LifeStage.Infant && child.parents.Count <= 0)
                {
                    child.Death();//The parent was already removed, so the child's death it won't affect this loop. Probably...
                }
                else
                {
                    child.happiness -= 3;
                }
            }
        }

        public void ChangeEducation(Education newEducation)
        {
            education = newEducation;
        }

        public void ChangeSocialClass(SocialClass newSocialClass)
        {
            socialClass = newSocialClass;

            if((int)newSocialClass > (int)socialClass)
            {
                happiness += 10;
            }
            else if((int)newSocialClass < (int)socialClass)
            {
                happiness -= 10;
            }
        }

        public SocialClass GetSocialClass()
        {
            if(relationshipPartner == null)
            {
                return (SocialClass)Mathf.Max((int)minimumSocialClass, (int)socialClass);
            }
            else
            {
                return (SocialClass)Mathf.Max((int)minimumSocialClass, (int)socialClass, (int)relationshipPartner.socialClass);
            }
        }

        public void ChangeJob(Job newJob)
        {
            job = newJob;
        }

        public void AcquireProperty(Structure structure)
        {
            propertiesOwned.Add(structure);

            if((int)structure.socialClass  > (int)minimumSocialClass)
            {
                minimumSocialClass = structure.socialClass;
            }
        }

        public void LoseProperty(Structure structure)
        {
            structure.BecomeForSale();

            propertiesOwned.Remove(structure);

            SocialClass newSocialClass = SocialClass.Poor;

            for(int i = 0; i < propertiesOwned.Count; ++i)
            {
                Structure ownedStructure = propertiesOwned[i];

                if(ownedStructure.socialClass >= newSocialClass)
                {
                    newSocialClass = ownedStructure.socialClass;
                }
            }

            minimumSocialClass = newSocialClass;
        }

        public void AddSickness(int amount)
        {
            if(sicknessProgress <= 0)
            {
                happiness -= 20;
            }

            sicknessProgress += amount;
        }

        public void RemoveSickness()
        {
            sicknessProgress = 0;

            happiness += 20;
        }

        public bool IsSick()
        {
            return sicknessProgress > 0;
        }

        public void AddRelationshipPartner(Person partner)
        {
            relationshipPartner = partner;
            relationshipProgress = 0;

            partner.relationshipPartner = this;
            partner.relationshipProgress = 0;
        }

        public void RemoveRelationshipParter()
        {
            relationshipPartner.relationshipPartner = null;
            relationshipPartner.relationshipProgress = 0;

            relationshipPartner = null;
            relationshipProgress = 0;
        }

        public int CalculateHappniess(Modifiers modifiers)
        {
            int total = happiness;

            return total;
        }
    }

    public enum Quality
    {
        Abysmal,
        Low,
        Good,
        Excellent,
        Count
    }

    [Serializable]
    public class Job
    {
        public string name;
        public Education educationRequired;
        public SocialClass socialClassProvided;
        public Quality quality;

        public int maxWorkers;

        [NonSerialized]
        public HashSet<Person> workers;

        [NonSerialized]
        public Workplace workplace = null;

        public void AddWorker(Person person)
        {
        }

        public void RemoveWorker(Person person)
        {
        }
    }
}