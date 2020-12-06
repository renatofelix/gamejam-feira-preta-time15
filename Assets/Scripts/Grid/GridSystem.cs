using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game {
    public class GridSystem : MonoBehaviour
    {

        [SerializeField] private int rows;
        [SerializeField] private int cols;


        public static GridSystem _instance;
        public static GridSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GridSystem>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GridSystem");
                        _instance = container.AddComponent<GridSystem>();
                    }
                }

                return _instance;
            }
        }

        public int GetRows()
        {
            return rows;
        }
        public int GetCols()
        {
            return cols;
        }

        private TileInformation[][] tileInformation;

        struct TileInformation {

            public int typeOfTile;
            public Structure structure;

        }

        private TileInformation GetTileInformation(Vector2Int index)
        {
            return tileInformation[index.x][index.y];
        }

        


}
}


