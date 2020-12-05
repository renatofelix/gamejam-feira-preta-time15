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

        public Dictionary<int, Person> people;

        //General
        public int nextId = 0;

        public void Awake()
        {
            grid = new Structure[gridWidth, gridHeight];
        }

        //People
        public void AddPerson(int amount)
        {
        }

        public void RemovePerson(Person person)
        {
        }

        //Structures
        
    }
}