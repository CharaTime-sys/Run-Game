using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

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
        if (Input.touches.Length!=0)
        {
            //加入手指的开始坐标
            Game_Controller.Instance.finger_start_pos.Add(Input.touches[0].position);
            //设置按下状态
            Game_Controller.Instance.pressed = true;
            if (Game_Controller.Instance.ninja.Is_buffing)
            {
                Game_Controller.Instance.Press_Checked(true);
                Game_Controller.Instance.Check_Down_And_Jump();
            }
            //测试上方的判定线
            //Game_Controller.Instance.Test_Check_Line();
        }
        base.OnPointerDown(eventData);
    }

    /// <summary>
    /// 鼠标抬起
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        //得到手势的方向
        if (Input.touches.Length != 0)
        {
            Game_Controller.Instance.test_vector = Input.touches[0].position - Game_Controller.Instance.finger_start_pos[0];
            //取消按下状态
            Game_Controller.Instance.Press_Checked(false);
            Game_Controller.Instance.pressed = false;
        }
        //移除原来的坐标
        Game_Controller.Instance.finger_start_pos.RemoveAt(0);
        //防止有bug
        if (!Game_Controller.Instance.is_jump_after)
        {
            Game_Controller.Instance.Test_Direction();
        }
        switch (Game_Controller.Instance.Buff_Type)
        {
            case Buff_Type.Jump:
                Game_Controller.Instance.Set_Jump_UI(false);
                break;
            case Buff_Type.Down:
                Game_Controller.Instance.Set_Down_UI(false);
                break;
            default:
                break;
        }
        //重置长按跳跃的状态
        Game_Controller.Instance.is_jump_after = false;
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