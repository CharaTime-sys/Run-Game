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
    [Header("判定线位置")]
    [SerializeField] Transform checkline;
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
        //找到checkline
        checkline = GameObject.Find("游戏必备/屏幕画布/判定手势相关/CheckLine").GetComponent<Transform>();
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
            //对每个位置进行循环
            foreach (var item in Input.touches)
            {
                if (set_Check_Line.Invoke(touch_index))
                {
                    Test_CheckLine = true;
                }//设置每个手势的手指位置坐标
                //加入手指的开始坐标
                Game_Controller.Instance.finger_start_pos = Input.touches[touch_index].position;
                Game_Controller.Instance.pressed = true;
                Game_Controller.Instance.Press_Checked(true);
                //设置按下状态
                Game_Controller.Instance.Check_Down_And_Jump();
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
        Dir_Type dir_Type = Dir_Type.Up;
        background.gameObject.SetActive(false);
        touch_index--;//减少索引
        //得到手势的方向，手指坐标不为空 检测在下面 手指坐标不为0
        if (Input.touches.Length != 0  && Game_Controller.Instance.finger_start_pos != new Vector2(-1000,1000))
        {
            Game_Controller.Instance.test_vector = Input.touches[touch_index].position - Game_Controller.Instance.finger_start_pos;
            //取消按下状态
            Game_Controller.Instance.Press_Checked(false);
            Game_Controller.Instance.pressed = false;
            //移除原来的坐标
            Game_Controller.Instance.finger_start_pos = new Vector2(-1000, 1000);

            if (Test_CheckLine || !Game_Controller.Instance.game_started)
            {
                Test_CheckLine = false;
                return;
            }
            //防止有bug
            if (!Game_Controller.Instance.is_jump_after)
            {
                dir_Type = Game_Controller.Instance.Test_Direction();
            }
            if (Game_Controller.Instance.ninja.Is_buffing)
            {
                switch (Game_Controller.Instance.Buff_Type)
                {
                    case Buff_Type.Jump:
                        Game_Controller.Instance.Set_Jump_UI(false);
                        if (Game_Controller.Instance.is_reached)
                        {
                            Game_Controller.Instance.ninja.Resume_Jump();
                        }
                        break;
                    case Buff_Type.Down:
                        Game_Controller.Instance.Set_Down_UI(false);
                        if (Game_Controller.Instance.is_reached)
                        {
                            Game_Controller.Instance.ninja.Resume_Down();
                        }
                        break;
                    default:
                        break;
                }
            }
            //重置长按跳跃的状态
            Game_Controller.Instance.is_jump_after = false;
            //进行评分
            if (Block_Controller.Instance.cur_block != null && !Game_Controller.Instance.ninja.Is_buffing)
            {
                Block_Controller.Instance.cur_block.Test_Score(dir_Type);
            }
        }
        disable_Check_Line?.Invoke();//手指松开判断
        base.OnPointerUp(eventData);
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