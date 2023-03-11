using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Camera_Type
{
    Side,
    Back,
    Forward,
}
public class Camera_Controller : MonoBehaviour
{
    [Header("镜头类型")]
    [SerializeField] Camera_Type cameraType;
    [Header("侧面镜头")]
    [SerializeField] Vector3 side_camera_pos;
    [SerializeField] Vector3 side_camera_rotate;
    [Header("后面镜头")]
    [SerializeField] Vector3 back_camera_pos;
    [SerializeField] Vector3 back_camera_rotate;
    public Camera main_camera;
    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        switch (cameraType)
        {
            case Camera_Type.Side:
                transform.position = side_camera_pos;
                transform.eulerAngles = side_camera_rotate;
                break;
            case Camera_Type.Back:
                transform.position = back_camera_pos;
                transform.eulerAngles = back_camera_rotate;
                break;
            case Camera_Type.Forward:
                break;
            default:
                break;
        }
        //main_camera.aspect = 0.45f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
