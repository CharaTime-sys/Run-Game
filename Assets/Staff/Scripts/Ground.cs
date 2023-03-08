using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Generate_Block();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //�ذ��ƶ�
        transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.speed * Time.deltaTime);
        //�õذ��ܹ�ѭ���ƶ���С��һ������ͷ���
        if (transform.position.z <=-51f)
        {
            transform.position = new Vector3(0, 0, 248.5f);
            Destroy_Block();
            Generate_Block();
        }
    }
    #region ש����غ���
    //����ש��
    private void Generate_Block()
    {
        float temp_start_pos = Game_Controller.Instance.start_pos;
        //while (temp_start_pos < 5f)
        //{
        //    GameObject Block = Instantiate(Game_Controller.Instance.block, transform);
        //    //���ȡһ��λ�ò���
        //    Vector3 temp_pos = Game_Controller.Instance.block_pos[Random.Range(0, Game_Controller.Instance.block_pos.Length)];
        //    Block.transform.localPosition = new Vector3(temp_pos.x, temp_pos.y, temp_start_pos);
        //    temp_start_pos += Game_Controller.Instance.distance;//ÿһ�������һ��Ԥ���塣����ʱÿ���ϰ�ֻ��һ�����������Կ��Ƕ���ϰ���ͬ���ã�
        //}
    }
    //�ݻ�ש��
    private void Destroy_Block()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    #endregion
}
