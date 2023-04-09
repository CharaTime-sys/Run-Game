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
    [SerializeField] Material monster;
    [SerializeField] Mesh mesh;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        floor_onces = new bool[floor_parts.Count];//��ʼ���ذ���
    }
    public void Add_Speed()
    {
        floor_speed += floor_add_speed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Game_Controller.Instance.game_started)
        {
            return;
        }
        if (Create_Helper.Instance!=null)
        {
            Floor_Move();
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
        }
    }

    private IEnumerator Increase_Z(GameObject item)
    {
        //�ȴ��ذ��˶���
        yield return new WaitForSeconds(floor_down_time);
        //ѭ���ذ�
        item.transform.localPosition += new Vector3(0, 0, 92f);
        //��ʾ������
        foreach (Transform child in item.transform)
        {
            child.gameObject.SetActive(true);
        }
        //�ı�״̬
        floor_onces[floor_parts.IndexOf(item)] = false;
        //StopAllCoroutines();
    }

    [ContextMenu("�ӱ�ǩ")]
    public void Add_tag()
    {
        foreach (GameObject item in floor_parts)
        {
            Transform item_trans = item.transform;
            foreach (Transform _transform in item_trans)
            {
                if (_transform.position.y !=-1.25f)
                {
                _transform.transform.localPosition = new Vector3(_transform.transform.localPosition.x, -1.25f, _transform.localPosition.z);
                }
            }
        }
    }
}
