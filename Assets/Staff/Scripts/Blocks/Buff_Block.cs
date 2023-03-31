using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Block : Block
{
    public float buff_time;//buffʱ��
    public Buff_Type buff_Type;//��Ծbuff����

    protected override void Update()
    {
        base.Update();
        Ray_Cast();
    }

    protected override void Ray_Cast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 2))
        {
            //�����߼�⵽����ʱ
            if (hit.transform.GetComponent<Ninja>()!=null)
            {
                Game_Controller.Instance.Set_buff_time_And_type(buff_time, buff_Type);
                Game_Controller.Instance.ninja.Set_Buff_Status(true);
                return;
            }
            //hit.transform.gameObject.SetActive(false);
        };
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.up * 2F, Color.red);
    }
}
