using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LineType
{
    Circle,
    Half_Circle,
}

public class Line_Controller : MonoBehaviour
{
    public static Line_Controller Instance;
    #region 类型相关
    [Header("类型")]
    public LineType type;
    //圆
    [Header("半径")]
    public float radious;
    //转弯
    //结尾长度
    public float end_length;
    #endregion
    #region 点相关
    public float delta = 1f;
    public float width;
    public Vector2 center;//手势中心
    public LineRenderer line;
    int numClicks = 0;//计数相关
    #endregion
    //临时变量
    Vector2 cur_point_pos;//现在的点
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        Reset_Width();
        Set_Line();
    }
    
    /// <summary>
    /// 重置宽度
    /// </summary>
    void Reset_Width()
    {
        line.startWidth = width;
        line.endWidth = width;
    }

    [ContextMenu("生成线段")]
    public void Set_Line()
    {
        switch (type)
        {
            case LineType.Circle:
                for (float i = 0; i <= 361; i += delta)
                {
                    line.positionCount = numClicks + 1;
                    line.SetPosition(numClicks, new Vector3(center.x + radious * Mathf.Cos(i / Mathf.Rad2Deg), center.y + radious * Mathf.Sin(i / Mathf.Rad2Deg), 0));
                    numClicks++;
                }
                break;
            case LineType.Half_Circle:
                for (float i = end_length; i >= 0; i-=delta)
                {
                    line.positionCount = numClicks + 1;
                    line.SetPosition(numClicks, new Vector3(center.x + i, center.y + radious, 0));
                    numClicks++;
                }
                for (float i = 90; i <= 270; i += delta)
                {
                    line.positionCount = numClicks + 1;
                    line.SetPosition(numClicks, new Vector3(center.x + radious * Mathf.Cos(i / Mathf.Rad2Deg), center.y + radious * Mathf.Sin(i / Mathf.Rad2Deg), 0));
                    numClicks++;
                }
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (line.positionCount == 0)
        {
            //当画完之后调用方法
            return;
        }
        cur_point_pos = line.GetPosition(numClicks-1);
        cur_point_pos += new Vector2(960f, 2130f);
        cur_point_pos *= new Vector2(1080f/960f, 1200f / 2130f);
        Debug.Log("线段的点："+cur_point_pos);
        if (Input.touchCount ==0)
        {
            return;
        }
        Debug.Log("目前的距离："+ Vector3.Distance(Input.touches[0].position, cur_point_pos));
        if (Vector3.Distance(Input.touches[0].position,cur_point_pos) <= Game_Controller.Instance.Finger_radious)
        {
            line.positionCount--;
            numClicks--;
        }
    }
    [ContextMenu("删除手势")]
    public void Delete_Gesture()
    {
        line.positionCount = 0;
        //Game_Controller.Instance.Test_Finger();
    }
}
