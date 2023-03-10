using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ninja : MonoBehaviour
{
    [Header("�ָ�ʱ��")]
    public float resume_time;

    [Header("��Ծ���¶׷���")]
    public Vector2 range;
    public Vector2 time;
    private Vector3 start_pos;
    [Header("�����ƶ��ľ����ʱ��")]
    [SerializeField] float move_distance;
    [SerializeField] float move_time;
    [Header("��ɫ����")]
    public Animator chara;
    //�����ƶ���״̬
    int dir_component;
    //�����ƶ���״̬
    bool is_moving;
    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position;
    }

    #region For Bodys
    public void Move_Left_And_Right(int direction)
    {
        //������Χ�����ƶ�
        if ((dir_component < 0 && direction < 0) || (dir_component > 0 && direction > 0) || is_moving)
        {
            return;
        }
        transform.DOMove(new Vector3(transform.position.x + direction * move_distance,transform.position.y,transform.position.z), move_time).SetEase(Ease.Linear);
        is_moving = true;
        //����ƶ�״̬
        Invoke("Reset_Move", move_time);
    }

    private void Reset_Move()
    {
        is_moving = false;
    }
    #endregion
    public void Jump()
    {
        Reset_Y_Action();
        chara.Play("Jump");
        transform.DOMoveY(range.x, time.x);
        Invoke("Resume_Jump", resume_time);
    }

    public void Down()
    {
        Reset_Y_Action();
        chara.Play("Down");
        transform.DOMoveY(start_pos.y - range.y, time.y);
        Invoke("Resume_Down", resume_time);
    }

    /// <summary>
    /// ����y����ķ���
    /// </summary>
    private void Reset_Y_Action()
    {
        CancelInvoke();
        transform.position = new Vector3(transform.position.x, start_pos.y, transform.position.z);
    }

    public void Resume_Jump()
    {
        chara.Play("Run");
        transform.DOMoveY(start_pos.y, time.x).SetEase(Ease.Linear);
    }

    public void Resume_Down()
    {
        chara.Play("Run");
        transform.DOMoveY(start_pos.y, time.y).SetEase(Ease.Linear);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Border")
        {
            if (other.name == "Border")
            {
                dir_component = -1;
            }
            else if (other.name == "Border_2")
            {
                dir_component = 1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Border")
        {
            dir_component = 0;
        }
    }
}
