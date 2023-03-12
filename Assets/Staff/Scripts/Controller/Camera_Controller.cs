using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    [Header("转换的镜头")]
    [SerializeField] Vector3[] target_pos;
    [SerializeField] Vector3[] target_rotate;
    public Camera main_camera;
    public GameObject curve_parent;
    [SerializeField] float change_time;//移动速度
    Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
        Set_Camera_Type();
        offset = new Vector2(Mathf.Abs(transform.position.x - target_pos[0].x), Mathf.Abs(transform.position.y - target_pos[1].y));
    }

    [ContextMenu("改变摄像机状态")]
    private void Set_Camera_Type()
    {
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
    }

    /// <summary>
    /// 改变状态
    /// </summary>
    /// <param name="index"></param>
    public void Change_Camera_Status(bool is_resumed,int index)
    {
        if (is_resumed)
        {
            transform.DOMoveY(back_camera_pos.y, change_time);
            curve_parent.transform.DOMoveY(offset.y+ curve_parent.transform.position.y, change_time);
            transform.DORotate(back_camera_rotate, change_time);
        }
        else
        {
            if (index == 1 || index == -1)
            {
                transform.DOMoveX(transform.position.x + target_pos[1].x * index, change_time);
                curve_parent.transform.DOMoveX(curve_parent.transform.position.x - offset.x, change_time);
            }
            else
            {
                transform.DOMoveY(target_pos[index].y, change_time);
                curve_parent.transform.DOMoveY(-offset.y + curve_parent.transform.position.y, change_time);
                transform.DORotate(target_rotate[index], change_time);
            }
        }
    }
}
