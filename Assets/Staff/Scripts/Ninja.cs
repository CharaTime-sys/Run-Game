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
    [Header("�»��ָ�ʱ��")]
    public float down_resume_time;
    [Header("�����ƶ���ʱ��")]
    [SerializeField] float move_time;
    [Header("�޵�ʱ��")]
    [SerializeField] float unmatched_time;
    [Header("��˸Ƶ��")]
    [SerializeField] float unmatched_frequency;
    [Header("BUFF�ӷּ��ʱ��")]
    [SerializeField] float buff_delta_time = 0.3f;
    [SerializeField] float buff_delta_timer = 0.3f;
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
    [SerializeField] int dir_component;

    //�Ƿ��г���buff״̬
    [SerializeField] bool is_buffing;
    //��Ծ���ƶ����»�״̬
    public bool is_jumping;
    public bool is_downing;
    [SerializeField] bool is_moving;
    //�Ƿ��޵�
    bool is_unmathcing;
    #endregion

    #region ˽�б���
    [Header("��ɫ����")]
    [SerializeField] Animator chara;
    [SerializeField] Vector3 start_pos;
    [SerializeField] SkinnedMeshRenderer player_render;
    Tweener tw;
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
        if (Is_buffing &&(is_downing || is_jumping))
        {
            if (buff_delta_timer <= 0f)
            {
                switch (Game_Controller.Instance.Buff_Type)
                {
                    case Buff_Type.Jump:
                        if (is_jumping)
                        {
                            Game_Controller.Instance.Set_Score(10);
                        }
                        break;
                    case Buff_Type.Down:
                        if (is_downing)
                        {
                            Game_Controller.Instance.Set_Score(10);
                        }
                        break;
                    default:
                        break;
                }
                buff_delta_timer = buff_delta_time;
            }
            else
            {
                buff_delta_timer -= Time.deltaTime;
            }
        }
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
                //��ɫ������˸
                StartCoroutine(Player_Hurted());
            }
            else
            {
                is_unmathcing = false;//�˳��޵�״̬
                GetComponent<BoxCollider>().enabled = true;//ȡ�����˼��
                StopAllCoroutines();
                player_render.enabled = true;
                Game_Controller.Instance.status_ui.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// ��ɫ����
    /// </summary>
    /// <returns></returns>
    IEnumerator Player_Hurted()
    {
        yield return new WaitForSeconds(unmatched_frequency);
        player_render.enabled = !player_render.enabled;
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
        //������Ч
        Game_Controller.Instance.Play_Effect(3);
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
        GetComponent<BoxCollider>().enabled = true;
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
        GetComponent<BoxCollider>().enabled = false;
        is_jumping = true;//������Ծ״̬
        is_downing = false;
        chara.Play("Jump");//���Ŷ���
        CancelInvoke();
        //ֹͣ��һ�εĶ���
        tw.Pause();
        //������Ч
        Game_Controller.Instance.Play_Effect(0);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, 2);
        tw = transform.DOMoveY(start_pos.y+range.x, time.x);//��Ծ
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
        //�ı���ײ�����
        GetComponent<BoxCollider>().enabled = false;
        is_downing = true;
        is_jumping = false;
        chara.Play("Down");
        CancelInvoke();
        //ֹͣ��һ�εĶ���
        tw.Pause();
        //������Ч
        Game_Controller.Instance.Play_Effect(2);
        tw = transform.DOMoveY(start_pos.y - range.y, time.y);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, 0);
        //����ڿգ�����Ҫ����
        if (if_resumed)
        {
            Invoke(nameof(Resume_Down), down_resume_time);
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
        GetComponent<BoxCollider>().enabled = true;
        //���Ŷ���
        chara.Play("Run");
        //ֹͣ��һ�εĶ���
        tw.Pause();
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(true, 2);
        tw = transform.DOMove(start_pos, time.x);
        is_jumping = false;
    }

    /// <summary>
    /// �����»�
    /// </summary>
    public void Resume_Down()
    {
        //�ı���ײ�����
        GetComponent<BoxCollider>().enabled = true;
        chara.Play("Run");
        //ֹͣ��һ�εĶ���
        tw.Pause();
        tw = transform.DOMove(start_pos, time.y);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(true, 0);
        is_downing = false;
    }
    #endregion

    /// <summary>
    /// ����buffЧ��
    /// </summary>
    public void Set_Buff_Status(bool enable)
    {
        is_buffing = enable;
        if (!enable)
        {
            Game_Controller.Instance.Set_Jump_UI(false);
            Game_Controller.Instance.Set_Down_UI(false);
            StopAllCoroutines();
        }
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
            Debug.Log("�����߽���");
        }

        //����������ϰ������Ѫ
        if (other.tag == "Block")
        {
            if (is_unmathcing || other.GetComponent<Block>().If_great)
            {
                return;
            }
            other.GetComponent<Block>().Set_loss();//����ʧ��
            Game_Controller.Instance.cur_block.Turn_Next();
            Set_Buff_Status(false);
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
            Game_Controller.Instance.Set_buff_time_And_type(other.GetComponent<Buff_Block>().buff_time, other.GetComponent<Buff_Block>().buff_Type);
            Set_Buff_Status(true);
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
