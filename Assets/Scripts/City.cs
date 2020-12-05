using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class City : MonoBehaviour
    {
        //Data
        public Structure[,] grid;
        public int gridWidth;
        public int gridHeight;

        public List<Structure> structures;
        public Dictionary<int, Person> people;

        //General
        [NonSerialized]
        public int nextPersonId = 0;

        [NonSerialized]
        public HashSet<Job>[] availableJobs;

        [NonSerialized]
        public HashSet<Residence>[] availableResidences;

        [NonSerialized]
        public HashSet<Structure>[] forSaleStructures;

        //Events
        public Action<Structure> OnAddStructure;
        public Action<Structure> OnRemoveStructure;

        public void Awake()
        {
            grid = new Structure[gridWidth, gridHeight];

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
        }

        //Structures
        public void AddStructure(Structure structurePrefab, Vector2Int position)
        {
            Structure structure = Instantiate(structurePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);

            grid[position.x, position.y] = structure;

            structure.position = position;

            OnAddStructure?.Invoke(structure);
        }

        public void RemoveStructure(Structure structure)
        {
            if(structure.canBeDestroyed)
            {
                grid[structure.position.x, structure.position.y] = null;

                OnRemoveStructure?.Invoke(structure);

                Destroy(structure.gameObject);
            }
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