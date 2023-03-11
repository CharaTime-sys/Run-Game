using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Cure_Controller : MonoBehaviour
{
    //目前的手势跟踪物体
    [SerializeField] SplineFollower splineFollower;
    //曲线物体
    [SerializeField] SplineRenderer splinecomputer;
    [Header("跟踪半径")]
    [SerializeField] float follow_radious;
    // Update is called once per frame
    void FixedUpdate()
    {
        Renew_Curve();
    }

    /// <summary>
    /// 更新曲线
    /// </summary>
    private void Renew_Curve()
    {
        if (Input.touchCount != 0)
        {
            //得到位置
            Vector2 follow_pos = Camera.main.WorldToScreenPoint(splineFollower.transform.position);
            //得到距离
            float distance = Vector2.Distance(follow_pos, Input.touches[0].position);
            //当判定到了就跟随，并且更新曲线
            if (distance <= follow_radious)
            {
                splineFollower.follow = true;
                splinecomputer.clipFrom = splineFollower.GetPercent();
            }
            else
            {
                splineFollower.follow = false;
            }
        }
        else
        {
            splineFollower.follow = false;
        }
    }
}
