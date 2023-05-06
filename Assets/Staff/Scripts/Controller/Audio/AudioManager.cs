using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //单例模式
    public static AudioManager instance;

    //背景音乐
    public AudioSource bgmSource;

    //音效
    public AudioSource sfxSource;

    //音效
    public AudioSource startSource;

    //时间音效播放器
    public AudioSource eventSource;

    //音效集合
    [SerializeField] AudioClip[] audioClips;

    public AudioClip background_bgm;
    //获取单例
    private void Awake()
    {
        instance = this;
    }

    //播放背景音乐
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
        bgmSource.UnPause();
    }
    public void PlayBGM()
    {
        bgmSource.Play();
    }
    
    public void PlayEvent()
    {
        eventSource.UnPause();
    }
    //停止背景音乐
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StopEvent()
    {
        eventSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void PauseSFX()
    {
        sfxSource.Pause();
    }

    public void PauseEvent()
    {
        eventSource.Pause();
    }

    //播放音效
    public void PlaySFX(int index)
    {
        sfxSource.clip = audioClips[index];
        sfxSource.PlayOneShot(audioClips[index]);
    }
    public void PlaySFX()
    {
        sfxSource.Play();
    }
    public void Playstart()
    {
        startSource.Play();
    }
    public void Pausestart()
    {
        startSource.Pause();
    }
}
