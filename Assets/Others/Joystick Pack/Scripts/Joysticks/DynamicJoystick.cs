using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public static DynamicJoystick Instance;
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

    //是否判定上方
    public bool if_top;
    [Header("上下的偏移值")]
    [SerializeField] float offset_y = 10f;
    public int touch_index = 0;//手指索引
    //判定线委托
    public delegate bool Set_check_line(int index);
    public Set_check_line set_Check_Line;
    //手指松开委托
    public delegate void Disable_check_line();
    public Disable_check_line disable_Check_Line;
    bool Test_CheckLine;//测试手指在哪里判断的
    private void Awake()
    {
        Instance = this;
    }
    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    /// <summary>
    /// 鼠标按下
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        //让手指的位置加入gamecontroller进入判定
        if (Input.touches.Length != 0)
        {
            foreach (var touch in Input.touches)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // 用射线检测是否与粒子发生碰撞
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Finger")))
                {
                    if (hit.collider.gameObject.name.Contains("Smoke"))
                    {
                        return;
                    }
                }
            }
            int index = 0;
            //对每个位置进行循环
            foreach (var item in Input.touches)
            {
                if (set_Check_Line!=null && set_Check_Line.Invoke(touch_index))
                {
                    Test_CheckLine = true;
                }//设置每个手势的手指位置坐标
                //加入手指的开始坐标
                Game_Controller.Instance.finger_start_pos = Input.touches[touch_index].position;
                Game_Controller.Instance.pressed = true;
                if (Game_Controller.Instance.ninja.Is_buffing)
                {
                    if (Game_Controller.Instance.buff_index==-1)
                    {
                        Game_Controller.Instance.buff_index = index;
                    }
                    Game_Controller.Instance.pressed_once = true;
                }
                Game_Controller.Instance.Press_Checked(true);
                //设置按下状态
                Game_Controller.Instance.Check_Down_And_Jump();
                index++;
            }
        }
        touch_index++;//增加索引
        base.OnPointerDown(eventData);
    }
    /// <summary>
    /// 鼠标抬起
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        foreach (var touch in Input.touches)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            // 用射线检测是否与粒子发生碰撞
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Finger")))
            {
                if (hit.collider.gameObject.name.Contains("Smoke"))
                {
                    return;
                }
            }
        }
        base.OnPointerUp(eventData);
        Dir_Type dir_Type = Dir_Type.Up;
        background.gameObject.SetActive(false);
        touch_index--;//减少索引
        disable_Check_Line?.Invoke();//手指松开判断
        //是否是手势
        bool if_monster = false;
        //得到手势的方向，手指坐标不为空 检测在下面 手指坐标不为0
        if (Input.touches.Length != 0)
        {
            Game_Controller.Instance.test_vector = Input.touches[touch_index].position - Game_Controller.Instance.finger_start_pos;
            //取消按下状态
            if (!Game_Controller.Instance.is_buffing || touch_index == Game_Controller.Instance.buff_index)
            {
                Game_Controller.Instance.Press_Checked(false);
                Game_Controller.Instance.pressed = false;
                Game_Controller.Instance.buff_index = -1;//设置buff索引
            }
            //移除原来的坐标
            Game_Controller.Instance.finger_start_pos = new Vector2(-1000, 1000);
            if (Test_CheckLine || !Game_Controller.Instance.game_started || !Game_Controller.Instance.game_started_gesture)
            {
                if (!Game_Controller.Instance.game_started_gesture)
                {
                    Game_Controller.Instance.game_started_gesture = true;
                }
                Test_CheckLine = false;
                return;
            }

            //防止有bug
            dir_Type = Game_Controller.Instance.Test_Direction();
            //进行评分
            foreach (Transform item in Block_Controller.Instance.block_parent.transform)
            {
                if (item.GetComponent<Block>().Test_Score(dir_Type, Input.touches[touch_index].position - Game_Controller.Instance.test_vector))
                {
                    if_monster = true;
                    break;
                }
            }
            if (!if_monster && !Game_Controller.Instance.is_jump_after && !Game_Controller.Instance.is_buffing)
            {
                Game_Controller.Instance.Change_Character(dir_Type);
            }
            //重置长按跳跃的状态
            Game_Controller.Instance.is_jump_after = false;
        }
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}