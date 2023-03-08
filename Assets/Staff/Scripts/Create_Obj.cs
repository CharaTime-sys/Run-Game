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
        public float time;
        public int index;
        [Header("是否是划线")]
        public bool is_line;

        void Start()
        {
            // Register for Koreography Events.  This sets up the callback.
            Koreographer.Instance.RegisterForEvents(eventID, AddImpulse);
            Invoke("Set_Audio", time);
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
            if (is_line)
            {
                if (Line_Controller.Instance.nums == 4)
                {
                    Line_Controller.Instance.delta = 0.01f;
                }
                Line_Controller.Instance.Set_Line();
                return;
            }
            // Add impulse by overriding the Vertical component of the Velocity.
            GameObject _target_obj = null;
            if (index == 0)
            {
                _target_obj = Game_Controller.Instance.blocks[Random.Range(0, 3)];
            }
            else
            {
                _target_obj = Game_Controller.Instance.downs[0];
            }
            Debug.Log(_target_obj.name);
            GameObject block = Instantiate(_target_obj, Game_Controller.Instance.block_pos, Quaternion.identity);
            block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, target_obj.transform.position.z);
        }
        void Set_Audio()
        {
            audio_source.Play();
        }
    }
}
