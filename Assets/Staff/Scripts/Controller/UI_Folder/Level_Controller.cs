using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Controller : Global_Instance<Level_Controller>
{
    #region 加载场景相关
    public GameObject load_canvas;
    public Slider load_slider;
    public int level_index = -1;
    public bool could_press = true;
    #endregion
    #region 关卡相关
    /// <summary>
    /// 加载相关关卡
    /// </summary>
    public void Load_Level(string level_name)
    {
        if (!could_press)
        {
            return;
        }
        StartCoroutine(Load_Level_Async(level_name));
    }
    public void Load_Level(string level_name,int level_index)
    {
        if (!could_press)
        {
            return;
        }
        this.level_index = level_index;
        StartCoroutine(Load_Level_Async(level_name));
    }
    public void Load_Level(int level_choose)
    {
        if (!could_press)
        {
            return;
        }
        string name = "";
        switch (level_index)
        {
            case 1:
                name += "Level_One";
                break;
            case 2:
                name += "Level_Two";
                break;
            case 3:
                name += "Level_Three";
                break;
            default:
                break;
        }
        switch (level_choose)
        {
            case 1:
                name += "Normal";
                break;
            case 2:
                name += "Entity";
                break;
            default:
                break;
        }
        StartCoroutine(Load_Level_Async(name));
    }

    IEnumerator Load_Level_Async(string level_name)
    {
        could_press = false;
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
                    load_canvas.SetActive(false);
                    operation.allowSceneActivation = true;
                    Invoke(nameof(Set_Pressed),0.5f);
                }
            }
            yield return null;
        }
    }

    private void Set_Pressed()
    {
        could_press = true;
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void Exit_Game()
    {
        Application.Quit();
    }
    #endregion
}
