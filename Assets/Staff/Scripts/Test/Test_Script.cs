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
        GUILayout.Label("�����������");
        line_Controller.type = (LineType)EditorGUILayout.EnumPopup("ѡ����������", line_Controller.type);
        switch (line_Controller.type)
        {
            case LineType.Circle:
                line_Controller.radious = EditorGUILayout.FloatField("Բ�뾶", line_Controller.radious);
                break;
            case LineType.Half_Circle:
                line_Controller.radious = EditorGUILayout.FloatField("ת��뾶", line_Controller.radious);
                line_Controller.end_length = EditorGUILayout.FloatField("��β����", line_Controller.end_length);
                break;
            default:
                break;
        }

        GUILayout.Label("���Ʋ������");
        line_Controller.center = EditorGUILayout.Vector2Field("��������", line_Controller.center);
        line_Controller.delta = EditorGUILayout.FloatField("���Ƶ������", line_Controller.delta);
        line_Controller.width = EditorGUILayout.FloatField("���ƿ��", line_Controller.width);
    }
}
