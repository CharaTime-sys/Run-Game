using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.EventSystems;

public class Curve_Block : Block
{
    //这个时候停止运动
    [SerializeField] float end_z_min = 5.6f;
    [SerializeField] float end_z_max = 6.4f;
    [SerializeField] SplineRenderer splineComputer;
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] int touch_index;//目前的手指位置

    bool is_over;//是否超过了判定范围
    bool is_once;//是否第一次按下
    bool is_add_once;//是否第一次加入事件
    bool is_pressed;//是否按下了
    private void FixedUpdate()
    {
        ////如果超过了就不会前进
        if (!is_pressed && !is_over)
        {
            base.FixedUpdate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        ////当进入判断范围内
        if (transform.position.z < end_z_max)
        {
            if (!is_add_once)
            {
                DynamicJoystick.Instance.set_Check_Line += Set_Pressed;
                DynamicJoystick.Instance.disable_Check_Line += Disable_Pressed;
                is_add_once = true;
            }
            Renew_Curve();
            if (transform.position.z < end_z_min)
            {
                Curve_Over(10);
                Destroy(gameObject);
            }
        }
    }
    /// <summary>
    /// 更新曲线
    /// </summary>
    private void Renew_Curve()
    {
        //还未按下时不进行判断
        if (!is_once)
        {
            return;
        }
        if (is_pressed)
        {
            //得到位置
            Vector2 follow_pos = splineFollower.GetComponent<Curve_Follow>().Curve_Point();
            //得到距离
            float distance = Vector2.Distance(follow_pos, Input.touches[touch_index].position);
            //当判定到了就跟随，并且更新曲线
            if (distance <= Cure_Controller.Instance.follow_radious)
            {
                splineFollower.follow = true;
                splineComputer.clipFrom = splineFollower.GetPercent();
            }
            else
            {
                splineFollower.follow = false;
            }
            if (splineComputer.clipFrom == 1)
            {
                Game_Controller.Instance.Set_Score(10);
                Destroy(gameObject);
            }
        }
        else
        {
            //当按下后又松开就判断失败
            splineFollower.follow = false;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 手势结束
    /// </summary>
    /// <param name="damage"></param>
    void Curve_Over(int damage)
    {
        Game_Controller.Instance.Set_HP(-damage);
    }

    /// <summary>
    /// 设置按下事件
    /// </summary>
    /// <param 目标记录的手指位置="touch"></param>
    void Set_Pressed(int index)
    {
        //设置对应变量
        is_once = true;
        is_pressed = true;
        touch_index = index;
    }

    /// <summary>
    /// 结束按下事件
    /// </summary>
    void Disable_Pressed()
    {
        //如果松的是目前的手指的话
        if (Input.touches[DynamicJoystick.Instance.touch_index].position == Input.touches[touch_index].position)
        {
            is_pressed = false;
        }
    }
}
