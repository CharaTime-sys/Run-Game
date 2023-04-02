using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //单例模式
    public static AudioManager instance;

    //背景音乐
    public AudioSource bgmSource;

    //音效
    public AudioSource sfxSource;

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
    }

    //停止背景音乐
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //播放音效
    public void PlaySFX(int index)
    {
        sfxSource.PlayOneShot(audioClips[index]);
    }
}
