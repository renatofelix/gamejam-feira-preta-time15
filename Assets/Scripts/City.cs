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

        public float positionMultiplier = 2.0f;
        public Transform cityTransform;

        [NonSerialized]
        public List<Structure> structures = new List<Structure>();

        [NonSerialized]
        public HashSet<Person> people = new HashSet<Person>();

        //General
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

            availableSchools = new HashSet<School>[(int)Education.Count];

            for(int i = 0; i < (int)Education.Count; ++i)
            {
                availableSchools[i] = new HashSet<School>();
            }

            availableHospitals = new HashSet<Hospital>();
        }

        //Structures
        public void AddStructure(Structure structurePrefab, Vector2Int position)
        {
            Structure structure = Instantiate(structurePrefab, new Vector3(position.x*positionMultiplier, 0, position.y*positionMultiplier), Quaternion.identity, cityTransform);

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