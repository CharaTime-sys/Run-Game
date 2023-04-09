using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    public static Block_Controller Instance;
    #region �ϰ���
    [Header("�ϰ������------------------------------------------")]
    [Header("�ϰ���Ԥ���壨��Ծ���»���ת��,buff,buff���壩")]
    public GameObject[] blocks;
    public GameObject[] block_extres;
    public GameObject[] downs;
    public GameObject[] down_extres;
    public GameObject[] turns;
    public GameObject[] buffs;
    public GameObject[] buff_blocks;
    #endregion

    #region λ�ò���
    [Header("�ϰ���λ�ñ���------------------------------------------")]
    [Header("�ϰ������ɵ�λ�ò���(��Ծ�ϰ����»��ϰ���ת���ϰ��������ϰ�)")]
    [Header("�ϰ����x��")]
    public float[] block_x_coords;
    public float jump_sec_x;
    [Header("��ͬ�ϰ���y��(jump,jump_extre,down1,down2,down3,down_extre,jump2")]
    public float[] block_y;
    [Header("�����ϰ���")]
    public Vector3[] turn_pos;
    public Vector3[] gesture_pos;
    public Vector3[] buff_pos;
    public Vector3[] buff_jump_pos;
    public Vector3[] buff_down_pos;
    #endregion

    [SerializeField] Transform colliders;
    //Ŀǰ���Ե��ϰ���
    public GameObject block_parent;
    public GameObject target_obj;
    bool if_once = true;//��ʼֻ��һ�μ���
    //����
    public bool If_once { get => if_once; }

    private void Awake()
    {
        Instance = this;
    }

    #region �����ϰ���
    /// <summary>
    /// ���ɶ�Ӧ����
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GameObject Get_Block_Pos(GameObject obj, Vector3[] pos)
    {
        GameObject block = Instantiate(obj, GameObject.Find("��Ϸ�ر�/Blocks").transform);
        //����λ��
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
        GameObject block = Instantiate(obj, GameObject.Find("��Ϸ�ر�/Blocks").transform);
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
        GameObject block = Instantiate(obj, GameObject.Find("��Ϸ�ر�/Blocks").transform);
        //����λ��
        block.transform.localPosition = pos;
        block.transform.localPosition += new Vector3(0, 0, target_obj.transform.localPosition.z);
        return block;
    }

    /// <summary>
    /// ����buff����������
    /// </summary>
    public void Create_Constant_Obj(Vector3 block_pos, int nums,Buff_Type buff_Type)
    {
        GameObject target_obj = null;
        Vector3[] target_pos = new Vector3[3];
        //���ɶ�Ӧ������
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
        //Ŀ��z��
        float target_z = block_pos.z + 10;
        for (int i = 0; i < nums; i++)
        {
            //�������ط�������
            for (int j = 0; j < 3; j++)
            {
                GameObject _block = Instantiate(target_obj, GameObject.Find("��Ϸ�ر�/Blocks").transform);
                _block.transform.localPosition = new Vector3(target_pos[j].x, target_pos[j].y, target_z);
            }
            target_z += Game_Controller.Instance.target_z;
        }
    }

    /// <summary>
    /// ������ͨ�ϰ���ĸ�������
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
            GameObject block = Instantiate(block_extres, GameObject.Find("��Ϸ�ر�/Blocks").transform);
            block.transform.localPosition = new Vector3(block_extre_pos[i],block_y[type],0);
            block.transform.localPosition += new Vector3(0, 0, target_obj.transform.localPosition.z);
        }
    }
    #endregion

    #region ������ײ��
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
