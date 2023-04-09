using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    public static Block_Controller Instance;
    #region 障碍物
    [Header("障碍物变量------------------------------------------")]
    [Header("障碍物预制体（跳跃，下滑，转向,buff,buff物体）")]
    public GameObject[] blocks;
    public GameObject[] block_extres;
    public GameObject[] downs;
    public GameObject[] down_extres;
    public GameObject[] turns;
    public GameObject[] buffs;
    public GameObject[] buff_blocks;
    #endregion

    #region 位置参数
    [Header("障碍物位置变量------------------------------------------")]
    [Header("障碍物生成的位置参数(跳跃障碍，下滑障碍，转向障碍，手势障碍)")]
    [Header("障碍物的x轴")]
    public float[] block_x_coords;
    public float jump_sec_x;
    [Header("不同障碍物y轴(jump,jump_extre,down1,down2,down3,down_extre,jump2")]
    public float[] block_y;
    [Header("其他障碍物")]
    public Vector3[] turn_pos;
    public Vector3[] gesture_pos;
    public Vector3[] buff_pos;
    public Vector3[] buff_jump_pos;
    public Vector3[] buff_down_pos;
    #endregion

    [SerializeField] Transform colliders;
    //目前测试的障碍物
    public GameObject block_parent;
    public GameObject target_obj;
    bool if_once = true;//开始只有一次加入
    //属性
    public bool If_once { get => if_once; }

    private void Awake()
    {
        Instance = this;
    }

    #region 生成障碍物
    /// <summary>
    /// 生成对应物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameObject Get_Block_Pos(GameObject obj, Vector3[] pos)
    {
        GameObject block = Instantiate(obj, GameObject.Find("游戏必备/Blocks").transform);
        //设置位置
        int _index = Random.Range(0, pos.Length);
        block.transform.localPosition = pos[_index];
        if (_index == 0 && block.GetComponent<Normal_Block>() != null)
        {
            block.GetComponent<Normal_Block>().is_left = true;
        }
        block.transform.localPosition += new Vector3(0, 0, target_obj.transform.localPosition.z);
        return block;
    }

    public GameObject Get_Block_Pos(GameObject obj, Vector3[] pos,int _index)
    {
        GameObject block = Instantiate(obj, GameObject.Find("游戏必备/Blocks").transform);
        block.transform.localPosition = pos[_index];
        if (_index == 0 && block.GetComponent<Normal_Block>() != null)
        {
            block.GetComponent<Normal_Block>().is_left = true;
        }
        block.transform.localPosition += new Vector3(0, 0, target_obj.transform.localPosition.z);
        return block;
    }

    public GameObject Get_Block_Pos(GameObject obj, Vector3 pos)
    {
        GameObject block = Instantiate(obj, GameObject.Find("游戏必备/Blocks").transform);
        //设置位置
        block.transform.localPosition = pos;
        block.transform.localPosition += new Vector3(0, 0, target_obj.transform.localPosition.z);
        return block;
    }

    /// <summary>
    /// 生成buff连续的物体
    /// </summary>
    public void Create_Constant_Obj(Vector3 block_pos, int nums,Buff_Type buff_Type)
    {
        GameObject target_obj = null;
        Vector3[] target_pos = new Vector3[3];
        //生成对应的物体
        switch (buff_Type)
        {
            case Buff_Type.Jump:
                target_obj = Block_Controller.Instance.buff_blocks[0];
                target_pos = Block_Controller.Instance.buff_jump_pos;
                break;
            case Buff_Type.Down:
                target_obj = Block_Controller.Instance.buff_blocks[0];
                target_pos = Block_Controller.Instance.buff_down_pos;
                break;
            default:
                break;
        }
        //目标z轴
        float target_z = block_pos.z + 10;
        for (int i = 0; i < nums; i++)
        {
            //在三个地方都生成
            for (int j = 0; j < 3; j++)
            {
                GameObject _block = Instantiate(target_obj, GameObject.Find("游戏必备/Blocks").transform);
                _block.transform.localPosition = new Vector3(target_pos[j].x, target_pos[j].y, target_z);
            }
            target_z += Game_Controller.Instance.target_z;
        }
    }

    /// <summary>
    /// 生成普通障碍物的附属物体
    /// </summary>
    /// <param name="block_extre_pos"></param>
    public void Create_Constant_Obj(GameObject block_extres,float[]block_extre_pos,int ignore,int type)
    {
        for (int i = 0; i < block_extre_pos.Length; i++)
        {
            if (i == ignore)
            {
                continue;
            }
            GameObject block = Instantiate(block_extres, GameObject.Find("游戏必备/Blocks").transform);
            block.transform.localPosition = new Vector3(block_extre_pos[i],block_y[type],0);
            block.transform.localPosition += new Vector3(0, 0, target_obj.transform.localPosition.z);
        }
    }
    #endregion

    #region 生成碰撞体
    public void Create_Collider(int _index)
    {
        for (int i = 0; i < colliders.childCount; i++)
        {
            colliders.GetChild(i).gameObject.SetActive(false);
            if (i != _index)
            {
                colliders.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    #endregion
}
