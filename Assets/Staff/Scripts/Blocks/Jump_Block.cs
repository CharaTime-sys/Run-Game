using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jump_Block : Block
{
    Material _material;

    bool is_shining;

    [Header("闪烁间隔")]
    [SerializeField] float delta = 0.3f;
    float timer = 0.3f;//计时器

    //动画相关
    [SerializeField] float offset_y;
    [SerializeField] float time;
    // Start is called before the first frame update
    void Start()
    {
        _material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    protected override void Changing_Status()
    {
        //闪烁状态
        if (is_shining)
        {
            if (timer >= 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // 设置材质
                if (is_switched)
                {
                    _material.SetColor("_EmissionColor", Color.black);
                }
                else
                {
                    _material.SetColor("_EmissionColor", Color.red);
                }
                is_switched = !is_switched;
                timer = delta;//重置计时器
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Effect")
        {
            if (other.name.StartsWith("3"))
            {
                is_shining = true;//设置改变状态
            }
            else if (other.name.StartsWith("1"))
            {
                is_shining = false;
                _material.SetColor("_EmissionColor", Color.black);
                Set_Uping();
            }
            else if (other.name.StartsWith("0"))
            {
                is_shining = false;
                _material.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    void Set_Uping()
    {
        transform.DOLocalMoveY(transform.localPosition.y + offset_y, time);
    }
}
