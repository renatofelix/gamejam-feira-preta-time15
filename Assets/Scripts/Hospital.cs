using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Hospital : Workplace
    {
        [NonSerialized]
        public HashSet<Person> patients = new HashSet<Person>();

        public override void Tick()
        {
            //TODO: Treat patients

            base.Tick();
        }

        public void AdimitPatient(Person person)
        {
        }

        public void DischargePatient(Person person)
        {
        }
    }
}