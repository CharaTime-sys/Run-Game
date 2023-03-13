using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    public int damage = -10;

    protected bool is_switched;
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.speed * Time.deltaTime);
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Changing_Status();
    }

    protected virtual void Changing_Status()
    {
        //改变自身状态
    }

    /// <summary>
    /// 测试分数
    /// </summary>
    public virtual void Test_Score()
    {
        //判断与手指的距离
        float distance = Mathf.Abs(Game_Controller.Instance.ninja.transform.position.z - transform.position.z);
        if (distance < Game_Controller.Instance.start_time * Game_Controller.Instance.speed)
        {
            if (distance < Game_Controller.Instance.prefect_time * Game_Controller.Instance.speed)
            {
                if (distance > Game_Controller.Instance.loss_time * Game_Controller.Instance.speed && Game_Controller.Instance.ninja.transform.position.z < transform.position.z)
                {
                    Game_Controller.Instance.Set_Score(20);
                    Game_Controller.Instance.Set_preference_Text("Prefect");
                    return;
                }
            }
            Game_Controller.Instance.Set_Score(10);
            Game_Controller.Instance.Set_preference_Text("Great");
        }
        //切换判断的对象
        if (transform.GetSiblingIndex() + 1 == transform.parent.childCount)
        {
            Game_Controller.Instance.Set_Once();
        }
        else
        {
            Game_Controller.Instance.cur_block = transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<Block>();
        }
    }
}
