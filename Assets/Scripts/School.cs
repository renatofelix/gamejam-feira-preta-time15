using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class School : Workplace
    {
        public int maxStudents;

        public Education educationRequired;

        public float[] temporaryHappinessBonusBasedOnEducation =
        {
            10,
            10,
            10,
            10,
        };

        public float[] permanentHappinessBonusBasedOnEducation =
        {
            10,
            10,
            10,
            10,
        };

        [NonSerialized]
        public HashSet<Person> students = new HashSet<Person>();

        public override void Awake()
        {
            city.availableSchools[(int)educationRequired].Add(this);

            base.Awake();
        }

        public override void Destroy()
        {
            foreach(Person person in students)
            {
                Dismiss(person);
            }

            base.Destroy();
        }

        public override void Tick()
        {
            base.Tick();

            foreach(Person student in students)
            {
                if(student.sicknessProgress <= 0)
                {
                    
                }
            }
        }

        public void Enroll(Person person)
        {
            if(person.school != null)
            {
                person.school.Dismiss(person);
            }

            students.Add(person);

            person.school = this;
            person.happiness += (int)(10*GetSocialClassValue());
            person.isLookingForBetterPlace = false;

            if(maxStudents - students.Count <= 0)
            {
                city.availableSchools[(int)educationRequired].Remove(this);
            }
        }

        public void Dismiss(Person person)
        {
            students.Remove(person);

            person.school = null;
            person.happiness -= (int)(10*GetSocialClassValue());
            person.isLookingForBetterPlace = false;

            if(!city.availableSchools[(int)educationRequired].Contains(this))
            {
                city.availableSchools[(int)educationRequired].Add(this);
            }
        }
    }
}