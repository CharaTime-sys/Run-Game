using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class Normal_Curve : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] SplineRenderer splineComputer;
    [SerializeField] SplineComputer _splineComputer;
    [SerializeField] SplineFollower splineFollower;
    [SerializeField] ParticleSystem particle;

    int touch_index = -1;//Ŀǰ����ָλ��
    [SerializeField] float finger_speed;
    [SerializeField] float delta_time;
    [SerializeField] float distance;
    float delta_timer;
    [SerializeField] Vector2 last_pos;
    [SerializeField] Vector2 point_pos;
    #region �жϱ���
    [SerializeField] bool is_over;//�Ƿ񳬹����ж���Χ
    [SerializeField] bool is_once;//�Ƿ��һ�ΰ���
    [SerializeField] bool is_add_once;//�Ƿ��һ�μ����¼�
    [SerializeField] bool is_add_once_disable;//�Ƿ��һ�μ����¼�(�ſ��¼�)
    [SerializeField] bool is_pressed;//�Ƿ�����
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
    /// ��������
    /// </summary>
    protected virtual void Renew_Curve()
    {
        //��δ����ʱ�������ж�
        if (!is_once)
        {
            return;
        }
        if (is_pressed)
        {
            //�����º�����ȡ���¼�
            if (!is_add_once_disable)
            {
                DynamicJoystick.Instance.disable_Check_Line += Disable_Pressed;
                is_add_once_disable = true;
                particle.Play();
            }
            //�õ�λ��
            Vector2 follow_pos = splineFollower.GetComponent<Curve_Follow>().Curve_Point();
            //�õ�����
            if (Input.touches.Length < touch_index + 1)
            {
                return;//���û����ָ���¾ͷ���
            }
            particle.transform.position = splineFollower.transform.position;
            distance = Vector2.Distance(follow_pos, Input.touches[touch_index].position);
            //���ж����˾͸��棬���Ҹ�������
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
            //�����º����ɿ����ж�ʧ��
            splineFollower.follow = false;
        }
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

    /// <summary>
    /// ���ð����¼�
    /// </summary>
    /// <param Ŀ���¼����ָλ��="touch"></param>
    bool Set_Pressed(int index)
    {
        if (touch_index != -1)
        {
            return false;
        }
        //���ö�Ӧ����
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
