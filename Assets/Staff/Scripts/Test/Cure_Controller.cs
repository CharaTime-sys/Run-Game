using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
/// <summary>
/// 手势类型
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
    //目前的手势跟踪物体
    [SerializeField] SplineFollower splineFollower;
    //曲线物体
    [SerializeField] SplineRenderer splinecomputer;
    [Header("跟踪半径")]
    public float follow_radious;
    private void Awake()
    {
        Instance = this;
    }
}
