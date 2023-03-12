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
            Invoke("Set_Audio", Game_Controller.Instance.music_delay);
        }

        void AddImpulse(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
        {
            if (koreoEvent.GetValueOfCurveAtTime(sampleTime)<=0.003f)
            {
                GameObject _buff = Get_Block_Pos(Game_Controller.Instance.buffs[(int)buff_Type], Game_Controller.Instance.buff_pos);
                _buff.GetComponent<Buff_Block>().buff_Type = buff_Type;
                _buff.GetComponent<Buff_Block>().buff_time = (koreoEvent.EndSample - koreoEvent.StartSample) / 88200;
                //设置位置
                _buff.transform.position = new Vector3(_buff.transform.position.x, _buff.transform.position.y, target_obj.transform.position.z);
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
    }
}
