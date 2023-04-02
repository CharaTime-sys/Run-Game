using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Controller : Global_Instance<Level_Controller>
{
    #region ���س������
    public GameObject load_canvas;
    public Slider load_slider;
    #endregion
    #region �ؿ����
    /// <summary>
    /// ������عؿ�
    /// </summary>
    public void Load_Level(string level_name)
    {
        StartCoroutine(Load_Level_Async(level_name));
    }
    IEnumerator Load_Level_Async(string level_name)
    {
        load_canvas.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(level_name);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            load_slider.value = operation.progress;
            if (operation.progress >= 0.9f)
            {
                load_slider.value = 1;
                if (Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                    load_canvas.SetActive(false);
                }
            }
            yield return null;
        }
    }
    /// <summary>
    /// �˳���Ϸ
    /// </summary>
    public void Exit_Game()
    {
        Application.Quit();
    }
    #endregion
}
