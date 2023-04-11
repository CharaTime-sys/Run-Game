using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Block : Block
{
    public Dir_Type dir_type;//手势类型
    public GameObject arrow;//箭头
    public GameObject circle;//范围
    public float raidus;//箭头
    public int[] indexs;
    public Vector3[] corners = new Vector3[4];//屏幕位置
    public float[] corner_num = new float[4];//数值 minx maxx miny maxy
    public int index;
    private void Start()
    {
        Init_max_and_min();
        Switch_Type();
    }
    protected override void Update()
    {
        base.Update();
        Refresh_Corners();
    }
    void Init_max_and_min()
    {
        corner_num[0] = 3000;
        corner_num[1] = 0;
        corner_num[2] = 3000;
        corner_num[3] = 0;
    }
    private void Refresh_Corners()
    {
        circle.GetComponent<RectTransform>().GetWorldCorners(corners);
        foreach (var item in corners)
        {
            Vector2 _item = Camera.main.WorldToScreenPoint(item);
            //找到大小值
            if (_item.x<corner_num[0])
            {
                corner_num[0] = _item.x;
            }
            if (_item.x > corner_num[1])
            {
                corner_num[1] = _item.x;
            }
            if (_item.y < corner_num[2])
            {
                corner_num[2] = _item.y;
            }
            if (_item.y > corner_num[3])
            {
                corner_num[3] = _item.y;
            }
        }
        if (if_over)
        {
            Game_Controller.Instance.Set_HP(damage);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 选择类型
    /// </summary>
    public void Switch_Type()
    {
        if (index == indexs.Length)
        {
            return;
        }
        switch (indexs[index])
        {
            case 1:
                dir_type = Dir_Type.Up;
                arrow.transform.localEulerAngles = new Vector3(-180, 0, 0);
                break;
            case 2:
                dir_type = Dir_Type.Down;
                arrow.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 3:
                dir_type = Dir_Type.Left;
                arrow.transform.localEulerAngles = new Vector3(90, 0, 0);
                break;
            case 4:
                dir_type = Dir_Type.Right;
                arrow.transform.localEulerAngles = new Vector3(-90, 0, 0);
                break;
            default:
                break;
        }
    }

    public override bool Test_Score(Dir_Type _dir_type,Vector2 finger_pos)
    {
        bool enable = false;
        if (finger_pos.x > corner_num[0] && finger_pos.x < corner_num[1]&& finger_pos.y > corner_num[2] && finger_pos.y < corner_num[3])
        {
            Debug.Log("手指在里面");
            if (_dir_type == dir_type)
            {
                Game_Controller.Instance.Set_Score(20);
                UI_Manager.Instance.Set_Status_UI("Prefect！");
                //播放音效
                AudioManager.instance.PlaySFX(1);
                index++;
            }
            enable = true;
        }
        if (index == indexs.Length)
        {
            Destroy(gameObject);
        }
        Switch_Type();
        return enable;
    }

    public override void OnTriggerEnter(Collider other)
    {

    }




    protected override void Ray_Cast()
    {

    }
    public override void OnDrawGizmos()
    {
    }
}
