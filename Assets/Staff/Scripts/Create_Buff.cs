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

        void Start()
        {
            // Register for Koreography Events.  This sets up the callback.
            Koreographer.Instance.RegisterForEventsWithTime(eventID, AddImpulse);
        }

        void AddImpulse(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
        {
            if (koreoEvent.GetValueOfCurveAtTime(sampleTime)<=0.003f)
            {
                GameObject _buff = Get_Block_Pos(Game_Controller.Instance.buffs[(int)buff_Type], Game_Controller.Instance.buff_pos);
                _buff.transform.SetParent(GameObject.Find("游戏必备/Blocks").transform);
                _buff.GetComponent<Buff_Block>().buff_Type = buff_Type;
                _buff.GetComponent<Buff_Block>().buff_time = (koreoEvent.EndSample - koreoEvent.StartSample) / 88200;
                //设置位置
                _buff.transform.position = new Vector3(_buff.transform.position.x, _buff.transform.position.y, target_obj.transform.position.z);
                //创造对应的物体
                Create_Constant_Obj(_buff.transform.position, (int)_buff.GetComponent<Buff_Block>().buff_time);
            }
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
            return Instantiate(obj, pos[UnityEngine.Random.Range(0, pos.Length - 1)], Quaternion.identity);
        }

        /// <summary>
        /// 生成buff连续的物体
        /// </summary>
        void Create_Constant_Obj(Vector3 block_pos,int nums)
        {
            GameObject target_obj = null;
            Vector3 target_pos = Vector3.zero;
            //生成对应的物体
            switch (buff_Type)
            {
                case Buff_Type.Jump:
                    target_obj = Game_Controller.Instance.blocks[0];
                    target_pos += Game_Controller.Instance.block_pos[0];
                    break;
                case Buff_Type.Down:
                    target_obj = Game_Controller.Instance.downs[0];
                    target_pos += Game_Controller.Instance.down_pos[0];
                    break;
                default:
                    break;
            }
            //目标z轴
            float target_z = block_pos.z;
            for (int i = 0; i < nums; i++)
            {
                target_z += Game_Controller.Instance.target_z;
                Instantiate(target_obj, target_pos + new Vector3(0, 0, target_z), Quaternion.identity,GameObject.Find("游戏必备/Blocks").transform);
            }
        }
    }
}
