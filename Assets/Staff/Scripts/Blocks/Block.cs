using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public int damage = -10;

    protected bool is_switched;

    #region 方块状态变量
    [SerializeField] protected bool if_great;
    [SerializeField] protected bool if_prefect;
    protected bool if_loss;
    public bool if_end = false;
    [SerializeField] protected bool if_over;
    protected bool touched;
    public bool If_great { get => if_great;}
    [SerializeField] Dir_Type dir_Type;
    #endregion
    //组件
    [SerializeField] Animator animator;
    //委托
    System.Action<Block> deactivateAction;
    private void Start()
    {
        Ray_Cast();
    }
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //没得到分就换下一个
        if (if_loss && !touched)
        {
            touched = true;
        }
        if (transform.position.z < 0)
        {
            //deactivateAction.Invoke(this);
            Destroy(gameObject);
        }
    }

    protected virtual void Update()
    {
        if (!Game_Controller.Instance.game_started)
        {
            return;
        }
        transform.Translate(Vector3.back * Game_Controller.Instance.speed * Time.deltaTime);
        Changing_Status();
        //障碍物是否超过人物
        if (transform.position.z < Game_Controller.Instance.ninja.transform.position.z)
        {
            if (Block_Controller.Instance.cur_block == gameObject.GetComponent<Block>())
            {
                Turn_Next();
            }
            if_over = true;
        }
    }

    protected virtual void Changing_Status()
    {
        //改变自身状态
    }

    /// <summary>
    /// 测试分数
    /// </summary>
    public virtual void Test_Score(Dir_Type _dir_type)
    {
        if (if_over || if_great || if_prefect)
        {
            //切换判断的对象
            Turn_Next();
        }
        if (dir_Type !=_dir_type)
        {
            return;
        }
        //设置不同得分标准
        if (!if_over && if_prefect)
        {
            Game_Controller.Instance.Set_Score(20);
            //播放音效
            AudioManager.instance.PlaySFX(1);
        }
        else if (If_great)
        {
            Game_Controller.Instance.Set_Score(10);
        }
        if (if_over || if_great || if_prefect)
        {
            if (!if_over)
            {
                Do_Ani("return");
            }
            if_end = true;
        }
    }

    /// <summary>
    /// 一些动画
    /// </summary>
    public virtual void Do_Ani(string name)
    {
        if (animator == null)
        {
            return;
        }
        animator.Play(name);
    }

    public void Turn_Next()
    {
        if ((transform.GetSiblingIndex() + 1 == transform.parent.childCount) || (GetComponent<Normal_Block>() != null&& transform.GetSiblingIndex() + 1 == transform.parent.childCount))
        {
            Block_Controller.Instance.Set_Once();
        }
        else
        {
            Block_Controller.Instance.cur_block = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Block>();
        }
    }

    /// <summary>
    /// 设置得分失败
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
                //设置得分状态
                if_great = true;
            }
            else if (other.name.StartsWith("0"))
            {
                //设置得分状态
                if_great = false;
                if_prefect = true;
            }
            else if (other.name.StartsWith("3"))
            {
                Do_Ani("start");
            }
        }
        if (other.tag == "Ground")
        {
            other.gameObject.SetActive(false);
        }
    }

    protected virtual void Ray_Cast()
    {

    }

    public void SetDeactivateAction(System.Action<Block> deactivateAction)
    {
        this.deactivateAction = deactivateAction;
    }
}
