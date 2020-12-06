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
        public static float[] efficiencyTable =
        {
            0.2f,
            0.5f,
            1.0f,
            1.5f,
        };

        // public SocialClass targetConsumer;
        
        // public int effectiveArea;

        [Header("Workplace")]
        public List<Job> work;

        public int[] budgetEffectOnEfficiency =
        {
            0,
            1,
            2,
        };

        [NonSerialized]
        public Efficiency efficiency = Efficiency.Low;//Depends on happiness and other status of all workers and if the target

        [NonSerialized]
        public Dictionary<string, Job> workBranches = new Dictionary<string, Job>();

        [NonSerialized]
        public int totalWorkers = 0;

        [NonSerialized]
        public int currentWorkers = 0;

        public override void Awake()
        {
            base.Awake();

            for(int i = 0; i < work.Count; ++i)
            {
                Job job = work[i];

                job.workers = new HashSet<Person>();

                workBranches.Add(job.name, job);

                city.availableJobs[(int)socialClass].Add(job);

                totalWorkers += job.maxWorkers;
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

        public void Hire(Job job, Person person)
        {
            if(person.job != null)
            {
                person.job.workplace.Fire(person);
            }

            job.workers.Add(person);

            person.job = job;
            person.happiness += (int)(10*(float)socialClass*1.5f);
            person.isLookingForBetterJob = false;

            person.ChangeSocialClass(job.socialClassProvided);

            if(job.maxWorkers - job.workers.Count <= 0)
            {
                city.availableJobs[(int)job.educationRequired].Remove(job);
            }

            ++currentWorkers;
        }

        public void Fire(Person person)
        {
            Job job = person.job;

            person.job.workers.Remove(person);

            person.job = null;
            person.happiness -= (int)(10*(float)socialClass*1.5f);
            person.isLookingForBetterJob = false;
            person.jobSecurityMonthsRemaining = city.jobSecurityMonthAmount;

            // person.ChangeSocialClass(job.);

            if(!city.availableJobs[(int)job.educationRequired].Contains(job))
            {
                city.availableJobs[(int)job.educationRequired].Add(job);
            }

            --currentWorkers;
        }

        public override void Tick()
        {
            base.Tick();

            CalculateEfficiency();
        }

        public override void OnChangeBudget(Budget oldBudget, Budget newBudget)
        {
            base.OnChangeBudget(oldBudget, newBudget);

            CalculateEfficiency();
        }

        public void CalculateEfficiency()
        {
            Efficiency newEfficiency = 0;

            newEfficiency += budgetEffectOnEfficiency[(int)budget];

            if(currentWorkers > totalWorkers*0.7f)
            {
                newEfficiency += 2;
            }
            else if(currentWorkers > 0)
            {
                newEfficiency  += 1;
            }

            Efficiency oldEfficiency = efficiency;

            efficiency = newEfficiency;

            OnChangeEfficiency(oldEfficiency, newEfficiency);
        }

        public float GetEffciencyValue()
        {
            return efficiencyTable[(int)efficiency];
        }

        public virtual void OnChangeEfficiency(Efficiency oldEfficiency, Efficiency newEfficiency)
        {
        }
    }
}