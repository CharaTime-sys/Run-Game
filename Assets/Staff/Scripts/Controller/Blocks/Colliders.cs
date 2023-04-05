using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.StartsWith("ground"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
