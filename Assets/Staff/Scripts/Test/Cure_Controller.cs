using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Cure_Controller : MonoBehaviour
{
    //Ŀǰ�����Ƹ�������
    [SerializeField] SplineFollower splineFollower;
    //��������
    [SerializeField] SplineRenderer splinecomputer;
    [Header("���ٰ뾶")]
    [SerializeField] float follow_radious;
    // Update is called once per frame
    void FixedUpdate()
    {
        Renew_Curve();
    }

    /// <summary>
    /// ��������
    /// </summary>
    private void Renew_Curve()
    {
        if (Input.touchCount != 0)
        {
            //�õ�λ��
            Vector2 follow_pos = Camera.main.WorldToScreenPoint(splineFollower.transform.position);
            //�õ�����
            float distance = Vector2.Distance(follow_pos, Input.touches[0].position);
            //���ж����˾͸��棬���Ҹ�������
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
