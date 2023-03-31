using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Controller : Global_Instance<Level_Controller>
{
    #region 关卡相关
    /// <summary>
    /// 加载相关关卡
    /// </summary>
    public void Load_Level(string level_name)
    {
        SceneManager.LoadScene(level_name);
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
