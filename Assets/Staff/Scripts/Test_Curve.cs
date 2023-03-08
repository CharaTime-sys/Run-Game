using UnityEngine;
namespace SonicBloom.Koreo.Demos
{
    public class Test_Curve : MonoBehaviour
    {
        [EventID]
        public string eventID;

        private Vector3 pos;
        // Start is called before the first frame update
        void Start()
        {
            Koreographer.Instance.RegisterForEventsWithTime(eventID, AddTrans);
        }

        public void AddTrans(KoreographyEvent evt,int sampleTime,int sampleDelta,DeltaSlice deltaSlice)
        {
            //ºÏ≤‚’‚∂Œ“Ù∆µ «∑Ò”–«˙œﬂ
            if (evt.HasCurvePayload())
            {
                float curveValue = evt.GetValueOfCurveAtTime(sampleTime);

                transform.localScale = Vector3.one * curveValue;
            }
        }
    }
}
