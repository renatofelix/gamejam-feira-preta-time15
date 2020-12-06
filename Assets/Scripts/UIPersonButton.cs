using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class UIPersonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
    {
        [NonSerialized]
        public Person person;

        public void Start()
        {
            if(person != null)
            {
                SetupButton(person);
            }
        }

        public void SetupButton(Person person)
        {
            Image image = GetComponent<Image>();
            image.sprite = GameMenu.instance.GetPersonIcon(person);
            image.color = GameMenu.instance.raceIconColor[(int)person.colorOrRace];
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GameMenu.instance.ShowPersonInfo(person);
        }
    }
}