using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance;
    [Header("�ذ��ƶ��ٶ�")]
    public Material floor_material;
    public float speed;
    public float tile_speed;
    private float offset_y;
    public GameObject[] blocks;
    public GameObject[] downs;
    [Header("�ϰ������ɵ�λ�ò���")]
    public Vector3 block_pos;
    [Header("ÿ���ϰ���ļ�����루��ʱΪ��ֵ��")]
    public float distance;
    [Header("�ϰ��￪ʼ���ֵľ���")]
    public float start_pos = -4f;
    [Header("�����ֱ�")]
    public DynamicJoystick joystick;
    //�����˶�����
    //private float horizontal_move = 0f;
    //private float vertical_move = 0f;

    //��ָ���º�̧�������
    public List<Vector2> finger_start_pos;
    [Header("��ָ�ж��뾶")]
    public float finger_radious;
    public Vector2 test_vector;
    [Header("����")]
    public Ninja ninja;
    public LineRenderer line;
    [Header("�ж���")]
    [SerializeField] RectTransform check_line;
    [Header("�ж�������")]
    [SerializeField] float line_range;

    //UI���
    [SerializeField] Text grade_text;
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //�õ�ˮƽ����ֱ�ķ���
        //horizontal_move = joystick.Horizontal;
        //vertical_move = joystick.Vertical;
    }
    private void FixedUpdate()
    {
        offset_y += tile_speed * Time.deltaTime;
        floor_material.mainTextureOffset = new Vector2(0, offset_y);
    }
    public void Test_Direction()
    {
        //�õ�����ĽǶȣ�˳ʱ��
        float angle = Mathf.Atan(test_vector.y/test_vector.x) * Mathf.Rad2Deg;
        if (test_vector.x<0f && test_vector.y>0f)
        {
            angle = 90f+Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }
        else if (test_vector.x < 0f && test_vector.y < 0f)
        {
            angle = 180f+Mathf.Atan(test_vector.y / test_vector.x) * Mathf.Rad2Deg;
        }
        else if (test_vector.x > 0f && test_vector.y < 0f)
        {
            angle = 270f + Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }

        //���ݽǶȵ��ò�ͬ����
        if (angle < 45 || angle > 315)
        {
            ninja.Move_Left_And_Right(1);
        }
        else if (angle >135 && angle < 225)
        {
            ninja.Move_Left_And_Right(-1);
        }
        else if (angle > 45 && angle < 135)
        {
            ninja.Jump();
        }
        else if (angle > 225 && angle < 315)
        {
            ninja.Down();
        }
    }

    public void Test_Finger()
    {
        if (line.positionCount == 0)
        {
            return;
        }
        Vector2 line_screen_pos = Camera.main.WorldToScreenPoint(line.GetPosition(line.positionCount - 1));
        if (finger_start_pos.Count == 0)
        {
            Debug.Log("���֣���");
            return;
        }
        //Debug.Log("�߶�λ��"+line_screen_pos);
        //Debug.Log("��ָλ��"+ Input.touches[0].position);
        float distance = Mathf.Sqrt(Vector2.SqrMagnitude(line_screen_pos - Input.touches[0].position))/100;
        Debug.Log(distance);
        if (distance<=finger_radious)
        {
            Debug.Log("�ӷ֣���");
        }
        else
        {
            Debug.Log("���֣���");
        }
    }

    /// <summary>
    /// �ж��߰��µ���
    /// </summary>
    public void Test_Check_Line()
    {
        if (Mathf.Abs(finger_start_pos[0].y - check_line.position.y) <=line_range)
        {
            grade_text.text = "�ж��ɹ�";
        }
        else
        {
            grade_text.text = "�ж�ʧ��";
        }
    }
}
