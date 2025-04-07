using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    public class DestroySelf : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(DestroyThis());
        }

        IEnumerator DestroyThis()
        {
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }
    }
}