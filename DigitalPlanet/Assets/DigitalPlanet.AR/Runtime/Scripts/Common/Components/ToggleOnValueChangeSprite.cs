using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class ToggleOnValueChangeSprite : MonoBehaviour
    {
        public Sprite unSelectSprite;
        public Sprite selectedSprite;
        public Toggle toggle;
        public Image image;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            image = GetComponent<Image>();
        }
        void Start()
        {
            if (gameObject.name.Contains("½ºÄÒ"))
                toggle.onValueChanged.Invoke(toggle.isOn);
            OnToggleClick();
        }

        public void OnToggleClick()
        {
            if (toggle.isOn)
            {
                image.sprite = selectedSprite;
            }
            else
            {
                image.sprite = unSelectSprite;
            }

        }

    }
}