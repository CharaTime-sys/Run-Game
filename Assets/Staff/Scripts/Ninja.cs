using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ninja : MonoBehaviour
{
    #region ʱ�����
    [Header("ʱ�����------------------------------------------")]
    [Header("�����ָ�ʱ��")]
    public Vector2 time;
    [Header("�ָ�ʱ��")]
    public float resume_time;
    [Header("�����ƶ���ʱ��")]
    [SerializeField] float move_time;
    [Header("�޵�ʱ��")]
    [SerializeField] float unmatched_time;
    #endregion

    #region �������
    [Header("�������------------------------------------------")]
    [Header("��Ծ���¶׷���,xΪ��Ծ���ȣ�yΪ�»�����")]
    [SerializeField] Vector2 range;
    [Header("�����ƶ��ľ���")]
    [SerializeField] float move_distance;
    #endregion

    #region ״̬���
    //�����ƶ���״̬(�����������ײ��Ͳ�������Ӧ�����ƶ���
    int dir_component;

    //�Ƿ��г���buff״̬
    [SerializeField] bool is_buffing;
    //��Ծ���ƶ����»�״̬
    public bool is_jumping;
    public bool is_downing;
    bool is_moving;
    //�Ƿ��޵�
    bool is_unmathcing;
    #endregion

    #region ˽�б���
    [Header("��ɫ����")]
    [SerializeField] Animator chara;
    [SerializeField] Vector3 start_pos;

    //Ѫ���ͷ���
    int hp = 100;
    int score = 0;
    float _unmatched_time;//�޵�ʱ���ʱ��
    #endregion

    #region �������
    public bool Is_buffing { get => is_buffing; set => is_buffing = value; }
    public int Hp { get => hp; set {
            hp = value;
            if (hp<=0)
            {
                hp = 0;
            }
        } }
    public int Score { get => score;set
        {
            score = value;
            Game_Controller.Instance.score_ui.text = "������" + score.ToString();
        }
    }
    #endregion

    private void Update()
    {
        Set_Unmatching();
    }

    /// <summary>
    /// �޵�״̬���
    /// </summary>
    void Set_Unmatching()
    {
        if (is_unmathcing)
        {
            if (_unmatched_time >= 0f)
            {
                _unmatched_time -= Time.deltaTime;
            }
            else
            {
                is_unmathcing = false;//�˳��޵�״̬
                GetComponent<BoxCollider>().enabled = true;//ȡ�����˼��
                Game_Controller.Instance.status_ui.gameObject.SetActive(false);
            }
        }
    }

    #region ��Ҷ������
    /// <summary>
    /// ���Ա��ƶ�
    /// </summary>
    /// <param ����="direction"></param>
    public void Move_Left_And_Right(int direction)
    {
        ////������Χ�����ƶ�
        if ((dir_component < 0 && direction < 0) || (dir_component > 0 && direction > 0) || is_moving)
        {
            return;
        }
        //transform.DOMove(new Vector3(transform.position.x + direction * move_distance,transform.position.y,transform.position.z), move_time).SetEase(Ease.Linear);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, -direction);
        is_moving = true;
        //����ƶ�״̬
        Invoke(nameof(Reset_Move), move_time);
    }

    /// <summary>
    /// ȡ���ƶ�״̬
    /// </summary>
    private void Reset_Move()
    {
        is_moving = false;
    }

    /// <summary>
    /// ��Ծ
    /// </summary>
    /// <param �Ƿ���Ҫ����="if_resumed"></param>
    public void Jump(bool if_resumed)
    {
        if (is_jumping)
        {
            return;
        }
        Debug.Log("��Ծ");
        is_jumping = true;//������Ծ״̬
        is_downing = false;
        chara.Play("Jump");//���Ŷ���
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, 2);
        transform.DOMoveY(start_pos.y+range.x, time.x);//��Ծ
        //����ڿգ�����Ҫ����
        if (if_resumed)
        {
            Invoke(nameof(Resume_Jump), resume_time);
        }
    }

    /// <summary>
    /// �»�
    /// </summary>
    /// <param �Ƿ���Ҫ����="if_resumed"></param>
    public void Down(bool if_resumed)
    {
        if (is_downing)
        {
            return;
        }
        Debug.Log("�»�");
        is_downing = true;
        is_jumping = false;
        chara.Play("Down");
        transform.DOMoveY(start_pos.y - range.y, time.y);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, 0);
        //����ڿգ�����Ҫ����
        if (if_resumed)
        {
            Invoke(nameof(Resume_Down), resume_time);
        }
    }

    /// <summary>
    /// ����y����ķ���
    /// </summary>
    private void Reset_Y_Action()
    {
        CancelInvoke();
        transform.position = new Vector3(transform.position.x, start_pos.y, transform.position.z);
    }

    /// <summary>
    /// ������Ծ
    /// </summary>
    public void Resume_Jump()
    {
        Debug.Log("������Ծ");
        //���Ŷ���
        chara.Play("Run");
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(true, 2);
        transform.DOMove(start_pos, time.x);
        is_jumping = false;
    }

    /// <summary>
    /// �����»�
    /// </summary>
    public void Resume_Down()
    {
        Debug.Log("�����»�");
        chara.Play("Run");
        transform.DOMove(start_pos, time.y);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(true, 0);
        is_downing = false;
    }
    #endregion

    /// <summary>
    /// ����buffЧ��
    /// </summary>
    public void Set_Buff_Status()
    {
        is_buffing = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //��������߽�Ļ����޷���������ͬ�ķ����ƶ�
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

        //����������ϰ������Ѫ
        if (other.tag == "Block")
        {
            if (is_unmathcing)
            {
                return;
            }
            //��Ѫ
            Game_Controller.Instance.Set_HP(other.gameObject.GetComponent<Block>().damage);
            //�����޵�״̬
            is_unmathcing = true;
            _unmatched_time = unmatched_time;
            GetComponent<BoxCollider>().enabled = false;//ȡ�����˼��
            //��ʾui�������ж�
            Game_Controller.Instance.status_ui.gameObject.SetActive(true);
        }
        //���������buff��������Ӧ��buff
        if (other.tag == "Buff")
        {
            Debug.Log("����buff��");
            Game_Controller.Instance.Set_buff_time_And_type(other.GetComponent<Buff_Block>().buff_time, other.GetComponent<Buff_Block>().buff_Type);
            Set_Buff_Status();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //�뿪�˱߽��Ժ�ָ������ƶ�
        if (other.tag == "Border")
        {
            dir_component = 0;
        }
    }
}
