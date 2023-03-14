using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Curve_Block : Block
{
    //���ʱ��ֹͣ�˶�
    [SerializeField] float end_z_min = 5.6f;
    [SerializeField] float end_z_max = 6.4f;
    [SerializeField] SplineRenderer splineComputer;
    [SerializeField] SplineFollower splineFollower;

    bool is_over;//�Ƿ񳬹����ж���Χ
    bool is_pressed;//�Ƿ�����

    private void FixedUpdate()
    {
        ////��������˾Ͳ���ǰ��
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
        ////�������жϷ�Χ��
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
    /// ��������
    /// </summary>
    private void Renew_Curve()
    {
        if (Input.touchCount != 0)
        {
            Debug.Log("�����ж�");
            is_pressed = true;
            //�õ�λ��
            Vector2 follow_pos = splineFollower.GetComponent<Curve_Follow>().Curve_Point();
            //�õ�����
            float distance = Vector2.Distance(follow_pos, Input.touches[0].position);
            //���ж����˾͸��棬���Ҹ�������
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
