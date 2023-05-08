using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public enum Buff_Type
{
    Jump,
    Down
}

public enum Dir_Type
{
    None,
    Left,
    Right,
    Up,
    Down
}
public class Game_Controller : MonoBehaviour
{
    public static Game_Controller Instance;
    [Header("物体开始的延迟")]
    public float staff_delay;
    [Header("无尽模式间隔")]
    public float level_delay;
    [Header("音乐开始的延迟")]
    public float music_delay;
    public bool start_game = false;
    #region 速度变量
    //设置材质速度，后面可以删除
    public Material floor_material;
    [Header("速度变量------------------------------------------")]
    [Header("障碍物移动速度")]
    public float speed;
    [Header("障碍物移动加速度")]
    public float add_speed;
    [Header("曲线移动速度")]
    public float curve_speed;
    #endregion

    #region 物体
    [Header("控制手柄")]
    [SerializeField] DynamicJoystick joystick;
    [Header("人物")]
    public Ninja ninja;
    public GameObject normal_curve;
    #endregion

    #region 状态变量
    [Header("手指状态（不用管）------------------------------------------")]
    //是否正在按下
    public bool is_pressing;
    public bool is_buffing;
    //判断是否是长按后的跳跃，防止手指放开的时候跳跃
    public bool is_jump_after;
    //是否到达了ui点
    public bool is_reached;
    //按下状态
    public bool pressed;
    public bool pressed_once;
    public bool game_started;
    public bool game_started_gesture = false;//开始的手势变量
    #endregion

    #region 时间变量
    [Header("开始判定的时间")]
    public float start_time;
    [Header("完美时间")]
    public float prefect_time;
    [Header("失败时间")]
    public float loss_time;
    [SerializeField] float pause_time;
    #endregion

    [Header("悬空时间")]
    [SerializeField] float buff_time;
    [Header("手指到ui的最大范围")]
    [SerializeField] float buff_max_distance;
    float buff_distance;
    //buff类型
    [SerializeField] Buff_Type buff_Type;

    #region 手势位置
    [Header("手势变量(不用管)------------------------------------------")]
    //手指按下和抬起的坐标
    public Vector2 finger_start_pos;
    public int buff_index = -1;
    public Vector2 test_vector;//手势方向
    #endregion

    Vector3 target_pos;//用于长按的判断
    Vector3 press_pos;//目前的手指位置
    public float target_z;

    #region 属性
    public Vector3 Press_pos { get => press_pos; }
    public Buff_Type Buff_Type { get => buff_Type; }
    #endregion

    //临时变量
    float offset_y;
    bool end_once;
    public bool if_pause = false;

    [SerializeField] GameObject startup;
    [SerializeField] SonicBloom.Koreo.Demos.Create_Obj[] musics;
    [SerializeField] SonicBloom.Koreo.Demos.Create_Buff[] music_buffs;
    [SerializeField] SimpleMusicPlayer simpleMusicPlayer;
    #region 画布相关
    [SerializeField] GameObject over_panel;
    [SerializeField] Button over_btn;
    [SerializeField] Button[] score_btns;
    [SerializeField] GameObject score_panel;
    [SerializeField] GameObject load_slider;
    #endregion
    private void OnApplicationFocus(bool focus)
    {
    }
    private void Awake()
    {
        Instance = this;
        Set_Over_Canvas();
        Set_Score_Canvas();
    }
    private void Set_Over_Canvas()
    {
        over_btn.onClick.AddListener(delegate { Level_Controller.Instance.Load_Level("Start"); });
        over_panel.GetComponent<Button>().onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(); });
    }
    private void Set_Score_Canvas()
    {
        score_btns[0].onClick.AddListener(delegate { Level_Controller.Instance.Load_Level(); });
        score_btns[1].onClick.AddListener(delegate { Level_Controller.Instance.Load_Level_Next(); });
        score_btns[2].onClick.AddListener(delegate { Level_Controller.Instance.Return_To_Choose(0); });
    }

    public void Game_Start()
    {
        game_started = true;
        AudioManager.instance.Playstart();
        if (load_slider!=null)
        {
            load_slider.SetActive(true);
        }
        Invoke(nameof(Set_Staff), staff_delay);
        ninja.GetComponent<Animator>().enabled = true;
        UI_Manager.Instance.Set_Tip(false, null);
    }
    public void Game_End()
    {
        if (end_once)
        {
            return;
        }
        AudioManager.instance.StopBGM();
        AudioManager.instance.StopSFX();
        AudioManager.instance.StopEvent();
        Floor_Controller.Instance.end_start = true;
        end_once = true;
        CancelInvoke();
    }
    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void Pause_Game()
    {
        if (normal_curve != null)
        {
            return;
        }
        if (!if_pause)
        {
            AudioManager.instance.PauseBGM();
            AudioManager.instance.PauseSFX();
            AudioManager.instance.Pausestart();
            AudioManager.instance.PauseEvent();
            Bubble_Controller.Instance.Pause_Invoke_Bubbles();
            UI_Manager.Instance.Set_Pause_Panel(true);
            game_started = false;
            ninja.GetComponent<Animator>().speed = 0;
            CancelInvoke();
            if_pause = !if_pause;
        }
        else
        {
            UI_Manager.Instance.Set_Pause_Panel(false);
            if (!UI_Manager.Instance.countdown_text.gameObject.active)
            {
                UI_Manager.Instance.Set_Count_Down();
            }
        }
    }

    public void Set_Pause_Bool()
    {
        if_pause = !if_pause;
    }

    public void Pause_Start()
    {
        if (pause_time < staff_delay)
        {
            Invoke(nameof(Set_Staff), staff_delay - pause_time);
            AudioManager.instance.Playstart();
        }
        UI_Manager.Instance.Set_Pause_Panel(false);
        ninja.GetComponent<Animator>().speed = 1;
        game_started = true;
        AudioManager.instance.PlayBGM();
        AudioManager.instance.PlayEvent();
        Bubble_Controller.Instance.Start_Bubbles();
    }

    public void Game_Win()
    {
        Score_Controller.Instance.Set_Max_Score(ninja.Score);
        Score_Controller.Instance.if_start_add = true;
        AudioManager.instance.StopBGM();
        AudioManager.instance.StopSFX();
        game_started = false;
        if (score_panel!=null)
        {
            score_panel.SetActive(true);
        }
        UI_Manager.Instance.Set_Main_UI();
        CancelInvoke();
    }
    public void Set_End()
    {
        game_started = false;
        UI_Manager.Instance.Set_Main_UI();
        if (Level_Controller.Instance!=null && Level_Controller.Instance.Get_Difficult("Entity"))
        {
            Score_Controller.Instance.Set_Max_Score(ninja.Score);
            Score_Controller.Instance.if_start_add = true;
            if (score_panel != null)
            {
                score_panel.SetActive(true);
            }
            CancelInvoke();
        }
        else
        {
            over_panel.SetActive(true);
        }
    }
    #region 音乐相关

    void Set_Staff()
    {
        startup.SetActive(true);
        foreach (var item in musics)
        {
            item.enabled = true;
        }
        foreach (var item in music_buffs)
        {
            item.enabled = true;
        }
        Invoke(nameof(Set_Audio), music_delay);
        Invoke(nameof(Disable_Staff), music_delay+startup.GetComponent<AudioSource>().clip.length);
        AudioManager.instance.eventSource.gameObject.SetActive(true);
        simpleMusicPlayer.Play();
        Add_Floor_Speed();
    }

    void Add_Floor_Speed()
    {
        speed += add_speed;
        Floor_Controller.Instance.Add_Speed();
    }

    public void Disable_Staff()
    {
        startup.SetActive(false);
        foreach (SonicBloom.Koreo.Demos.Create_Obj item in musics)
        {
            item.enabled = false;
        }
        foreach (SonicBloom.Koreo.Demos.Create_Buff item in music_buffs)
        {
            item.enabled = false;
        }
        if (Level_Controller.Instance!=null && Level_Controller.Instance.Get_Difficult("Entity"))
        {
            Invoke(nameof(Set_Staff), level_delay);
        }
    }

    /// <summary>
    /// 播放音乐
    /// </summary>
    void Set_Audio()
    {
        AudioManager.instance.PlayBGM(AudioManager.instance.background_bgm);
    }

    #endregion

    #region 设置属性
    /// <summary>
    /// 设置血量
    /// </summary>
    /// <param name="hp"></param>
    public void Set_HP(int hp)
    {
        ninja.Hp += hp;
    }

    /// <summary>
    /// 设置分数
    /// </summary>
    /// <param name="score"></param>
    public void Set_Score(int score)
    {
        ninja.Score += score;
    }

    /// <summary>
    /// 设置buff时间和类型
    /// </summary>
    public void Set_buff_time_And_type(float target_time, Buff_Type buff_Type)
    {
        this.buff_time = target_time;
        this.buff_Type = buff_Type;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        //if (SceneManager.GetActiveScene().name.Contains("Three"))
        //{
        //    Debug.Log("第三关是横屏");
        //}
        //else
        //{
        //    Debug.Log("其他是竖屏");
        //    Screen.orientation = ScreenOrientation.Portrait;
        //}
        if (is_pressing)
        {
            //得到手指位置
            if (Input.touches.Length!=0 && buff_index!=-1)
            {
                press_pos = Input.touches[buff_index].position;
            }
            Test_target_UI();
            //得到手指和buffui的距离
            buff_distance = Vector3.Distance(press_pos, target_pos);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Game_Start();
        }
        Buff_Pressed();
        //暂停时间设置
        if (pause_time < staff_delay && game_started)
        {
            pause_time += Time.deltaTime;
        }
    }

    /// <summary>
    /// 寻找目标ui的位置
    /// </summary>
    void Test_target_UI()
    {
        switch (Buff_Type)
        {
            case Buff_Type.Jump:
                target_pos = UI_Manager.Instance.jump_buff_ui.transform.position;
                break;
            case Buff_Type.Down:
                target_pos = UI_Manager.Instance.down_buff_ui.transform.position;
                break;
            default:
                break;
        }
    }

    public Dir_Type Test_Direction()
    {
        bool level_three = Camera.main.name.Contains("Three");
        //得到具体的角度，顺时针
        float angle = Mathf.Atan(test_vector.y/test_vector.x) * Mathf.Rad2Deg;
        if (test_vector.x<0f && test_vector.y>0f)
        {
            angle = 90f+Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }
        else if (test_vector.x < 0f && test_vector.y < 0f)
        {
            angle = 180f+Mathf.Atan(test_vector.y / test_vector.x) * Mathf.Rad2Deg;
        }
        else if (test_vector.x > 0f && test_vector.y < 0f)
        {
            angle = 270f + Mathf.Atan(-test_vector.x / test_vector.y) * Mathf.Rad2Deg;
        }
        //根据角度调用不同方法,加入侧面的判断
        if (angle < 45 || angle > 315)
        {
            if (level_three)
            {
                return Dir_Type.None;
            }
            return Dir_Type.Right;
        }
        else if (angle >135 && angle < 225)
        {
            if (level_three)
            {
                return Dir_Type.Down;
            }
            return Dir_Type.Left;
        }
        else if (angle > 45 && angle < 135)
        {
            if (level_three)
            {
                return Dir_Type.Left;
            }
            return Dir_Type.Up;
        }
        else if (angle > 225 && angle < 315)
        {
            if (level_three)
            {
                return Dir_Type.Right;
            }
            return Dir_Type.Down;
        }
        return Dir_Type.Up;
    }

    public void Change_Character(Dir_Type dir_Type)
    {
        switch (dir_Type)
        {
            case Dir_Type.None:
                break;
            case Dir_Type.Left:
                ninja.Move_Left_And_Right(-1);
                break;
            case Dir_Type.Right:
                ninja.Move_Left_And_Right(1);
                break;
            case Dir_Type.Up:
                ninja.Jump(true);
                break;
            case Dir_Type.Down:
                ninja.Down(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 判断按下的状态
    /// </summary>
    public void Press_Checked(bool enable)
    {
        is_pressing = enable;
    }

    /// <summary>
    /// 按下跳跃时悬空
    /// </summary>
    void Buff_Pressed()
    {
        if (pressed_once)
        {
            //如果正在按下并且手指的距离在buffui的范围内则能腾空
            if (is_pressing && buff_distance <= buff_max_distance)
            {
                //当玩家具有跳跃或下蹲buff
                if (ninja.Is_buffing)
                {
                    //不让玩家完成后返回地面
                    switch (Buff_Type)
                    {
                        case Buff_Type.Jump:
                            ninja.Jump(false);
                            break;
                        case Buff_Type.Down:
                            ninja.Down(false);
                            break;
                        default:
                            break;
                    }
                    //到达了标识点
                    is_reached = true;
                    is_buffing = true;
                }
            }
            //当手指到达了之后，时间消失或者手指放开或者距离过大
            if ((is_reached && (buff_time <= 0f || buff_distance >= buff_max_distance)) || !is_pressing)
            {
                Return_NonBuff();
            }
        }
        //当玩家没有buff时，防止时间一直减少
        if (ninja.Is_buffing&&buff_time >0f)
        {
            buff_time -= Time.deltaTime;
        }
        if (buff_time<=0f)
        {
            buff_time = 0f;
            ninja.Set_Buff_Status(false);
        }
    }

    private void Return_NonBuff()
    {
        //取消玩家buff状态
        ninja.Is_buffing = false;
        is_buffing = false;
        ninja.Set_Buff_Status(false);
        buff_time = 0f;
        //落地
        switch (Buff_Type)
        {
            //只有当玩家确实按到了按钮才会返回跳跃，不然就是普通的跳跃
            case Buff_Type.Jump:
                if (is_reached)
                {
                    ninja.Resume_Jump();
                }
                break;
            case Buff_Type.Down:
                if (is_reached)
                {
                    ninja.Resume_Down();
                }
                break;
        }
        //取消到达ui状态
        is_reached = false;
        //设置长按跳跃状态
        if (is_pressing)
        {
            is_jump_after = true;
        }
        pressed_once = false;
    }

    /// <summary>
    /// 测试跳跃还是滑行，改变ui
    /// </summary>
    public void Check_Down_And_Jump()
    {
        if (!ninja.Is_buffing)
        {
            return;
        }
        switch (Buff_Type)
        {
            case Buff_Type.Jump:
                UI_Manager.Instance.Set_Jump_UI(true);
                break;
            case Buff_Type.Down:
                UI_Manager.Instance.Set_Down_UI(true);
                break;
            default:
                break;
        }
    }

    public void Set_Score_Staff(int score = 20,string status_content = "Prefect！", bool if_play = true, int clip_index = 1)
    {
        Set_Score(score);
        UI_Manager.Instance.Set_Status_UI(status_content);
        //播放音效
        if (if_play)
        {
            AudioManager.instance.PlaySFX(clip_index);
        }
    }
}
