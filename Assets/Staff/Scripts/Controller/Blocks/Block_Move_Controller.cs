using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Move_Controller : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //�������ƶ�����ֹ��������
        if (transform.childCount == 0)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.speed * Time.deltaTime);
        }
    }
}
