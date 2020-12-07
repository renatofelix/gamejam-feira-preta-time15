using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class School : Workplace
    {
        [Header("Shool")]
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

        public override void Start()
        {
            base.Start();

            city.availableSchools[(int)educationRequired].Add(this);
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

            List<Person> graduated = new List<Person>();

            foreach(Person student in students)
            {
                float penalty = 0.0f;

                if(student.IsSick())
                {
                    penalty += 0.2f;
                }

                if(student.job != null)
                {
                    penalty += 0.3f;
                }

                student.educationProgress += (int)((100*GetEffciencyValue())*(1.0f - penalty));

                if(student.lifeStage != LifeStage.Infant && student.educationProgress >= 1000)
                {
                    graduated.Add(student);
                }
            }

            foreach(Person student in graduated)
            {
                student.Graduate();

                Dismiss(student);
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