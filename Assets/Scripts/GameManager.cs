using System;
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

        public int ticksPerMonth = 4;
        public float trimesterDuration = 120;

        public int budget;
        public int politicalPoints;

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