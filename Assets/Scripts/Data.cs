using System;
using System.Collections.Generic;

namespace Game
{
    public enum LifeStage
    {
        Infant,
        Child,
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
        Working,
        LowerMiddle,
        UpperMidle,
        LowerUpper,
        Upper,
        Count
    }

    public enum Status
    {
        Healty,
        Sick,
        Pregnant,
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

        public Job job;
        public List<Structure> propertiesOwned;

        public Status status;
        public int statusProgress;
        
        public int happiness;

        public void ChangeEducation(Education newEducation)
        {
            education = newEducation;
        }

        public void ChangeSocialClass(SocialClass newSocialClass)
        {
            socialClass = newSocialClass;
        }

        public void ChangeJob(Job newJob)
        {
            job = newJob;
        }

        public void OwnProperty(Structure structure)
        {
            propertiesOwned.Add(structure);
        }

        public void ChangeStatus(Status newStatus)
        {
            status = newStatus;
        }

        public void UpdateStatusProgress(int amount)
        {
            statusProgress += amount;
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
        public Workplace work;
        public Education educationRequired;
        public SocialClass wealthProvided;
        public Quality quality;
    }

    [Serializable]
    public class Work
    {
        public Workplace workplace;
        public Job job;
        public int maxWorkers;

        [NonSerialized]
        public List<Person> workers;

        public void AddWorker(Person person)
        {
        }

        public void RemoveWorker(Person person)
        {
        }
    }
}