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
    [Header("�޵�ʱ��")]
    [SerializeField] float unmatched_time;
    float _unmatched_time;

    [Header("��ɫ����")]
    public Animator chara;
    //�����ƶ���״̬
    int dir_component;
    //�����ƶ���״̬
    bool is_moving;
    //�Ƿ��г���buff
    [SerializeField] bool is_buffing;
    [SerializeField] bool is_jumping;
    [SerializeField] bool is_downing;
    //�Ƿ��޵�
    bool is_unmathcing;

    public bool Is_buffing { get => is_buffing; set => is_buffing = value; }
    public int Hp { get => hp; set {
            hp = value;
            if (hp<=0)
            {
                hp = 0;
            }
        } }

    [Header("��ɫѪ��")]
    private int hp = 100;
    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        Set_Buff_Status();
        start_pos = transform.position;
    }

    private void Update()
    {
        if (is_unmathcing)
        {
            if (_unmatched_time >=0f)
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

    //����buffЧ��
    public void Set_Buff_Status()
    {
        is_buffing = true;
    }

    #region For Bodys
    public void Move_Left_And_Right(int direction)
    {
        //������Χ�����ƶ�
        if ((dir_component < 0 && direction < 0) || (dir_component > 0 && direction > 0) || is_moving)
        {
            return;
        }
        //Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, direction);
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
    public void Jump(bool if_resumed)
    {
        if (is_jumping)
        {
            return;
        }
        is_jumping = true;
        Reset_Y_Action();
        chara.Play("Jump");
        transform.DOMoveY(range.x, time.x);
        //����ڿգ�����Ҫ����
        if (if_resumed)
        {
            Invoke(nameof(Resume_Jump), resume_time);
        }
    }

    public void Down(bool if_resumed)
    {
        if (is_downing)
        {
            return;
        }
        is_downing = true;
        Reset_Y_Action();
        chara.Play("Down");
        transform.DOMoveY(start_pos.y - range.y, time.y);
        //Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, 0);
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

    public void Resume_Jump()
    {
        //���Ŷ���
        chara.Play("Run");
        transform.DOMoveY(start_pos.y, time.y);
        is_jumping = false;
    }

    public void Resume_Down()
    {
        chara.Play("Run");
        transform.DOMoveY(start_pos.y, time.y);
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(true, 0);
        is_downing = false;
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
        if (other.tag == "Block")
        {
            if (is_unmathcing)
            {
                return;
            }
            Game_Controller.Instance.Set_HP(other.gameObject.GetComponent<Block>().damage);
            is_unmathcing = true;//�����޵�״̬
            _unmatched_time = unmatched_time;
            GetComponent<BoxCollider>().enabled = false;//ȡ�����˼��
            Game_Controller.Instance.status_ui.gameObject.SetActive(true);
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
