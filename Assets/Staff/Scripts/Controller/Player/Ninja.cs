using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ninja : MonoBehaviour
{
    #region ʱ�����
    [Header("ʱ�����------------------------------------------")]
    [Header("�����ָ�ʱ��")]
    public Vector2 time = new Vector2(0.2f,0.2f);
    [Header("�ָ�ʱ��")]
    public float resume_time = 0.5f;
    [Header("�»��ָ�ʱ��")]
    public float down_resume_time = 0.5f;
    [Header("�����ƶ���ʱ��")]
    [SerializeField] float move_time = 0.1f;
    [Header("�޵�ʱ��")]
    [SerializeField] float unmatched_time = 2f;
    [Header("��˸Ƶ��")]
    [SerializeField] float unmatched_frequency = 0.3f;
    [Header("BUFF�ӷּ��ʱ��")]
    [SerializeField] float buff_delta_time = 0.1f;
    [SerializeField] float buff_delta_timer = 0.1f;
    #endregion

    #region �������
    [Header("�������------------------------------------------")]
    [Header("��Ծ���¶׷���,xΪ��Ծ���ȣ�yΪ�»�����")]
    [SerializeField] Vector2 range = new Vector2(6,0.6f);
    [Header("�����ƶ��ľ���")]
    [SerializeField] float move_distance = 5;
    #endregion

    #region ״̬���
    //�����ƶ���״̬(�����������ײ��Ͳ�������Ӧ�����ƶ���
    [SerializeField] int dir_component;

    //�Ƿ��г���buff״̬
    [SerializeField] bool is_buffing;
    //��Ծ���ƶ����»�״̬
    public bool is_jumping;
    public bool is_downing;
    public bool is_returning;
    [SerializeField] bool is_moving;
    //�Ƿ��޵�
    bool is_unmathcing;
    #endregion

    #region ˽�б���
    [Header("��ɫ����")]
    [SerializeField] Animator chara;
    [SerializeField] Vector3 start_pos = new Vector3(3.387681f, -5.86f,0);
    [SerializeField] SkinnedMeshRenderer[] player_render;
    [SerializeField] float move_y = 0.8f;
    Tweener tw;
    [Header("����ƫ��")]
    public float offset_z = 2.28f;
    public float offset_y = 2.28f;
    [Header("��ײ�����")]
    [SerializeField] Vector2[] collider_size_and_pos_y;
    //Ѫ���ͷ���
    [SerializeField] int hp = 250;
    int max_hp = 250;
    float score = 0;
    [Header("���������ٶ�")]
    [SerializeField]
    float score_speed = 5;
    float _unmatched_time;//�޵�ʱ���ʱ��
    #endregion

    #region �������
    public bool Is_buffing { get => is_buffing; set => is_buffing = value; }
    public int Hp { get => hp; set {
            hp = value;
            UI_Manager.Instance.Set_Hp_UI();
            if (hp<=0)
            {
                hp = 0;
                _Game_End();
            }
        } }

    private static void _Game_End()
    {
        Game_Controller.Instance.Game_End();
    }

    public float Score { get => score;set
        {
            score = value;
            UI_Manager.Instance.Set_Score_UI();
        }
    }

    public int Dir_component { get => dir_component;}
    public int Max_hp { get => max_hp;}
    #endregion

    private void Update()
    {
        if (!Game_Controller.Instance.game_started)
        {
            return;
        }
        Score += score_speed * Time.deltaTime;
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
        Ray_cast();
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
                foreach (var item in player_render)
                {
                    item.enabled = true;
                }
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
        foreach (var item in player_render)
        {
            item.enabled = !item.enabled;
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
            chara.Play("Jump");//���Ŷ���
            Invoke(nameof(Resume_Jump), resume_time);
        }
        else
        {
            chara.Play("long jump");//���Ŷ���
        }
        is_returning = false;
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
        is_returning = false;
    }

    private void Set_Collider(int index)
    {
        GetComponent<BoxCollider>().center = new Vector3(0, collider_size_and_pos_y[index].x, 0);
        GetComponent<BoxCollider>().size = new Vector3(1.92f, collider_size_and_pos_y[index].y, 2.9f);
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
        is_returning = true;
        Invoke(nameof(Set_Returning), time.x);
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
        is_returning = true;
        Invoke(nameof(Set_Returning), time.y);
        Game_Controller.Instance.is_reached = false;
    }

    void Set_Returning()
    {
        is_returning = false;
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
        }
    }

    public void Ray_cast()
    {
        Ray ray = new Ray(transform.position-new Vector3(0,offset_y, offset_z), Vector3.down);
        Ray ray_cur = new Ray(transform.position-new Vector3(0,offset_y, 0), Vector3.down);
        Ray ray_forward = new Ray(transform.position+new Vector3(0, -offset_y, offset_z), Vector3.down);
        if (!Physics.Raycast(ray,2f,LayerMask.GetMask("Ground"))&& !Physics.Raycast(ray_forward, 2f, LayerMask.GetMask("Ground")) && !Physics.Raycast(ray_cur, 2f, LayerMask.GetMask("Ground"))
            && !is_jumping && !is_downing && !is_returning && !is_moving)
        {
            //Before_Game_End();
        }
    }

    private void Before_Game_End()
    {
        is_jumping = true;
        is_downing = true;
        is_moving = true;
        transform.DOLocalMoveY(transform.localPosition.y - 7f, 0.2f);
        Invoke(nameof(Game_End), 0.2f);
    }

    public void Game_End()
    {
        gameObject.SetActive(false);
        Game_Controller.Instance.Game_End();
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position - new Vector3(0, offset_y, offset_z), Vector3.down * 2f, Color.red);
        Debug.DrawRay(transform.position - new Vector3(0, offset_y, 0), Vector3.down * 2f, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0, -offset_y, offset_z), Vector3.down * 2f, Color.red);
    }
    /// <summary>
    /// ��Ϸʤ��
    /// </summary>
    void Game_Win()
    {
        Game_Controller.Instance.Game_Win();
    }
    private void OnTriggerEnter(Collider other)
    {
        //����������ϰ������Ѫ
        if (other.tag == "Block")
        {
            if (other.name.Contains("win"))
            {
                Invoke(nameof(Game_Win), 5f);
                return;
            }
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

    [ContextMenu("�ı��ɫ")]
    public void Change_Character()
    {
        start_pos = new Vector3(3.3848f, -7.62f, -2.7974f);
        transform.localPosition = start_pos;
    }
}
