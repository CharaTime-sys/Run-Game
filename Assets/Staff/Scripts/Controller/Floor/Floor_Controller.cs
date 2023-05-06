using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floor_Controller : MonoBehaviour
{
    public static Floor_Controller Instance;
    [Header("�ذ����")]
    [SerializeField] List<GameObject> floor_parts;
    [SerializeField] bool[] floor_onces;//�ذ�ֻ���ƶ�һ��
    [Header("�ƶ��ٶ�")]
    [SerializeField] float floor_speed;
    [Header("�ƶ����ٶ�")]
    [SerializeField] float floor_add_speed;
    [Header("�ذ��³�������ʱ��")]
    [SerializeField] float floor_down_time;
    [Header("�ذ��³�������ʱ��")]
    [SerializeField] float floor_up_time;
    [Header("���룬xΪ�³���yΪ����")]
    [SerializeField] Vector2 floor_distance;
    [SerializeField] GameObject target_block;
    public Material material;
    public Mesh mesh;
    public bool is_entity;

    #region ��������
    [SerializeField] float end_speed;
    public bool end_start;
    #endregion
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        end_speed = floor_speed / 3;
        floor_onces = new bool[floor_parts.Count];//��ʼ���ذ���
    }
    public void Add_Speed()
    {
        end_speed = floor_speed / 3;
        floor_speed += floor_add_speed;
    }
    private void Update()
    {
        if (!Game_Controller.Instance.game_started)
        {
            return;
        }
        if (Create_Helper.Instance != null)
        {
            Floor_Move();
        }
        //�����Ϸʧ����ʼ����
        if (end_start)
        {
            Decrease_Speed();
        }
    }
    /// <summary>
    /// �ذ��ƶ�
    /// </summary>
    private void Floor_Move()
    {
        Create_Helper.Instance.Move();
        foreach (Transform item in transform)
        {
            item.localPosition -= new Vector3(0, 0, floor_speed * Time.deltaTime);
            if (item.position.z < -5f)
            {
                if (is_entity)
                {
                    StartCoroutine(Increase_Z(item.gameObject));
                }
            }
        }
    }

    public void Decrease_Speed()
    {
        floor_speed -= Time.deltaTime * end_speed;
        Game_Controller.Instance.speed -= Time.deltaTime * end_speed;
        if (floor_speed <= 0f)
        {
            end_start = false;
            Game_Controller.Instance.speed = 0f;
            Invoke(nameof(Game_End), 2f);
        }
    }

    public void Game_End()
    {
        Game_Controller.Instance.Set_End();
    }

    private IEnumerator Increase_Z(GameObject item)
    {
        //�ȴ��ذ��˶���
        yield return new WaitForSeconds(floor_down_time);
        //ѭ���ذ�
        item.transform.localPosition += new Vector3(0, 0, 88f);
        //��ʾ������
        foreach (Transform child in item.transform)
        {
            child.gameObject.SetActive(true);
        }
        //�ı�״̬
        floor_onces[floor_parts.IndexOf(item)] = false;
        StopAllCoroutines();
    }

    [ContextMenu("�ӱ�ǩ")]
    public void Add_tag()
    {
        foreach (Transform item in transform)
        {
            foreach (Transform _item in item)
            {
                if (_item.GetComponent<MeshRenderer>() != null)
                {
                    _item.GetComponent<MeshRenderer>().material = material;
                    //_item.GetComponent<MeshFilter>().mesh = mesh;
                    //_item.GetComponent<BoxCollider>().size = new Vector3(0.2f, 0.2f, 0.2f);
                    //_item.GetComponent<BoxCollider>().center = Vector3.zero;
                    //_item.localScale = new Vector3(20, 20, 20);
                    //_item.localPosition = new Vector3(_item.localPosition.x, -3f, _item.localPosition.z);
                    //_item.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    //if (_item.childCount !=0)
                    //{
                    //    _item.GetChild(0).localPosition = new Vector3(0f, 0.0941f, -0.0437f);
                    //}
                    //if (_item.childCount == 2)
                    //{
                    //    _item.GetChild(1).localScale = new Vector3(0.03f, 0.03f, 0.03f);
                    //    _item.GetChild(1).localPosition = new Vector3(0f, 0.2f, 0f);
                    //}
                }
            }
        }
        //foreach (Transform item in transform)
        //{
        //    if (item.name.Contains("jump_sec"))
        //    {
        //        item.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = monster;
        //        item.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = monster;
        //        item.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material = monster_1;
        //    }
        //if (item.name.Contains("Buff"))
        //{
        //    item.GetChild(0).GetComponent<MeshRenderer>().material = monster;
        //}
        //if (item.name.Contains("Down 1") || item.name.Contains("Jump"))
        //{
        //    item.GetChild(0).GetComponent<MeshRenderer>().material = monster;
        //    item.GetChild(1).GetComponent<MeshRenderer>().material = monster_1;
        //}
        //if (item.name.Contains("Monster 2"))
        //{
        //    item.GetComponent<MeshRenderer>().material = monster_3;
        //    item.GetChild(0).GetComponent<MeshRenderer>().material = monster_4;
        //}
        //if (item.name.Contains("Monster 1"))
        //{
        //    item.GetComponent<MeshRenderer>().material = monster_2;
        //    item.GetChild(0).GetComponent<MeshRenderer>().material = monster_4;
        //}
        //if (item.name.Contains("ground"))
        //{
        //    item.GetComponent<MeshRenderer>().material = monster;
        //    item.GetChild(0).GetComponent<MeshRenderer>().material = monster_1;
        //}
    }
}
