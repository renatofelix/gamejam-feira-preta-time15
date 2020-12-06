using System;
using System.Collections.Generic;
using System.Linq;
using DumontStudios.General;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game
{
    [DefaultExecutionOrder(-1000)]
    public class GameManager : MonoBehaviour
    {
        public static string[] months =
        {
            "Janeiro",
            "Fevereiro",
            "Março",
            "Abril",
            "Maio",
            "Junho",
            "Julho",
            "Agosto",
            "Setembro",
            "Outubro",
            "Novembro",
            "Dezembro",
        };

        public static GameManager instance;

        //Game
        public float time;
        public int tick = 0;
        public int month = 0;
        public int trimester = 0;
        public int year = 2020;

        public int ticksPerMonth = 2;
        public float trimesterDuration = 120;

        public int budget;
        public int politicalPoints;

        public float[] findRelationshipChance = new float[(int)SocialClass.Count];
        public float[] procriateChance = new float[(int)SocialClass.Count];
        public float[] sicknessChance = new float[(int)SocialClass.Count];
        public float[] untreatedSicknessKillChance = new float[(int)SocialClass.Count];

        [NonSerialized]
        public City city;

        //Events
        public Action<GameObject> onDestroyEvent;

        public void Awake()
        {
            if(instance)
            {
                Destroy(instance);
            }

            instance = this;

            Table table = TableHandler.Load("Data");

            city = GetComponent<City>();
        }

        public void Start()
        {
        }

        public void Destroy()
        {
            onDestroyEvent = null;
        }

        public void Update()
        {
            time += Time.deltaTime;

            if(time >= (trimesterDuration/3.0f)/ticksPerMonth)
            {
                time -= (trimesterDuration/3.0f)/ticksPerMonth;

                ++tick;

                OnUpdateTick();

                if(tick >= ticksPerMonth)
                {
                    tick = 0;

                    ++month;

                    OnUpdateMonth();

                    if(month >= 2)
                    {
                        ++trimester;

                        OnUpdateTrimester();
                    }
                    
                    if(month >= months.Length)
                    {
                        month = 0;

                        ++year;

                        OnUpdateYear();
                    }
                }
            }
        }

        public void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
        }

        public void OnEntityDestroy(GameObject entity)
        {
            onDestroyEvent?.Invoke(entity);
        }

        //Time
        public void OnUpdateTick()
        {
            UpdatePeople();
        }

        public void OnUpdateMonth()
        {
        }

        public void OnUpdateTrimester()
        {
        }

        public void OnUpdateYear()
        {
            if(year == 4)
            {
                //TODO: End game
            }
        }

        //Structure Management

        //People Management
        public void UpdatePeople()
        {
            foreach(Person person in city.people)
            {
                if(!person.IsSick() && Random.Range(0f, 100f) <= sicknessChance[(int)person.GetSocialClass()])
                {
                    person.AddSickness(Random.Range(800, 1200));
                }

                if(person.lifeStage == LifeStage.Infant)
                {
                    if(person.IsSick())
                    {
                        SearchForHospital(person);
                    }
                }
                else if(person.lifeStage == LifeStage.Youth)
                {
                    if(person.IsSick())
                    {
                        SearchForHospital(person);
                    }
                }
                else if(person.lifeStage == LifeStage.Adult || person.lifeStage == LifeStage.Senior)
                {
                    if(person.residence == null || person.isLookingForBetterPlace)
                    {
                        List<Person> family = new List<Person>();

                        family.Add(person);

                        if(person.relationshipPartner != null && person.relationshipPartner.residence == person.residence)
                        {
                            family.Add(person.relationshipPartner);
                        }

                        for(int childIndex = 0; childIndex < person.children.Count; ++childIndex)
                        {
                            Person child = person.children[childIndex];

                            if(child.lifeStage == LifeStage.Infant || child.lifeStage == LifeStage.Youth)
                            {
                                family.Add(child);
                            }
                        }

                        Residence rentedResidence = null;

                        for(int currentSocialClass = (int)person.GetSocialClass(); currentSocialClass >= 0 && rentedResidence == null; --currentSocialClass)
                        {
                            HashSet<Residence> residenceCollection = city.availableResidences[(int)currentSocialClass];

                            foreach(Residence residence in residenceCollection)
                            {
                                if(residence.maxResidents - residence.residents.Count >= family.Count)
                                {
                                    rentedResidence = residence;

                                    break;
                                }
                            }
                        }

                        if(rentedResidence != null)
                        {
                            foreach(Person member in family)
                            {
                                rentedResidence.RentFor(member);
                            }
                        }
                    }

                    if(!person.IsSick())
                    {
                        if(person.job == null || (int)person.job.educationRequired < (int)person.education || person.isLookingForBetterJob)
                        {
                            for(int currentEducation = (int)person.education; currentEducation >= 0; --currentEducation)
                            {
                                HashSet<Job> jobCollection = city.availableJobs[currentEducation];

                                if(jobCollection.Count > 0)
                                {
                                    Job job = jobCollection.First();

                                    job.workplace.Hire(job, person);

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        SearchForHospital(person);
                    }
                }
            }
        }

        public void SearchForSchool(Person person)
        {
            if(person.school = null)
            {
                // if(
            }
        }

        public void SearchForHospital(Person person)
        {
            if(person.hospital == null)
            {
                if(city.availableHospitals.Count > 0)
                {
                    Hospital hospital = city.availableHospitals.First();

                    hospital.AdimitPatient(person);
                }
            }
        }

        //
    }
}