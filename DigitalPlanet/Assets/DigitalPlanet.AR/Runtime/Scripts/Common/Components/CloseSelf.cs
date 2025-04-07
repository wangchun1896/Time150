using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    public class CloseSelf : MonoBehaviour
    {
        public float time = 1.5f;
        void OnEnable()
        {
            StartCoroutine(DestroyThis());
        }

        IEnumerator DestroyThis()
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(false);
        }
    }
}