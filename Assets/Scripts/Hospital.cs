using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Hospital : Workplace
    {
        [Header("Hospital")]
        public int maxPatients;

        public float[] efficiencyKillChance = 
        {
            50f,
            30f,
            15f,
            5f,
        };

        [NonSerialized]
        public HashSet<Person> patients = new HashSet<Person>();

        public override void Start()
        {
            base.Start();

            city.availableHospitals.Add(this);
        }

        public override void Destroy()
        {
            foreach(Person person in patients)
            {
                DischargePatient(person);
            }

            base.Destroy();
        }

        public override void Tick()
        {
            base.Tick();

            List<Person> dead = new List<Person>();
            List<Person> discharged = new List<Person>();

            foreach(Person patient in patients)
            {
                if(Random.Range(0f, 100f) <= efficiencyKillChance[(int)efficiency])
                {
                    dead.Add(patient);
                }
                else
                {
                    patient.sicknessProgress -= (int)(100*GetEffciencyValue());

                    if(patient.sicknessProgress <= 0)
                    {
                        discharged.Add(patient);
                    }
                }
            }

            foreach(Person patient in dead)
            {
                patient.Death();
            }

            foreach(Person patient in discharged)
            {
                patient.RemoveSickness();

                DischargePatient(patient);
            }
        }

        public void AdimitPatient(Person person)
        {
            if(person.hospital != null)
            {
                person.hospital.DischargePatient(person);
            }

            patients.Add(person);

            person.hospital = this;

            if(maxPatients - patients.Count <= 0)
            {
                city.availableHospitals.Remove(this);
            }
        }

        public void DischargePatient(Person person)
        {
            patients.Remove(person);

            person.hospital = null;

            if(!city.availableHospitals.Contains(this))
            {
                city.availableHospitals.Add(this);
            }
        }
    }
}