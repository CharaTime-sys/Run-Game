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
        //地板移动
        transform.Translate(new Vector3(0, 0, -1) * Game_Controller.Instance.speed * Time.deltaTime);
        //让地板能够循环移动，小于一定距离就返回
        if (transform.position.z <=-51f)
        {
            transform.position = new Vector3(0, 0, 248.5f);
            Destroy_Block();
            Generate_Block();
        }
    }
    #region 砖块相关函数
    //生成砖块
    private void Generate_Block()
    {
        float temp_start_pos = Game_Controller.Instance.start_pos;
        //while (temp_start_pos < 5f)
        //{
        //    GameObject Block = Instantiate(Game_Controller.Instance.block, transform);
        //    //随机取一个位置参数
        //    Vector3 temp_pos = Game_Controller.Instance.block_pos[Random.Range(0, Game_Controller.Instance.block_pos.Length)];
        //    Block.transform.localPosition = new Vector3(temp_pos.x, temp_pos.y, temp_start_pos);
        //    temp_start_pos += Game_Controller.Instance.distance;//每一个间隔放一个预制体。（暂时每个障碍只有一个，后续可以考虑多个障碍共同作用）
        //}
    }
    //摧毁砖块
    private void Destroy_Block()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    #endregion
}
