using System.Collections.Generic;
using UnityEngine;

namespace DumontStudios.General
{
    public class Resource
    {
        public UnityEngine.Object obj;
        public uint instanceCounter = 0;
    }

    public class ResourceManager
    {
        public static Dictionary<string, Resource> resources = new Dictionary<string, Resource>();

        public static UnityEngine.Object Instantiate(string resourceName, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null)
        {
            Object instance = MonoBehaviour.Instantiate(LoadAndIncreaseCounter(resourceName), position, rotation, parent);
            instance.name = resourceName;

            return instance;
        }

        public static T Instantiate<T>(string resourceName)
        {
            GameObject instance = (GameObject)MonoBehaviour.Instantiate(LoadAndIncreaseCounter(resourceName));
            instance.name = resourceName;

            return instance.GetComponent<T>();
        }

        public static T Instantiate<T>(string resourceName, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null)
        {
            GameObject instance = (GameObject)MonoBehaviour.Instantiate(LoadAndIncreaseCounter(resourceName), position, rotation, parent);
            instance.name = resourceName;

            return instance.GetComponent<T>();
        }

        public static void Destroy(GameObject obj, bool unloadResourceIfZeroInstances = true)
        {
            if(!obj)
            {
                return;
            }

            string instanceName = obj.name.Substring(0, obj.name.Length - 8);

            if(resources.ContainsKey(instanceName))
            {
                Resource resource = resources[instanceName];

                resource.instanceCounter--;

                if(resource.instanceCounter <= 0 && unloadResourceIfZeroInstances)
                {
                    Resources.UnloadUnusedAssets();
                    //Resources.UnloadAsset(resource.obj);
                    resources.Remove(instanceName);
                }
            }

            MonoBehaviour.Destroy(obj);
        }

        public static UnityEngine.Object Load(string resourceName)
        {
            UnityEngine.Object obj;

            if(!resources.ContainsKey(resourceName))
            {
                obj = Resources.Load(resourceName);
                resources.Add(resourceName, new Resource() { obj = obj });
            }
            else
            {
                obj = resources[resourceName].obj;
            }

            if(obj == null)
            {
                RuntimeConsole.LogError("The Resoruce '" + resourceName + "' does not exist.", alsoLogOnUnity: true);
            }

            return obj;
        }

        private static UnityEngine.Object LoadAndIncreaseCounter(string resourceName)
        {
            UnityEngine.Object obj;

            if(!resources.ContainsKey(resourceName))
            {
                obj = Resources.Load(resourceName);
                resources.Add(resourceName, new Resource() { obj = obj, instanceCounter = 1 });
            }
            else
            {
                Resource resource = resources[resourceName];
                resource.instanceCounter++;

                obj = resource.obj;
            }

            if(obj == null)
            {
                RuntimeConsole.LogError("The Resource '" + resourceName + "' does not exist.", alsoLogOnUnity : true);
            }

            return obj;
        }
    }
}
