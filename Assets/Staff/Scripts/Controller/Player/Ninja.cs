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
    [SerializeField] float move_y;
    Tweener tw;
    [Header("��ײ�����")]
    [SerializeField] Vector2[] collider_size_and_pos_y;
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
            UI_Manager.Instance.Set_Score_UI();
        }
    }
    #endregion

    private void Update()
    {
        if (!Game_Controller.Instance.game_started)
        {
            return;
        }
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
                StopAllCoroutines();
                player_render.enabled = true;
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
        dir_component += direction;
        //������Ч
        AudioManager.instance.PlaySFX(3);
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
        Set_Collider(2);
        is_jumping = true;//������Ծ״̬
        is_downing = false;
        chara.Play("Jump");//���Ŷ���
        CancelInvoke();
        //ֹͣ��һ�εĶ���
        tw.Pause();
        //������Ч
        AudioManager.instance.PlaySFX(0);
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
        Set_Collider(1);
        is_downing = true;
        is_jumping = false;
        chara.Play("Down");
        CancelInvoke();
        //ֹͣ��һ�εĶ���
        tw.Pause();
        //������Ч
        AudioManager.instance.PlaySFX(2);
        tw = transform.DOMoveY(start_pos.y - range.y, time.y);
        //���þ�ͷת��
        Camera.main.GetComponent<Camera_Controller>().Change_Camera_Status(false, 0);
        //����ڿգ�����Ҫ����
        if (if_resumed)
        {
            Invoke(nameof(Resume_Down), down_resume_time);
        }
    }

    private void Set_Collider(int index)
    {
        GetComponent<BoxCollider>().center = new Vector3(0, collider_size_and_pos_y[index].x, 0);
        GetComponent<BoxCollider>().size = new Vector3(0.5f, collider_size_and_pos_y[index].y, 0.5f);
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
        //�ı���ײ�����
        Set_Collider(0);
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
        Set_Collider(0);
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
        Particle_Controller.Instance.Set_Particle_visual(Particle_Controller.Instance.buff_particle, enable);
        if (!enable)
        {
            UI_Manager.Instance.Set_Jump_UI(false);
            UI_Manager.Instance.Set_Down_UI(false);
            StopAllCoroutines();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //����������ϰ������Ѫ
        if (other.tag == "Block" && !other.GetComponent<Block>().if_end)
        {
            //Game_Controller.Instance.cur_block.Turn_Next();
            if (is_unmathcing || other.GetComponent<Block>().If_great)
            {
                return;
            }
            other.GetComponent<Block>().Set_loss();//����ʧ��
            other.GetComponent<Block>().Set_Collider();//����ʧ��
            Set_Buff_Status(false);
            //��Ѫ
            Game_Controller.Instance.Set_HP(other.gameObject.GetComponent<Block>().damage);
            //�����޵�״̬
            is_unmathcing = true;
            is_moving = false;
            _unmatched_time = unmatched_time;
        }
        if (other.transform.parent.GetComponent<Buff_Block>()!=null)
        {
            transform.DOLocalMoveY(transform.localPosition.y + move_y, 0.5f);
        }
    }
}
