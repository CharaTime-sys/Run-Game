using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LineType
{
    Circle,

}

public class Line_Controller : MonoBehaviour
{
    public static Line_Controller Instance;
    [Header("����")]
    public LineType type;
    public float radious;
    public Vector2 circle_center;
    [Header("�ߵĵ�������")]
    public float delta;
    [Header("��ʼ�뾶")]
    public float start_width;
    public LineRenderer line;
    private int numClicks = 0;
    public float delete_time = 0.4f;
    public float start_delete_time = 0.4f;
    public int nums;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Set_Line();
    }

    public void Set_Line()
    {
        //�����߶�
        line.startWidth = start_width;
        line.endWidth = start_width;
        for (float i = 0; i <= 360; i += delta)
        {
            line.positionCount = numClicks + 1;
            //����������һ��Բ�Σ���ͬ����״�����ò�ͬ�ĺ���
            line.SetPosition(numClicks, new Vector3(circle_center.x + radious * Mathf.Cos(i / Mathf.Rad2Deg), circle_center.y + radious * Mathf.Sin(i / Mathf.Rad2Deg), 12));
            numClicks++;
        }
        numClicks = 0;
        //��n��������ʧ����
        InvokeRepeating("Delete_Point", start_delete_time, delete_time);
        nums++;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    [ContextMenu("���ٵ�")]
    public void Delete_Point()
    {
        if (line.positionCount == 0)
        {
            CancelInvoke();
            return;
        }
        line.positionCount--;
        Game_Controller.Instance.Test_Finger();
    }
}
