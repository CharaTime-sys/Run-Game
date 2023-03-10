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

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        //让手指的位置加入gamecontroller进入判定
        if (Input.touches.Length!=0)
        {
            Game_Controller.Instance.finger_start_pos.Add(Input.touches[0].position);
            Game_Controller.Instance.Test_Check_Line();
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        //得到手势的方向
        if (Input.touches.Length != 0)
        {
            Game_Controller.Instance.test_vector = Input.touches[0].position - Game_Controller.Instance.finger_start_pos[0];
        }
        //移除原来的坐标
        Game_Controller.Instance.finger_start_pos.RemoveAt(0);
        Game_Controller.Instance.Test_Direction();
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