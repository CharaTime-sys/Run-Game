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
            GameObject _buff = Block_Controller.Instance.Get_Block_Pos(Block_Controller.Instance.buffs[(int)buff_Type], Block_Controller.Instance.buff_pos);
            _buff.GetComponent<Buff_Block>().buff_Type = buff_Type;
            _buff.GetComponent<Buff_Block>().buff_time = (koreoEvent.EndSample - koreoEvent.StartSample) / 88200;
            //创造对应的物体
            Block_Controller.Instance.Create_Constant_Obj(_buff.transform.localPosition, (int)_buff.GetComponent<Buff_Block>().buff_time * 2,buff_Type);
            _buff.GetComponent<Buff_Block>().buff_time += 1f;
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
    }
}
