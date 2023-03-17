using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public int damage = -10;

    protected bool is_switched;

    #region ����״̬����
    [SerializeField] protected bool if_great;
    [SerializeField] protected bool if_prefect;
    protected bool if_loss;
    [SerializeField] protected bool if_over;
    protected bool touched;

    public bool If_great { get => if_great;}
    #endregion
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.speed * Time.deltaTime);
        //û�õ��־ͻ���һ��
        if (if_loss && !touched)
        {
            touched = true;
            if (transform.GetSiblingIndex() + 1 != transform.parent.childCount)
            {
                Game_Controller.Instance.cur_block = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Block>();
            }
        }
        if (transform.position.z < 2)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Changing_Status();
        //�ϰ����Ƿ񳬹�����
        if (transform.position.z < Game_Controller.Instance.ninja.transform.position.z)
        {
            if_over = true;
        }
    }

    protected virtual void Changing_Status()
    {
        //�ı�����״̬
    }

    /// <summary>
    /// ���Է���
    /// </summary>
    public virtual void Test_Score()
    {
        //���ò�ͬ�÷ֱ�׼
        if (if_prefect)
        {
            Game_Controller.Instance.Set_Score(20);
            Game_Controller.Instance.Set_preference_Text("Prefect");
            //������Ч
            Game_Controller.Instance.Play_Effect(1);
        }
        else if (If_great)
        {
            Game_Controller.Instance.Set_Score(10);
            Game_Controller.Instance.Set_preference_Text("Great");
        }
        if (if_over || if_great || if_prefect)
        {
            //�л��жϵĶ���
            if ((transform.GetSiblingIndex() + 1 == transform.parent.childCount) || (GetComponent<Normal_Block>() != null && transform.GetSiblingIndex() + 2 == transform.parent.childCount))
            {
                Game_Controller.Instance.Set_Once();
            }
            else
            {
                if (GetComponent<Normal_Block>()!=null)
                {
                    Game_Controller.Instance.cur_block = transform.parent.GetChild(transform.GetSiblingIndex() + 2).GetComponent<Block>();
                }
                else
                {
                    Game_Controller.Instance.cur_block = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Block>();
                }
            }
        }
    }

    /// <summary>
    /// ���õ÷�ʧ��
    /// </summary>
    public void Set_loss()
    {
        if_loss = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Effect")
        {
            if (other.name.StartsWith("1"))
            {
                //���õ÷�״̬
                if_great = true;
                //����Ч��
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            else if (other.name.StartsWith("0"))
            {
                //���õ÷�״̬
                if_great = false;
                if_prefect = true;
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
