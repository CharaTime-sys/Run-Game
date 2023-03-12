using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
/// <summary>
/// ��������
/// </summary>
public enum Curve_Type
{
    Up,
    Right,
    Left,
    Circle,
    Down
}
public class Cure_Controller : MonoBehaviour
{
    public static Cure_Controller Instance;
    public Curve_Type curve_Type;
    public GameObject[] curves;
    //Ŀǰ�����Ƹ�������
    [SerializeField] SplineFollower splineFollower;
    //��������
    [SerializeField] SplineRenderer splinecomputer;
    [Header("���ٰ뾶")]
    public float follow_radious;
    private void Awake()
    {
        Instance = this;
    }
}
