using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public enum Efficiency
    {
        Low,
        Medium,
        High,
        Over,
        Count
    }

    public class Workplace : Structure
    {
        public SocialClass targetConsumer;
        
        public int effectiveArea;

        public List<Job> work;

        [NonSerialized]
        public Efficiency efficiency = Efficiency.Low;//Depends on happiness and other status of all workers and if the target

        [NonSerialized]
        public Dictionary<string, Job> workBranches = new Dictionary<string, Job>();

        public override void Awake()
        {
            base.Awake();

            for(int i = 0; i < work.Count; ++i)
            {
                Job job = work[i];

                job.workers = new List<Person>(job.maxWorkers);

                workBranches.Add(job.name, job);

                city.availableJobs[(int)socialClass].Add(job);
            }
        }

        public override void Destroy()
        {
            foreach(Job work in work)
            {
                foreach(Person person in work.workers)
                {
                    Fire(person);
                }
            }
            
            base.Destroy();
        }

        public void Hire(Person person)
        {
            if(person.job != null)
            {
                person.job.workplace.Fire(person);
            }
        }

        public void Fire(Person person)
        {
        }

        public override void Tick()
        {


            base.Tick();
        }
    }
}