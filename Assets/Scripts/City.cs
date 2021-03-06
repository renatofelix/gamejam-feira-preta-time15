using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class City : MonoBehaviour
    {
        //Data
        public Tile[,] grid;
        public int gridWidth;
        public int gridHeight;

        public float positionMultiplier = 2.0f;
        public Transform cityTransform;

        //Government data
        public int jobSecurityMonthAmount = 0;

        //General
        [NonSerialized]
        public List<Structure> structures = new List<Structure>();

        [NonSerialized]
        public HashSet<Person> people = new HashSet<Person>();

        [NonSerialized]
        public HashSet<Person>[] singlePeople;

        [NonSerialized]
        public HashSet<Job>[] availableJobs;

        [NonSerialized]
        public HashSet<Residence>[] availableResidences;

        [NonSerialized]
        public HashSet<Structure>[] forSaleStructures;

        [NonSerialized]
        public HashSet<School>[] availableSchools;

        [NonSerialized]
        public HashSet<Hospital> availableHospitals;

        //Events
        public Action<Structure> OnAddStructure;
        public Action<Structure> OnRemoveStructure;

        public void Awake()
        {
            grid = new Tile[gridHeight, gridWidth];

            for(int i = 0; i < gridHeight; ++i)
            {
                for(int j = 0; j < gridWidth; ++j)
                {
                    grid[i, j].structure = null;
                    grid[i, j].coverages = new int[(int)Coverage.Count];
                }
            }

            singlePeople = new HashSet<Person>[(int)SocialClass.Count];

            for(int i = 0; i < (int)SocialClass.Count; ++i)
            {
                singlePeople[i] = new HashSet<Person>();
            }

            availableJobs = new HashSet<Job>[(int)Education.Count];

            for(int i = 0; i < (int)Education.Count; ++i)
            {
                availableJobs[i] = new HashSet<Job>();
            }

            availableResidences = new HashSet<Residence>[(int)SocialClass.Count];

            for(int i = 0; i < (int)Education.Count; ++i)
            {
                availableResidences[i] = new HashSet<Residence>();
            }

            forSaleStructures = new HashSet<Structure>[(int)SocialClass.Count];

            for(int i = 0; i < (int)Education.Count; ++i)
            {
                forSaleStructures[i] = new HashSet<Structure>();
            }

            availableSchools = new HashSet<School>[(int)Education.Count];

            for(int i = 0; i < (int)Education.Count; ++i)
            {
                availableSchools[i] = new HashSet<School>();
            }

            availableHospitals = new HashSet<Hospital>();
        }

        //Structures
        public void AddStructure(Structure structurePrefab, Vector2Int position, Quaternion rotation)
        {
            Structure structure = Instantiate(structurePrefab, new Vector3(position.x*positionMultiplier, 0, position.y*positionMultiplier), rotation, cityTransform);
            structure.city = this;

            grid[(gridHeight - 1) - position.y, position.x].structure = structure;

            structure.position = position;

            OnAddStructure?.Invoke(structure);
        }

        public void AddStructure(Structure structurePrefab, Vector2Int position)
        {
            AddStructure(structurePrefab, position, Quaternion.identity);
        }

        public void RemoveStructure(Structure structure)
        {
            if(structure.canBeDestroyed)
            {
                grid[(gridHeight - 1) - structure.position.y, structure.position.x].structure = null;

                OnRemoveStructure?.Invoke(structure);

                Destroy(structure.gameObject);
            }
        }

        public ref Tile GetTile(Vector2Int position)
        {
            return ref grid[position.y, position.x];
        }

        public Structure GetStructure(Vector2Int position)
        {
            return grid[(gridHeight - 1) - position.y, position.x].structure;
        }

        //People
        public void AddPerson(int amount)
        {
        }

        public void RemovePerson(Person person)
        {
            if(person.canDie)
            {
            }
        }
    }
}