using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ninja : MonoBehaviour
{
    //temp to not consider the four rigids
    //[Header("胳膊和手")]
    //public GameObject left_leg;
    //public GameObject right_leg;
    //public GameObject right_arm;
    //public GameObject left_arm;

    [Header("恢复时间")]
    public float resume_time;

    [Header("跳跃和下蹲幅度")]
    public Vector2 range;
    public Vector2 time;
    private Vector3 start_pos;
    [Header("角色动画")]
    public Animator chara;
    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }

    #region For Bodys
    //public void Set_Left_Leg()
    //{
    //    left_leg.transform.localEulerAngles = new Vector3(0, 0, 270);
    //    Invoke("Resume_Left_Leg", resume_time);
    //}
    //public void Set_Right_Leg()
    //{
    //    right_leg.transform.localEulerAngles = new Vector3(0, 0, 90);
    //    Invoke("Resume_Right_Leg", resume_time);
    //}
    //public void Set_Left_Arm()
    //{
    //    left_arm.transform.localEulerAngles = new Vector3(90, 0, 0);
    //    Invoke("Resume_Left_Arm", resume_time);
    //}
    //public void Set_Right_Arm()
    //{
    //    right_arm.transform.localEulerAngles = new Vector3(90, 0, 0);
    //    Invoke("Resume_Right_Arm", resume_time);
    //}

    //public void Resume_Left_Leg()
    //{
    //    left_leg.transform.localEulerAngles = new Vector3(0, 0, 180);
    //}
    //public void Resume_Right_Leg()
    //{
    //    right_leg.transform.localEulerAngles = new Vector3(0, 0, 180);
    //}
    //public void Resume_Left_Arm()
    //{
    //    left_arm.transform.localEulerAngles = new Vector3(-45, 0, 0);
    //}
    //public void Resume_Right_Arm()
    //{
    //    right_arm.transform.localEulerAngles = new Vector3(-45, 0, 0);
    //}
    #endregion

    public void Jump()
    {
        CancelInvoke();
        transform.position = start_pos;
        transform.DOMoveY(range.x, time.x);
        chara.Play("Jump");
        Invoke("Resume_Jump", resume_time);
    }

    public void Down()
    {
        CancelInvoke();
        transform.position = start_pos;
        chara.Play("Down");
        transform.DOMoveY(start_pos.y-range.y, time.y);
        Invoke("Resume_Down", resume_time);
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
}
