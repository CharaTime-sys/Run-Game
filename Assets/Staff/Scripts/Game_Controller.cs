using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance;
    [Header("地板移动速度")]
    public Material floor_material;
    public float speed;
    public float tile_speed;
    private float offset_y;
    public GameObject[] blocks;
    public GameObject[] downs;
    [Header("障碍物生成的位置参数")]
    public Vector3 block_pos;
    [Header("每个障碍物的间隔距离（暂时为定值）")]
    public float distance;
    [Header("障碍物开始出现的距离")]
    public float start_pos = -4f;
    [Header("控制手柄")]
    public DynamicJoystick joystick;
    //左右运动的量
    //private float horizontal_move = 0f;
    //private float vertical_move = 0f;

    //手指按下和抬起的坐标
    public List<Vector2> finger_start_pos;
    [Header("手指判定半径")]
    public float finger_radious;
    public Vector2 test_vector;
    [Header("人物")]
    public Ninja ninja;
    public LineRenderer line;
    [Header("判定线")]
    [SerializeField] RectTransform check_line;
    [Header("判定线区间")]
    [SerializeField] float line_range;

    //UI相关
    [SerializeField] Text grade_text;
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //得到水平和竖直的分量
        //horizontal_move = joystick.Horizontal;
        //vertical_move = joystick.Vertical;
    }
    private void FixedUpdate()
    {
        offset_y += tile_speed * Time.deltaTime;
        floor_material.mainTextureOffset = new Vector2(0, offset_y);
    }
    public void Test_Direction()
    {
        //得到具体的角度，顺时针
        float angle = Mathf.Atan(test_vector.y/test_vector.x) * Mathf.Rad2Deg;
        if (test_vector.x<0f && test_vector.y>0f)
        {
            angle = 90f+Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }
        else if (test_vector.x < 0f && test_vector.y < 0f)
        {
            angle = 180f+Mathf.Atan(test_vector.y / test_vector.x) * Mathf.Rad2Deg;
        }
        else if (test_vector.x > 0f && test_vector.y < 0f)
        {
            angle = 270f + Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }

        //根据角度调用不同方法
        if (angle < 45 || angle > 315)
        {
            ninja.Move_Left_And_Right(1);
        }
        else if (angle >135 && angle < 225)
        {
            ninja.Move_Left_And_Right(-1);
        }
        else if (angle > 45 && angle < 135)
        {
            ninja.Jump();
        }
        else if (angle > 225 && angle < 315)
        {
            ninja.Down();
        }
    }

    public void Test_Finger()
    {
        if (line.positionCount == 0)
        {
            return;
        }
        Vector2 line_screen_pos = Camera.main.WorldToScreenPoint(line.GetPosition(line.positionCount - 1));
        if (finger_start_pos.Count == 0)
        {
            Debug.Log("减分！！");
            return;
        }
        //Debug.Log("线段位置"+line_screen_pos);
        //Debug.Log("手指位置"+ Input.touches[0].position);
        float distance = Mathf.Sqrt(Vector2.SqrMagnitude(line_screen_pos - Input.touches[0].position))/100;
        Debug.Log(distance);
        if (distance<=finger_radious)
        {
            Debug.Log("加分！！");
        }
        else
        {
            Debug.Log("减分！！");
        }
    }

    /// <summary>
    /// 判定线按下调用
    /// </summary>
    public void Test_Check_Line()
    {
        if (Mathf.Abs(finger_start_pos[0].y - check_line.position.y) <=line_range)
        {
            grade_text.text = "判定成功";
        }
        else
        {
            grade_text.text = "判定失败";
        }
    }
}
