using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Controller : Global_Instance<Level_Controller>
{
    #region �ؿ����
    /// <summary>
    /// ������عؿ�
    /// </summary>
    public void Load_Level(string level_name)
    {
        SceneManager.LoadScene(level_name);
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
