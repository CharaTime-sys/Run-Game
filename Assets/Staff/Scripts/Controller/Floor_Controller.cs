using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floor_Controller : MonoBehaviour
{
    [Header("地板组块")]
    [SerializeField] List<GameObject> floor_parts;
    [SerializeField] bool[] floor_onces;//地板只能移动一次
    [Header("移动速度")]
    [SerializeField] float floor_speed;
    [Header("地板下沉和上升时间")]
    [SerializeField] float floor_down_time;
    [Header("地板下沉和上升时间")]
    [SerializeField] float floor_up_time;
    [Header("距离，x为下沉，y为上升")]
    [SerializeField] Vector2 floor_distance;
    private void Start()
    {
        floor_onces = new bool[floor_parts.Count];//初始化地板检测
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Floor_Move();
    }

    /// <summary>
    /// 地板移动
    /// </summary>
    private void Floor_Move()
    {
        foreach (var item in floor_parts)
        {
            item.transform.localPosition -= new Vector3(0, 0, floor_speed * Time.deltaTime);
            //回到原点
            if (item.transform.localPosition.z < -18.5f)
            {
                //让地板沉下去
                if (!floor_onces[floor_parts.IndexOf(item)])
                {
                    item.transform.DOLocalMoveY(item.transform.localPosition.y - floor_distance.x, floor_down_time);
                    //改变状态
                    floor_onces[floor_parts.IndexOf(item)] = true;
                    //增加地板坐标
                    StartCoroutine(Increase_Z(item));
                }
            }
        }
    }

    private IEnumerator Increase_Z(GameObject item)
    {
        //等待地板运动完
        yield return new WaitForSeconds(floor_down_time);
        //循环地板
        item.transform.localPosition += new Vector3(0, floor_distance.x + floor_distance.y, 105f);
        //显示子物体
        foreach (Transform child in item.transform)
        {
            child.gameObject.SetActive(true);
        }
        //改变状态
        floor_onces[floor_parts.IndexOf(item)] = false;
        //从上面降下来
        item.transform.DOLocalMoveY(item.transform.localPosition.y - floor_distance.y, floor_up_time);
        StopAllCoroutines();
    }

    [ContextMenu("加标签")]
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
