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
        public float[] sicknessKillChance = new float[(int)SocialClass.Count];

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

        //Structure Management

        //People Management
        public void UpdatePeople()
        {
            for(int i = 0; i < city.people.Count; ++i)
            {
                Person person = city.people[i];

                if(!person.isSick && Random.Range(0f, 100f) <= sicknessChance[(int)person.GetSocialClass()])
                {
                    person.ChangeSicknessState(true);
                }

                if(person.lifeStage == LifeStage.Infant)
                {
                }
                else if(person.lifeStage == LifeStage.Youth)
                {
                }
                else if(person.lifeStage == LifeStage.Adult || person.lifeStage == LifeStage.Senior)
                {
                    if(person.residence == null || person.isLookingForBetterPlace)
                    {
                        List<Person> family = new List<Person>();

                        family.Add(person);

                        for(int childIndex = 0; childIndex < person.children.Count; ++childIndex)
                        {
                            Person child = person.children[childIndex];

                            if(child.lifeStage == LifeStage.Infant || child.lifeStage == LifeStage.Youth)
                            {
                                family.Add(child);
                            }
                        }

                        bool rented = false;

                        for(int currentSocialClass = (int)person.GetSocialClass(); currentSocialClass >= 0 && !rented; --currentSocialClass)
                        {
                            HashSet<Residence> residenceCollection = city.availableResidences[(int)currentSocialClass];

                            foreach(Residence residence in residenceCollection)
                            {
                                if(residence.maxResidents - residence.residents.Count >= family.Count)
                                {
                                    for(int familyMemberIndex = 0; familyMemberIndex < family.Count; ++familyMemberIndex)
                                    {
                                        Person familyMember = family[familyMemberIndex];

                                        residence.RentFor(family[familyMemberIndex]);
                                    }

                                    rented = true;

                                    break;
                                }
                            }
                        }
                    }

                    if(!person.isSick)
                    {
                        if(person.job == null || (int)person.job.educationRequired < (int)person.education || person.isLookingForBetterJob)
                        {
                            for(int currentEducation = (int)person.education; currentEducation >= 0; --i)
                            {
                                HashSet<Job> jobCollection = city.availableJobs[currentEducation];

                                if(jobCollection.Count > 0)
                                {
                                    Job job = jobCollection.First();

                                    job.workplace.Hire(person);

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //TODO: try to find hospital
                    }
                }
            }
        }

        //Time
        public void OnUpdateTick()
        {
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
    }
}