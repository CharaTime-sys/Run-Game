//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2020 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;

namespace SonicBloom.Koreo.Demos
{
    public class Create_Obj : MonoBehaviour
    {
        [EventID]
        public string eventID;
        public AudioSource audio_source;
        [Header("0代表跳跃障碍，1代表下滑障碍，2代表转向障碍，手势就不用管索引")]
        public int index;
        [Header("转向的索引集合(哪个不需要产生)")]
        public int[] indexs;
        //目前的索引
        int cur_index = 0;
        [Header("是否是手势")]
        public bool is_line;
        int down_previous = 0;
        void OnEnable()
        {
            // Register for Koreography Events.  This sets up the callback.
            Koreographer.Instance.RegisterForEvents(eventID, AddImpulse);
        }
        void OnDisable()
        {
            // Sometimes the Koreographer Instance gets cleaned up before hand.
            //  No need to worry in that case.
            if (Koreographer.Instance != null)
            {
                Koreographer.Instance.UnregisterForAllEvents(this);
            }
        }

        void AddImpulse(KoreographyEvent evt)
        {
            //如果是手势的话就到这里来判断
            if (is_line)
            {
                switch (Cure_Controller.Instance.curve_Type)
                {
                    case Curve_Type.Up:
                        GameObject curve = Instantiate(Cure_Controller.Instance.curves[0], Block_Controller.Instance.gesture_pos[0], Quaternion.identity);
                        curve.transform.SetParent(GameObject.Find("Curves").transform);
                        break;
                    case Curve_Type.Right:
                        break;
                    case Curve_Type.Left:
                        break;
                    case Curve_Type.Circle:
                        break;
                    case Curve_Type.Down:
                        break;
                    default:
                        break;
                }
                return;
            }
            // Add impulse by overriding the Vertical component of the Velocity.
            GameObject _target_obj = null;
            GameObject block = null;
            int _index = Random.Range(0, Block_Controller.Instance.block_x_coords.Length);//取得随机一列的值
            int obj_index = 0;
            float temp_y = 0;
            //生成对应的物体
            switch (index)
            {
                case 0:
                    obj_index = Random.Range(0, Block_Controller.Instance.blocks.Length);
                    _target_obj = Block_Controller.Instance.blocks[obj_index];//得到随机的跳跃障碍
                    float temp_x = 0;
                    switch (obj_index)
                    {
                        case 0:
                            temp_y = Block_Controller.Instance.block_y[0];
                            temp_x = Block_Controller.Instance.block_x_coords[_index];
                            break;
                        case 1:
                            temp_y = Block_Controller.Instance.block_y[6];
                            temp_x = Block_Controller.Instance.jump_sec_x;
                            _index = 0;
                            break;
                        default:
                            break;
                    }
                    block = Block_Controller.Instance.Get_Block_Pos(_target_obj, new Vector3(temp_x, temp_y,0));
                    //temping not to create monster
                    Block_Controller.Instance.Create_Constant_Obj(Block_Controller.Instance.block_extres[0], Block_Controller.Instance.block_x_coords, _index, 1);
                    break;
                case 1:
                    obj_index = Random.Range(0, Block_Controller.Instance.downs.Length);
                    if (down_previous == 1 && obj_index == 1)
                    {
                        obj_index = 0;
                    }
                    down_previous = obj_index;
                    _target_obj = Block_Controller.Instance.downs[obj_index];
                    switch (obj_index)
                    {
                        case 0:
                            temp_y = Block_Controller.Instance.block_y[2];
                            break;
                        case 1:
                            temp_y = Block_Controller.Instance.block_y[3];
                            break;
                        case 2:
                            temp_y = Block_Controller.Instance.block_y[4];
                            break;
                        default:
                            break;
                    }
                    block = Block_Controller.Instance.Get_Block_Pos(_target_obj, new Vector3(Block_Controller.Instance.block_x_coords[_index], temp_y, 0));
                    //temping not to create monster
                    Block_Controller.Instance.Create_Constant_Obj(Block_Controller.Instance.down_extres[0], Block_Controller.Instance.block_x_coords, _index, 5);
                    break;
                case 2:
                    Block_Controller.Instance.Create_Constant_Obj(Block_Controller.Instance.block_extres[0], Block_Controller.Instance.block_x_coords, _index, 5);
                    _target_obj = Block_Controller.Instance.turns[Random.Range(0, Block_Controller.Instance.turns.Length - 1)];
                    //temping not to create monster
                    block = Block_Controller.Instance.Get_Block_Pos(_target_obj, Block_Controller.Instance.turn_pos);
                    break;
                default:
                    break;
            }
            block.GetComponent<Block>()._index = _index - 1;
            if (Block_Controller.Instance.If_once)
            {
                Block_Controller.Instance.Add_Block_To_Current(block.GetComponent<Block>());
            }
        }
    }
}
