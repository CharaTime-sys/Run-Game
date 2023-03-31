using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collsions_card : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            other.gameObject.SetActive(false);
        }
    }
}
