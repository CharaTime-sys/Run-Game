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
        public GameObject target_obj;
        public AudioSource audio_source;
        [Header("0代表跳跃障碍，1代表下滑障碍，2代表转向障碍，手势就不用管索引")]
        public int index;
        [Header("是否是手势")]
        public bool is_line;

        void Start()
        {
            // Register for Koreography Events.  This sets up the callback.
            Koreographer.Instance.RegisterForEvents(eventID, AddImpulse);
            Invoke("Set_Audio", Game_Controller.Instance.music_delay);
        }

        void OnDestroy()
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
                        GameObject curve = Instantiate(Cure_Controller.Instance.curves[0], Game_Controller.Instance.gesture_pos[0], Quaternion.identity);
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
            //生成对应的物体
            switch (index)
            {
                case 0:
                    _target_obj = Game_Controller.Instance.blocks[Random.Range(0, Game_Controller.Instance.blocks.Length - 1)];
                    block = Get_Block_Pos(_target_obj, Game_Controller.Instance.block_pos);
                    break;
                case 1:
                    _target_obj = Game_Controller.Instance.downs[Random.Range(0, Game_Controller.Instance.downs.Length - 1)];
                    block = Get_Block_Pos(_target_obj, Game_Controller.Instance.down_pos);
                    break;
                case 2:
                    _target_obj = Game_Controller.Instance.turns[Random.Range(0, Game_Controller.Instance.turns.Length - 1)];
                    block = Get_Block_Pos(_target_obj, Game_Controller.Instance.turn_pos);
                    break;
                default:
                    break;
            }
            //设置位置
            block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, target_obj.transform.position.z);
        }
        void Set_Audio()
        {
            audio_source.Play();
        }

        /// <summary>
        /// 生成对应物体
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public GameObject Get_Block_Pos(GameObject obj,Vector3[] pos)
        {
            return Instantiate(obj, pos[Random.Range(0, pos.Length - 1)], Quaternion.identity);
        }
    }
}
