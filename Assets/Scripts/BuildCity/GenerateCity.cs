using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;


namespace Game
{
    public class GenerateCity : MonoBehaviour
    {
        public Vector2Int size;


        [SerializeField] private City city;
        [SerializeField] private CityBuilds cityBuilds;

        private List<int> positionBuildText = new List<int>();
        private void Awake()
        {
            convertTextToTile();
        }
        private void Start()
        {
            GenerateMap();
        }

        private void Update()
        {
            GenerateMap();
        }



        public void GenerateMap()
        {
            int a = 0;

            string holderName = "Generated Map";

            if (transform.Find(holderName))
            {
                DestroyImmediate(transform.Find(holderName).gameObject);
            }

            Transform mapHolder = new GameObject(holderName).transform;
            mapHolder.parent = transform;

            size.x = city.gridWidth;
            size.y = city.gridHeight;

            for (int i = 0; i < size.y; i++)
            {
                for (int y = 0; y < size.x; y++)
                {
                    GameObject obj =  GameObject.Instantiate(cityBuilds.builds[positionBuildText[a]], new Vector3(i * 2, 0, y * 2), Quaternion.identity, mapHolder);
                    city.AddStructure(obj.GetComponent<Structure>(), new Vector2Int(i, y));
                    a++;
                }
            }

        }

        private void convertTextToTile()
        {
            string path = "Assets/Resources/MapDataDefault.txt";
            string[] stringNumber;
            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);

            stringNumber = reader.ReadToEnd().Split(' ', '\n');

            foreach (var item in stringNumber)
            {
                int number;
                int.TryParse(item, out number);
                positionBuildText.Add(number);
            }
            reader.Close();
        }


    }
}
