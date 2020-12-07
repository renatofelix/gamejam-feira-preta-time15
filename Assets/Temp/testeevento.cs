using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

     
    public class testeevento : MonoBehaviour
    {

        private void OnEnable()
        {
            GridManager.Instance.OnSelectTile += TESTESTESTE;
        }

        private void OnDisable()
        {
            GridManager.Instance.OnSelectTile -= TESTESTESTE;
        }

        public void TESTESTESTE(Structure e)
        {
            Debug.Log("ALGO FOI SELECIONADO");
        }
    }
}
