using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Residence : Structure
    {
        public int maxResidents;
        public List<Person> residentes;

        public override void Awake()
        {
            base.Awake();
        }

        public override void Destroy()
        {
            //TODO: Remove all workers.

            base.Destroy();
        }
    }
}