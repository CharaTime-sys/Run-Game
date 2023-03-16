using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.EventSystems;

public class Curve_Block : Block
{
    //���ʱ��ֹͣ�˶�
    [SerializeField] float end_z_min = 5.6f;
    [SerializeField] float end_z_max = 6.4f;
    [SerializeField] SplineRenderer splineComputer;
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] int touch_index;//Ŀǰ����ָλ��

    bool is_over;//�Ƿ񳬹����ж���Χ
    bool is_once;//�Ƿ��һ�ΰ���
    bool is_add_once;//�Ƿ��һ�μ����¼�
    bool is_pressed;//�Ƿ�����
    private void FixedUpdate()
    {
        ////��������˾Ͳ���ǰ��
        if (!is_pressed && !is_over)
        {
            base.FixedUpdate();
        }
    }
    // Update is called once per frame
    void Update()
    {
        ////�������жϷ�Χ��
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
    /// ��������
    /// </summary>
    private void Renew_Curve()
    {
        //��δ����ʱ�������ж�
        if (!is_once)
        {
            return;
        }
        if (is_pressed)
        {
            //�õ�λ��
            Vector2 follow_pos = splineFollower.GetComponent<Curve_Follow>().Curve_Point();
            //�õ�����
            float distance = Vector2.Distance(follow_pos, Input.touches[touch_index].position);
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
                Destroy(gameObject);
            }
        }
        else
        {
            //�����º����ɿ����ж�ʧ��
            splineFollower.follow = false;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���ƽ���
    /// </summary>
    /// <param name="damage"></param>
    void Curve_Over(int damage)
    {
        Game_Controller.Instance.Set_HP(-damage);
    }

    /// <summary>
    /// ���ð����¼�
    /// </summary>
    /// <param Ŀ���¼����ָλ��="touch"></param>
    void Set_Pressed(int index)
    {
        //���ö�Ӧ����
        is_once = true;
        is_pressed = true;
        touch_index = index;
    }

    /// <summary>
    /// ���������¼�
    /// </summary>
    void Disable_Pressed()
    {
        //����ɵ���Ŀǰ����ָ�Ļ�
        if (Input.touches[DynamicJoystick.Instance.touch_index].position == Input.touches[touch_index].position)
        {
            is_pressed = false;
        }
    }
}
