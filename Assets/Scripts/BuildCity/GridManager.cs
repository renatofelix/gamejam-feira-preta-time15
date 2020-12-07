using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


namespace Game
{
    public class GridManager : MonoBehaviour
    {

        [SerializeField] private int rows;
        [SerializeField] private int cols;
        //[SerializeField] private TextMeshProUGUI selectDebug;
        //[SerializeField] private TextMeshProUGUI currentMouseLocation;

        [SerializeField] private GameObject GhostReference;
        [SerializeField] private GameObject CurrentObjectSelect;
        private Vector3 ghostRotation;


        [SerializeField] private GameObject objectWillBuild;
        [SerializeField] private Structure structureToBuild;

        //[SerializeField] private Material materialGhost;
        //[SerializeField] private Material materialCorrect;



        [SerializeField] private City city;


        [SerializeField] private Structure currentTile;


        public Action<Structure> OnSelectTile;
        public Action<Structure> OnDeselectTile;

        public Action<Structure> OnBuildModeOn;
        public Action<bool> OnBuildModeOff;

        public Action<bool> OnSaleMode;
        //public Action<Structure> OnSaleModeOff;


        private bool buildingMode = false;
        private bool selectMode = false;
        public bool saleMode = false;

        public static GridManager _instance;
        public static GridManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<GridManager>();

                    if (_instance == null)
                    {
                        GameObject container = new GameObject("GridSystem");
                        _instance = container.AddComponent<GridManager>();
                    }
                }
                return _instance;
            }
        }

        private void OnEnable()
        {
            
            city.OnAddStructure += StructureBuilded;
        }

        private void OnDisable()
        {
            city.OnAddStructure -= StructureBuilded;
        }

        public bool isContructMode()
        {
            return buildingMode;
        }

        private void StructureBuilded(Structure a)
        {
            if (structureToBuild != null)
            {
                buildingMode = false;
               // structureToBuild.GetComponent<MeshRenderer>().material = materialCorrect;
                structureToBuild = null;
                Destroy(CurrentObjectSelect);
            }

        }

        public void SaleStructure(Structure e)
        {
            city.RemoveStructure(e);
        }

        

        public int GetRows()
        {
            return rows;
        }
        public int GetCols()
        {
            return cols;
        }
        public void SetCurrentTile(Structure structure)
        {
            selectMode = true;
            currentTile = structure;
            OnSelectTile?.Invoke(currentTile);
            //selectDebug.text = currentTile.transform.name + " X [" + (currentTile.transform.position.x /2)+ "] Y [" + (currentTile.transform.position.z / 2)+"]" ;

        }

        public void ActivateModeSale()
        {
            saleMode = true;
            OnSaleMode?.Invoke(saleMode);
        }

        public Structure getCurrentTile()
        {
            return currentTile;
        }

        private void CreateGhostBuild()
        {
            buildingMode = true;
            GhostReference = structureToBuild.gameObject;
            CurrentObjectSelect = GameObject.Instantiate(GhostReference);

        }

        private void MovimentGhost()
        {
            CurrentObjectSelect.transform.position = new Vector3(CameraInteraction.GridMousePostion.x * 2, 0.3f, CameraInteraction.GridMousePostion.y * 2);


            if (Input.mouseScrollDelta.y > 0)
            {
                CurrentObjectSelect.transform.Rotate(0, 90, 0);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                CurrentObjectSelect.transform.Rotate(0, -90, 0);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("cliquei para adicionar o edificio");
                city.AddStructure(structureToBuild, new Vector2Int(CameraInteraction.GridMousePostion.x, CameraInteraction.GridMousePostion.y), CurrentObjectSelect.transform.rotation);
            }
        }

        public void SetStructureToBuild(Structure build)
        {
            structureToBuild = build;
            CreateGhostBuild();
        }

        private void Update()
        {
            if (buildingMode)
            {
                MovimentGhost();
            }

            //change gamemode
            GameModeManager();


            //currentMouseLocation.text = " X [" + CameraInteraction.GridMousePostion.x + "] Y [" + CameraInteraction.GridMousePostion.y + "]";
        }

        public void BuildModeOff()
        {
            buildingMode = false;
            OnBuildModeOff?.Invoke(true);
            Destroy(CurrentObjectSelect);
        }

        public void SelectModeOff()
        {
            selectMode = false;
            OnDeselectTile?.Invoke(currentTile);
            currentTile = null;
        }

        public void SaleModeOff()
        {
            saleMode = false;
        }

        private void GameModeManager()
        {
            //if (Input.GetKeyDown(KeyCode.Escape) && buildingMode)
            //{
            //    buildingMode = false;
            //    OnBuildModeOff?.Invoke(true);
            //    Destroy(CurrentObjectSelect);
            //}
            //else if (Input.GetKeyDown(KeyCode.Escape) && saleMode)
            //{
            //    saleMode = false;
            //}
            //else if (Input.GetKeyDown(KeyCode.Escape) && selectMode)
            //{
            //    selectMode = false;
            //    OnDeselectTile?.Invoke(currentTile);
            //    currentTile = null;
            //}
        }

        public void DeleteObjectButton()
        {
            city.RemoveStructure(currentTile);
            //selectDebug.text = "";
            currentTile = null;
        }
    }
}


