using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Workplace : Structure
    {
        public int effectiveArea;
        public Work[] work;

        public override void Awake()
        {
            base.Awake();

            for(int i = 0; i < work.Length; ++i)
            {
                work[i].workers = new List<Person>(work[i].maxWorkers);
            }
        }

        public override void Destroy()
        {
            //TODO: Remove all workers
            
            base.Destroy();
        }

        public void AddWorker(Person person)
        {
        }

        public void RemoveWorker(Person person)
        {
        }
    }
}