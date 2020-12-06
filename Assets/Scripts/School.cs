using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class School : Workplace
    {
        public int maxStudents;

        public Education educationRequired;

        public int[] temporaryHappinessBonusBasedOnEducation =
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
                if(!student.IsSick())
                {
                    student.educationProgress += (int)(100*GetEffciencyValue());

                    if(student.educationProgress >= 1000)
                    {
                        student.Graduate();
                    }
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
            person.happiness += temporaryHappinessBonusBasedOnEducation[(int)educationRequired];
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
            person.happiness -= temporaryHappinessBonusBasedOnEducation[(int)educationRequired];
            person.isLookingForBetterPlace = false;

            if(!city.availableSchools[(int)educationRequired].Contains(this))
            {
                city.availableSchools[(int)educationRequired].Add(this);
            }
        }
    }
}