using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floor_Controller : MonoBehaviour
{
    [Header("�ذ����")]
    [SerializeField] List<GameObject> floor_parts;
    [SerializeField] bool[] floor_onces;//�ذ�ֻ���ƶ�һ��
    [Header("�ƶ��ٶ�")]
    [SerializeField] float floor_speed;
    [Header("�ذ��³�������ʱ��")]
    [SerializeField] float floor_down_time;
    [Header("�ذ��³�������ʱ��")]
    [SerializeField] float floor_up_time;
    [Header("���룬xΪ�³���yΪ����")]
    [SerializeField] Vector2 floor_distance;
    private void Start()
    {
        floor_onces = new bool[floor_parts.Count];//��ʼ���ذ���
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Floor_Move();
    }

    /// <summary>
    /// �ذ��ƶ�
    /// </summary>
    private void Floor_Move()
    {
        foreach (var item in floor_parts)
        {
            item.transform.localPosition -= new Vector3(0, 0, floor_speed * Time.deltaTime);
            //�ص�ԭ��
            if (item.transform.localPosition.z < -18.5f)
            {
                //�õذ����ȥ
                if (!floor_onces[floor_parts.IndexOf(item)])
                {
                    item.transform.DOLocalMoveY(item.transform.localPosition.y - floor_distance.x, floor_down_time);
                    //�ı�״̬
                    floor_onces[floor_parts.IndexOf(item)] = true;
                    //���ӵذ�����
                    StartCoroutine(Increase_Z(item));
                }
            }
        }
    }

    private IEnumerator Increase_Z(GameObject item)
    {
        //�ȴ��ذ��˶���
        yield return new WaitForSeconds(floor_down_time);
        //ѭ���ذ�
        item.transform.localPosition += new Vector3(0, floor_distance.x + floor_distance.y, 105f);
        //��ʾ������
        foreach (Transform child in item.transform)
        {
            child.gameObject.SetActive(true);
        }
        //�ı�״̬
        floor_onces[floor_parts.IndexOf(item)] = false;
        //�����潵����
        item.transform.DOLocalMoveY(item.transform.localPosition.y - floor_distance.y, floor_up_time);
        StopAllCoroutines();
    }

    [ContextMenu("�ӱ�ǩ")]
    public void Add_tag()
    {
        foreach (var item in floor_parts)
        {
            Transform parent = item.transform;
            foreach (Transform child in parent)
            {
                child.gameObject.AddComponent<BoxCollider>();
                child.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            }
        }
    }
}
