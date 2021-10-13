using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<SoundManager>();
        }
        if (instance == null)
        {
            instance = new GameObject("SoundManager").AddComponent<SoundManager>();
        }
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField]
    private SoundDatabase bgmDB, soundEffectDB;
    [SerializeField]
    private AudioSource bgm;

    [SerializeField]
    private AudioMixerGroup bgmMixer, sfxMixer;

    private List<AudioSource> sfxList = new List<AudioSource>();

    public static AudioClip GetBgm(string _bgm)
    {
        return instance.bgmDB.GetSound(_bgm);
    }

    public static AudioClip GetSoundFx(string _sfx)
    {
        return instance.soundEffectDB.GetSound(_sfx);
    }

    //배경음 재생
    public static void PlayBgm(AudioClip _clip)
    {
        instance.bgm.clip = _clip;
        instance.bgm.Play();
    }

    //효과음 재생
    public static void PlaySfx(AudioClip _clip)
    {
        var audio = instance.GetSfxAudio();
        audio.clip = _clip;
        audio.Play();
        //자리 없으면 오디오소스 추가
    }

    private AudioSource GetSfxAudio()
    {

        //남는 효과음 자리 있으면
        foreach (var _audio in instance.sfxList)
        {
            if (!_audio.isPlaying)
            {
                return _audio;
            }
        }
        //없으면 새 효과음 추가
        var newObj = new GameObject("SfxSource" + sfxList.Count);
        newObj.transform.SetParent(transform);
        var audio = newObj.AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = sfxMixer;
        sfxList.Add(audio);

        return audio;
    }

    public void BgmVolume(float value)
    {
        bgmMixer.audioMixer.SetFloat("Bgm",Mathf.Log10(value) * 20);
        Debug.Log(Mathf.Log10(value));
    }
    public void SfxVolume(float value)
    {
        bgmMixer.audioMixer.SetFloat("Sfx", Mathf.Log10(value) * 20);
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayBgm(bgmDB.GetSound("fsm-team-escp-abyss"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // PlaySfx(soundEffectDB.GetSound("NormalShot"));
           // PlayBgm(soundEffectDB.GetSound("NormalShot"));
        }
    }
}
