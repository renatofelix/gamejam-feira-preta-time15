using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CameraInteraction : MonoBehaviour
    {
        public static Vector2Int GridMousePostion;

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !GridManager.Instance.isContructMode())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit; 

                if (Physics.Raycast(ray, out hit))
                {
                    GridManager.Instance.SetCurrentTile(hit.transform.GetComponent<Structure>()); 
                }
            }
            else if(Input.GetMouseButtonDown(0) && GridManager.Instance.saleMode)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GridManager.Instance.SaleStructure(hit.transform.GetComponent<Structure>());
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GridMousePostion = new Vector2Int((int)(hit.transform.position.x / 2), (int)hit.transform.position.z / 2);
                }
            }
        }
    }
}
