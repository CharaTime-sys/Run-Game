using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //����ģʽ
    public static AudioManager instance;

    //��������
    public AudioSource bgmSource;

    //��Ч
    public AudioSource sfxSource;

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
    }
    [ContextMenu("����ʱ��")]
    public void Debug_Music()
    {
        Debug.Log(audioClips[4].length);
    }
    //ֹͣ��������
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    //������Ч
    public void PlaySFX(int index)
    {
        sfxSource.PlayOneShot(audioClips[index]);
    }
}
