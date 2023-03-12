using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve_Follow : MonoBehaviour
{
    /// <summary>
    /// 得到曲线在屏幕中的开始位置
    /// </summary>
    /// <returns></returns>
    public Vector2 Curve_Point()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
}
