using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jump_Block : Block
{
    Material _material;

    //动画相关
    [SerializeField] float offset_y;
    [SerializeField] float offset_child_y;
    [SerializeField] float time = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Effect")
        {
            if (other.name.StartsWith("1"))
            {
                Set_Uping(true);
            }
            else if (other.name.StartsWith("0"))
            {
                Set_Uping(false);
            }
        }
        //让地面消失
        if (other.tag == "Ground")
        {
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 升起来
    /// </summary>
    void Set_Uping(bool if_self)
    {
        if (if_self)
        {
            transform.DOLocalMoveY(transform.localPosition.y + offset_y, time);
        }
        else
        {
            transform.GetChild(0).DOLocalMoveY(transform.GetChild(0).localPosition.y + offset_child_y, time);
        }
    }
}
