using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TimeStar.DigitalPlant
{
    public class StarWheelController : MonoBehaviour
    {
        public List<TouchAndInpuEffect3D> starWheelsList;

        public TouchAndInpuEffect3D lastSelectedTouchAndInputEffect3DGameobject;
        public bool isPlayWheel = false;

        public string currentSeletedStar;

        public void SetIsPlayWheelFalse()
        {
            isPlayWheel = false;
        }
    }
}
