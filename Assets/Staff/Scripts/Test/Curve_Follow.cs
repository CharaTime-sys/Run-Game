using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve_Follow : MonoBehaviour
{
    /// <summary>
    /// �õ���������Ļ�еĿ�ʼλ��
    /// </summary>
    /// <returns></returns>
    public Vector2 Curve_Point()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
}
