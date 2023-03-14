using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Curve_Block : Block
{
    //这个时候停止运动
    [SerializeField] float end_z_min = 5.6f;
    [SerializeField] float end_z_max = 6.4f;
    [SerializeField] SplineRenderer splineComputer;
    [SerializeField] SplineFollower splineFollower;

    bool is_over;//是否超过了判定范围
    bool is_pressed;//是否按下了

    private void FixedUpdate()
    {
        ////如果超过了就不会前进
        if (!is_pressed)
        {
            if (!is_over)
            {
                base.FixedUpdate();
            }
            else
            {
                Curve_Over(10);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        ////当进入判断范围内
        if (transform.position.z < end_z_max)
        {
            Renew_Curve();
        }
        if (transform.position.z < end_z_min)
        {
            Curve_Over(10);
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 更新曲线
    /// </summary>
    private void Renew_Curve()
    {
        if (Input.touchCount != 0)
        {
            Debug.Log("进入判断");
            is_pressed = true;
            //得到位置
            Vector2 follow_pos = splineFollower.GetComponent<Curve_Follow>().Curve_Point();
            //得到距离
            float distance = Vector2.Distance(follow_pos, Input.touches[0].position);
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
                Curve_Over(0);
            }
        }
        else
        {
            splineFollower.follow = false;
        }
    }

    void Curve_Over(int damage)
    {
        Game_Controller.Instance.Set_HP(-damage);
    }
}
