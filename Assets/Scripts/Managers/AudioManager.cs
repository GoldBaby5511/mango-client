using UnityEngine;
using System.Collections;

/// <summary>
/// 音频管理类
/// </summary>
public class AudioManager : MonoBehaviour 
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        
        get {
            
            if (_instance == null)
            {
                string name = "GameManager";
                GameObject manager = GameObject.Find("GameManager");
                if (manager == null)
                {
                    manager = new GameObject(name);
                    _instance = manager.AddComponent<AudioManager>();
                }
                else
                {
                    _instance = manager.GetComponent<AudioManager>();
                    if (_instance == null)
                    {
                        _instance = manager.AddComponent<AudioManager>();
                    }
                }
            }
            
            return _instance; 
        }
    }

    private AudioSource audioSound;     //音效播放器
    private AudioSource audioMusic;     //背景音乐播放
    private AudioSource audioLanguage;  //配音播放器


    /// <summary>
    /// 是否播放背景音乐
    /// </summary>
    public bool IsPlayMusic
    {
        get { return !audioMusic.mute; }
        set 
        {
            audioMusic.mute = !value;
            PlayerPrefs.SetInt("isMusicOpen", value ? 1 : 0);
        }
    }

    /// <summary>
    /// 是否播放音效
    /// </summary>
    public bool IsPlaySound
    {
        get { return !audioSound.mute; }
        set 
        {
            audioSound.mute = !value;
            PlayerPrefs.SetInt("isSoundOpen", value ? 1 : 0);
        }
    }

    /// <summary>
    /// 是否播放配音
    /// </summary>
    public bool IsPlayLanguage
    {
        get { return !audioLanguage.mute; }
        set 
        {
            audioLanguage.mute = !value;
            PlayerPrefs.SetInt("isLanguageOpen", value ? 1 : 0);
        }
    }

    /// <summary>
    /// 背景音乐大小
    /// </summary>
    public float MusicVolume
    {
        get { return audioMusic.volume; }
        set { audioMusic.volume = value; }
    }

    /// <summary>
    /// 音效大小
    /// </summary>
    public float SoundVolume
    {
        get { return audioSound.volume; }
        set { audioSound.volume = value; }
    }

    public float LanguageVolume
    {
        get { return audioLanguage.volume; }
        set { audioLanguage.volume = value; }
    }

    /// <summary>
    /// 是否循环播放背景音乐
    /// </summary>
    private bool isLoop = true;
    public bool IsLoop
    {
        get { return audioMusic.loop; }
        set { audioMusic.loop = value; }
    }

    void Awake()
    {
        audioSound = gameObject.AddComponent<AudioSource>();
        audioMusic = gameObject.AddComponent<AudioSource>();
        audioLanguage = gameObject.AddComponent<AudioSource>();

        audioMusic.loop = true;
        audioMusic.playOnAwake = false;
        Init();
    }

    void Init()
    {
        this.IsPlayMusic = (PlayerPrefs.GetInt("isMusicOpen", 1) > 0);
        this.IsPlaySound = (PlayerPrefs.GetInt("isSoundOpen", 1) > 0);
        this.IsPlayLanguage = (PlayerPrefs.GetInt("isLanguageOpen", 1) > 0);
    }


    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayMusic(AudioClip clip,bool isLoop = true)
    {
        if (clip == null)
        {
            return;
        }

        if (!isLoop)
        {
            audioMusic.loop = false;
            audioMusic.Stop();
            audioMusic.Play();
        }

        if (audioMusic.isPlaying && clip.name == audioMusic.clip.name)
        {
            return;
        }
        audioMusic.loop = true;
        audioMusic.clip = clip;
        audioMusic.Play();
    }

    /// <summary>
    /// 停止播放音乐
    /// </summary>
    public void StopMusic()
    {
        audioMusic.clip = null;
        audioMusic.Stop();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        audioSound.PlayOneShot(clip);
    }

    /// <summary>
    /// 播放配音
    /// </summary>
    public void PlayLanguage(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }
        audioLanguage.PlayOneShot(clip);
    }
}
