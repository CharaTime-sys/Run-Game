using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Normal_Curve : MonoBehaviour
{
    [Header("曲线组件")]
    [SerializeField] SplineRenderer splineComputer;
    [SerializeField] SplineComputer _splineComputer;
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] ParticleSystem particle;

    int touch_index = -1;//目前的手指位置
    [SerializeField] float finger_speed;
    [SerializeField] float delta_time;
    [SerializeField] float distance;
    float delta_timer;
    [SerializeField] Vector2 last_pos;
    [SerializeField] Vector2 point_pos;
    #region 判断变量
    [SerializeField] bool is_over;//是否超过了判定范围
    [SerializeField] bool is_once;//是否第一次按下
    [SerializeField] bool is_add_once;//是否第一次加入事件
    [SerializeField] bool is_add_once_disable;//是否第一次加入事件(放开事件)
    [SerializeField] bool is_pressed;//是否按下了
    #endregion

    [SerializeField] float curve_delta;
    // Start is called before the first frame update
    void Start()
    {
        point_pos = Camera.main.WorldToScreenPoint(_splineComputer.GetPoint(_splineComputer.pointCount - 1).position);
        if (!is_add_once)
        {
            DynamicJoystick.Instance.set_Check_Line += Set_Pressed;
            is_add_once = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        splineComputer.clipFrom = splineFollower.GetPercent();
        Renew_Curve();
    }

    /// <summary>
    /// 更新曲线
    /// </summary>
    protected virtual void Renew_Curve()
    {
        //还未按下时不进行判断
        if (!is_once)
        {
            return;
        }
        if (is_pressed)
        {
            //当按下后才添加取消事件
            if (!is_add_once_disable)
            {
                DynamicJoystick.Instance.disable_Check_Line += Disable_Pressed;
                is_add_once_disable = true;
                particle.Play();
            }
            //得到位置
            Vector2 follow_pos = splineFollower.GetComponent<Curve_Follow>().Curve_Point();
            //得到距离
            if (Input.touches.Length < touch_index + 1)
            {
                return;//如果没有手指按下就返回
            }
            particle.transform.position = splineFollower.transform.position;
            distance = Vector2.Distance(follow_pos, Input.touches[touch_index].position);
            //当判定到了就跟随，并且更新曲线
            if (distance <= Cure_Controller.Instance.follow_radious || Vector2.Distance(Input.touches[touch_index].position,point_pos) < Vector2.Distance(follow_pos, point_pos))
            {
                splineFollower.follow = true;
            }
            else
            {
                splineFollower.follow = false;
            }
            if (splineComputer.clipFrom >= 0.98f)
            {
                Game_Controller.Instance.Game_Start();
                Destroy(gameObject);
            }
        }
        else
        {
            //当按下后又松开就判断失败
            splineFollower.follow = false;
        }
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

    /// <summary>
    /// 设置按下事件
    /// </summary>
    /// <param 目标记录的手指位置="touch"></param>
    bool Set_Pressed(int index)
    {
        if (touch_index != -1)
        {
            return false;
        }
        //设置对应变量
        if (distance <= Cure_Controller.Instance.follow_radious)
        {
            is_once = true;
            is_pressed = true;
            touch_index = index;
            return true;
        }
        return false;
    }
}
