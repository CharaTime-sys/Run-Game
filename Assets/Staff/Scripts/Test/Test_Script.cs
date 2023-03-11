using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Line_Controller))]
public class Test_Script : Editor
{
    public override void OnInspectorGUI()
    {
        Line_Controller line_Controller = (Line_Controller)target;
        GUILayout.Label("手势类型相关");
        line_Controller.type = (LineType)EditorGUILayout.EnumPopup("选择手势类型", line_Controller.type);
        switch (line_Controller.type)
        {
            case LineType.Circle:
                line_Controller.radious = EditorGUILayout.FloatField("圆半径", line_Controller.radious);
                break;
            case LineType.Half_Circle:
                line_Controller.radious = EditorGUILayout.FloatField("转弯半径", line_Controller.radious);
                line_Controller.end_length = EditorGUILayout.FloatField("结尾长度", line_Controller.end_length);
                break;
            default:
                break;
        }

        GUILayout.Label("手势参数相关");
        line_Controller.center = EditorGUILayout.Vector2Field("手势中心", line_Controller.center);
        line_Controller.delta = EditorGUILayout.FloatField("手势点数间距", line_Controller.delta);
        line_Controller.width = EditorGUILayout.FloatField("手势宽度", line_Controller.width);
    }
}
