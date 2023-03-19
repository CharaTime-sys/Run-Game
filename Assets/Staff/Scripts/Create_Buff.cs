//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2020 Sonic Bloom, LLC    
//----------------------------------------------

using System;
using UnityEngine;

namespace SonicBloom.Koreo.Demos
{
    public class Create_Buff : MonoBehaviour
    {
        [EventID]
        public string eventID;
        public GameObject target_obj;
        public AudioSource audio_source;
        [Header("buff类型")]
        public Buff_Type buff_Type;
        bool if_once = true;
        float cur_sample = 264600;

        void Start()
        {
            // Register for Koreography Events.  This sets up the callback.
            Koreographer.Instance.RegisterForEventsWithTime(eventID, AddImpulse);
        }

        void AddImpulse(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
        {
            if (cur_sample != koreoEvent.StartSample)
            {
                if_once = true;
                cur_sample = koreoEvent.StartSample;
            }
            if (!if_once)
            {
                return;
            }
            GameObject _buff = Get_Block_Pos(Game_Controller.Instance.buffs[(int)buff_Type], Game_Controller.Instance.buff_pos);
            _buff.GetComponent<Buff_Block>().buff_Type = buff_Type;
            _buff.GetComponent<Buff_Block>().buff_time = (koreoEvent.EndSample - koreoEvent.StartSample) / 88200;
            //创造对应的物体
            Create_Constant_Obj(_buff.transform.localPosition, (int)_buff.GetComponent<Buff_Block>().buff_time * 2);
            _buff.GetComponent<Buff_Block>().buff_time += 1.9f;
            if_once = false;
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

        /// <summary>
        /// 生成对应物体
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public GameObject Get_Block_Pos(GameObject obj,Vector3[] pos)
        {
            GameObject _block = Instantiate(obj, GameObject.Find("游戏必备/Blocks").transform);
            _block.transform.localPosition = pos[UnityEngine.Random.Range(0, pos.Length - 1)] + new Vector3(0,0, target_obj.transform.position.z);
            return _block;
        }

        /// <summary>
        /// 生成buff连续的物体
        /// </summary>
        void Create_Constant_Obj(Vector3 block_pos,int nums)
        {
            GameObject target_obj = null;
            Vector3[] target_pos = new Vector3[3];
            //生成对应的物体
            switch (buff_Type)
            {
                case Buff_Type.Jump:
                    target_obj = Game_Controller.Instance.buff_blocks[0];
                    target_pos = Game_Controller.Instance.buff_jump_pos;
                    break;
                case Buff_Type.Down:
                    target_obj = Game_Controller.Instance.buff_blocks[0];
                    target_pos = Game_Controller.Instance.buff_down_pos;
                    break;
                default:
                    break;
            }
            //目标z轴
            float target_z = block_pos.z + 5;
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
    }
}
