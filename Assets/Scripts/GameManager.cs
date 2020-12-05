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
        public static GameManager instance;

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
        }

        public void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
        }

        public void OnEntityDestroy(GameObject entity)
        {
            onDestroyEvent?.Invoke(entity);
        }
    }
}
