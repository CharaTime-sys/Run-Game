using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve_Follow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Curve_Point());
    }

    public Vector2 Curve_Point()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
}
