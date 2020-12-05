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
        public int id;
        public string name;
        public int age;
        public LifeStage lifeStage;
        public Gender gender;
        public ColorOrRace colorOrRace;
        public Education education;
        public SocialClass socialClass;

        public bool canDie;

        [NonSerialized]
        public List<Person> parents = new List<Person>();

        [NonSerialized]
        public List<Person> children = new List<Person>();

        [NonSerialized]
        public Person relationshipPartner;

        [NonSerialized]
        public int relationshipElapsedTicks;

        [NonSerialized]
        public SocialClass minimumSocialClass;//Used in case the person is unemployed but is very rich

        [NonSerialized]
        public Job job;

        [NonSerialized]
        public bool isLookingForBetterJob;

        [NonSerialized]
        public Residence residence;

        [NonSerialized]
        public bool isLookingForBetterPlace;

        [NonSerialized]
        public List<Structure> propertiesOwned;

        [NonSerialized]
        public bool isSick;
        
        [NonSerialized]
        public int happiness;

        public void Born()
        {
        }

        public void Death()
        {
            // for(int i =
        }

        public void ChangeEducation(Education newEducation)
        {
            education = newEducation;
        }

        public void ChangeSocialClass(SocialClass newSocialClass)
        {
            socialClass = newSocialClass;
        }

        public SocialClass GetSocialClass()
        {
            return (SocialClass)Mathf.Max((int)minimumSocialClass, (int)socialClass);
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

        public void ChangeSicknessState(bool isSickState)
        {
            isSick = isSickState;

            if(isSick)
            {
                happiness -= 20;
            }
            else
            {
                happiness += 20;
            }
        }

        public void RelationshipPartnerChange(Person person)
        {
            relationshipPartner = person;

            relationshipElapsedTicks = 0;
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
        public List<Person> workers;

        [NonSerialized]
        public Workplace workplace;

        public void AddWorker(Person person)
        {
        }

        public void RemoveWorker(Person person)
        {
        }
    }
}