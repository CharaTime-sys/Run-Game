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

    [Header("转换的镜头(主要物体)")]
    [SerializeField] Vector3[] target_pos;
    [SerializeField] Vector3[] target_rotate;
    [Header("返回的镜头")]
    [SerializeField] Vector3 return_target_pos;
    [SerializeField] Vector3 return_target_rotate;
    public Camera main_camera;
    [SerializeField] GameObject main_staff;
    public GameObject curve_parent;
    [SerializeField] float change_time;//移动速度
    [SerializeField] float change_time_horizontal;//横向移动速度
    Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        main_camera = Camera.main;
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
    /// 改变状态,0是y轴
    /// </summary>
    /// <param 是否要回去="is_resumed"></param>
    /// <param 改变的状态索引="index"></param>
    public void Change_Camera_Status(bool is_resumed,int index)
    {
        //横向移动回去
        if (is_resumed)
        {
            main_staff.transform.DOMoveY(return_target_pos.y, change_time);
            main_staff.transform.DORotate(return_target_rotate, change_time);
        }
        else
        {
            //横向移动
            if (index == 1 || index == -1)
            {
                if (name.Contains("Three"))
                {
                    Game_Controller.Instance.ninja.transform.DOMoveX(Game_Controller.Instance.ninja.transform.position.x - target_pos[1].x * index, change_time_horizontal);
                    return;
                }
                main_staff.transform.DOMoveX(main_staff.transform.position.x + target_pos[1].x * index, change_time_horizontal);
            }
            ////纵向移动
            //else if(index == 0)
            //{
            //    main_staff.transform.DOMoveY(target_pos[index].y, change_time);
            //    main_staff.transform.DORotate(target_rotate[index], change_time);
            //}
            //else
            //{
            //    main_staff.transform.DOMoveY(target_pos[index].y, change_time);
            //}
        }
    }
    [ContextMenu("改变摄像机")]
    public void Change_Camera()
    {
        Camera.main.backgroundColor = new Color32(255, 162, 116,255);
    }
}
