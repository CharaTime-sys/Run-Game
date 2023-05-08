using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public int damage = -30;

    protected bool is_switched;

    #region ����״̬����
    [SerializeField] protected bool if_great;
    [SerializeField] protected bool if_prefect;
    protected bool if_loss;
    public bool if_end = false;
    [SerializeField] protected bool if_over;
    protected bool touched;
    public bool If_great { get => if_great;}
    [SerializeField] Dir_Type dir_Type;
    bool hit_once;
    #endregion
    //���
    [SerializeField] Animator animator;
    //ί��
    System.Action<Block> deactivateAction;

    public int _index;
    protected virtual void Start()
    {
        Ray_Cast();
    }
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //û�õ��־ͻ���һ��
        if (if_loss && !touched)
        {
            touched = true;
        }
        if (transform.position.z < -5)
        {
            //deactivateAction.Invoke(this);
            Destroy(gameObject);
        }
    }

    protected virtual void Update()
    {
        if(!Game_Controller.Instance.game_started)
        {
            return;
        }
        Discriminat_Statue();
        Set_Translate();
        Changing_Status();
        //�ϰ����Ƿ񳬹�����
        if (transform.position.z < Game_Controller.Instance.ninja.transform.position.z)
        {
            if_over = true;
            Set_Collider();
        }
        //ÿ֡�������жϣ�ֻ�е�������ײ��ⷶΧ��ʱ��Ż��е÷��ж�
        Set_Buff_Hit();
    }

    void Set_Buff_Hit()
    {
        if (GetComponent<Monster_Block>()!=null || hit_once || GetComponent<Buff_Block>()!=null)
        {
            return;
        }
        if (Game_Controller.Instance.is_buffing)
        {
            if (if_prefect || if_over)
            {
                Game_Controller.Instance.Set_Score_Staff(20);
                Do_Ani("return");
                Set_Collider();
                hit_once = true;
            }
        }
    }

    public virtual void Set_Collider()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    private void Set_Translate()
    {
        if (transform.localEulerAngles.y == 90)
        {
            transform.Translate(new Vector3(1, 0, 0) * Game_Controller.Instance.speed * Time.deltaTime);
        }
        else if (transform.localEulerAngles.y == 180)
        {
            transform.Translate(new Vector3(0, 0, 1) * Game_Controller.Instance.speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.speed * Time.deltaTime);
        }
    }

    protected virtual void Changing_Status()
    {
        //�ı�����״̬
    }

    /// <summary>
    /// ���Է���
    /// </summary>
    public virtual bool Test_Score(Dir_Type _dir_type,Vector2 finger_pos)
    {
        if (_index != Game_Controller.Instance.ninja.Dir_component)
        {
            Debug.Log("���ʧ��");
            Debug.Log(Game_Controller.Instance.ninja.Dir_component);
            return false;
        }
        //���������ҽ����жϣ���Ϊ���Ҷ����Գɹ�
        if (!(dir_Type == Dir_Type.Left || dir_Type == Dir_Type.Right))
        {
            if (dir_Type != _dir_type)
            {
                return false;
            }
        }
        else
        {
            if (!(_dir_type != Dir_Type.Left || _dir_type != Dir_Type.Right))
            {
                Debug.Log(_dir_type);
                Debug.Log("���ʧ��");
                return false;
            }
        }
        if (if_loss)
        {
            return false;
        }
        Debug.Log("���Խ��м�⣡");
        //���ò�ͬ�÷ֱ�׼
        if (!if_over && if_prefect)
        {
            Game_Controller.Instance.Set_Score_Staff(20, "Prefect��");
        }
        else if (If_great)
        {
            Game_Controller.Instance.Set_Score_Staff(20, "Great��",false);
        }
        if (if_over || if_great || if_prefect)
        {
            if (!if_over)
            {
                Do_Ani("return");
            }
            Set_Collider();
        }
        return false;
    }

    /// <summary>
    /// һЩ����
    /// </summary>
    public virtual void Do_Ani(string name)
    {
        if (animator == null)
        {
            return;
        }
        animator.Play(name);
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
        //�����ϰ������������ֹ����ڲ�ͬ������������ȴ�з���
        if (other.tag == "Border")
        {
            _index = -1;
            if (other.name.EndsWith("2"))
            {
                _index = 1;
            }
        }
    }

    protected virtual void Ray_Cast()
    {
        RaycastHit hit;
        Ray[] ray = {
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(0, 0, 8), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(0, 0, 4), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(0, 0, 12), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(3, 0, 8), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(3, 0, 4), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(3, 0, 12), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(-3,0, 8), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(-3,0, 4), Vector3.down * 5f),
            new Ray(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(-3,0, 12), Vector3.down * 5f),
            };
        foreach (Ray item in ray)
        {
            if (Physics.Raycast(item, out hit))
            {
                if (hit.transform.GetComponent<Base_block>() != null)
                {
                    hit.transform.GetComponent<Base_block>().could_ani = true;
                }
            }
        }
    }

    public void SetDeactivateAction(System.Action<Block> deactivateAction)
    {
        this.deactivateAction = deactivateAction;
    }

    public virtual void OnDrawGizmos()
    {
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z) - new Vector3(0,0,8), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(0,0,4), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(0,0,12), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(3, 0, 8), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(3, -0, 4), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(3, -0, 12), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(-3, -0, 8), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f,transform.position.z)- new Vector3(-3, -0, 4), Vector3.down * 5f, Color.red);
        Debug.DrawRay(new Vector3(3.2f, -5.68f, transform.position.z) - new Vector3(-3, -0, 12), Vector3.down * 5f, Color.red);
    }

    public void Discriminat_Statue()
    {
        float line_distance = Mathf.Abs(transform.position.z - Block_Data.Instance.discriminant_line.position.z);
        if (line_distance <=Block_Data.Instance.min_and_max_distance.x)//�����ж�
        {
            if_great = false;
            if_prefect = true;
            if (Game_Controller.Instance.is_buffing)
            {
                Test_Score(dir_Type, Vector2.zero);
            }
            Debug.Log("������");
        }
        else if (line_distance <= Block_Data.Instance.min_and_max_distance.y)//great�ж�
        {
            if_great = true;
        }
        else if (line_distance <= Block_Data.Instance.min_and_max_distance.z)//�����ж�
        {
            Do_Ani("start");//��ʼ����
        }
    }
}
