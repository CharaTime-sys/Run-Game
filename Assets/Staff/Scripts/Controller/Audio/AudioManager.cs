using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //����ģʽ
    public static AudioManager instance;

    //��������
    public AudioSource bgmSource;

    //��Ч
    public AudioSource sfxSource;

    //��Ч
    public AudioSource startSource;

    //ʱ����Ч������
    public AudioSource eventSource;

    //��Ч����
    [SerializeField] AudioClip[] audioClips;

    public AudioClip background_bgm;
    //��ȡ����
    private void Awake()
    {
        instance = this;
    }

    //���ű�������
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
    //ֹͣ��������
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

    //������Ч
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
