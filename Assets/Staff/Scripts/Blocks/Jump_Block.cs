using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jump_Block : Block
{
    Material _material;

    bool is_shining;

    [Header("��˸���")]
    [SerializeField] float delta = 0.3f;
    float timer = 0.3f;//��ʱ��

    //�������
    [SerializeField] float offset_y;
    [SerializeField] float time;
    // Start is called before the first frame update
    void Start()
    {
        _material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }

    protected override void Changing_Status()
    {
        //��˸״̬
        if (is_shining)
        {
            if (timer >= 0f)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // ���ò���
                if (is_switched)
                {
                    _material.SetColor("_EmissionColor", Color.black);
                }
                else
                {
                    _material.SetColor("_EmissionColor", Color.red);
                }
                is_switched = !is_switched;
                timer = delta;//���ü�ʱ��
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
                is_shining = true;//���øı�״̬
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
