using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Camera main_camera;
    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;

        main_camera.aspect = 0.45f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
