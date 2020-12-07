using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class UIStructureButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        public Structure structure;

        public void OnPointerEnter(PointerEventData eventData)
        {
            GameMenu.instance.ShowTooltip(Input.mousePosition, structure.displayName, structure.cost.ToString(), structure.description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameMenu.instance.HideTooltip();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(GameManager.instance.money >= structure.cost)
            {
                GridManager._instance.SetStructureToBuild(structure);
            }
        }
    }
}