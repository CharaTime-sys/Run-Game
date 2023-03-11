using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ninja : MonoBehaviour
{
    [Header("恢复时间")]
    public float resume_time;

    [Header("跳跃和下蹲幅度")]
    public Vector2 range;
    public Vector2 time;
    private Vector3 start_pos;
    [Header("左右移动的距离和时间")]
    [SerializeField] float move_distance;
    [SerializeField] float move_time;
    [Header("角色动画")]
    public Animator chara;
    //左右移动的状态
    int dir_component;
    //正在移动的状态
    bool is_moving;
    //是否有持续buff
    [SerializeField] bool is_buffing;

    public bool Is_buffing { get => is_buffing; set => is_buffing = value; }

    // Start is called before the first frame update
    void Start()
    {
        Set_Buff_Status();
        start_pos = transform.position;
    }

    //重置buff效果
    public void Set_Buff_Status()
    {
        is_buffing = true;
    }

    #region For Bodys
    public void Move_Left_And_Right(int direction)
    {
        //超过范围不能移动
        if ((dir_component < 0 && direction < 0) || (dir_component > 0 && direction > 0) || is_moving)
        {
            return;
        }
        transform.DOMove(new Vector3(transform.position.x + direction * move_distance,transform.position.y,transform.position.z), move_time).SetEase(Ease.Linear);
        is_moving = true;
        //清空移动状态
        Invoke("Reset_Move", move_time);
    }

    private void Reset_Move()
    {
        is_moving = false;
    }
    #endregion
    public void Jump(bool if_resumed)
    {
        Reset_Y_Action();
        chara.Play("Jump");
        GetComponent<Rigidbody>().useGravity = false;
        transform.DOMoveY(range.x, time.x);
        //如果腾空，则不需要调用
        if (if_resumed)
        {
            Invoke(nameof(Resume_Jump), resume_time);
        }
    }

    public void Down(bool if_resumed)
    {
        Reset_Y_Action();
        chara.Play("Down");
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().enabled = false;
        transform.DOMoveY(start_pos.y - range.y, time.y);
        //如果腾空，则不需要调用
        if (if_resumed)
        {
            Invoke(nameof(Resume_Down), resume_time);
        }
    }

    /// <summary>
    /// 重置y方向的分量
    /// </summary>
    private void Reset_Y_Action()
    {
        CancelInvoke();
        transform.position = new Vector3(transform.position.x, start_pos.y, transform.position.z);
    }

    public void Resume_Jump()
    {
        //播放动画
        chara.Play("Run");
        //返回原地
        GetComponent<Rigidbody>().useGravity = true;
    }

    public void Resume_Down()
    {
        chara.Play("Run");
        GetComponent<BoxCollider>().enabled = true;
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
