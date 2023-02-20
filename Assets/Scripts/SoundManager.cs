using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;


    #region Singleton
    private void Awake()
    {
        if(instance == null) 
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #endregion

    public AudioSource[] audioSourcesEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourcesEffects.Length; j++)
                {
                    if (!audioSourcesEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourcesEffects[j].clip = effectSounds[i].clip;
                        audioSourcesEffects[j].Play();

                        Debug.Log("소리 들림");

                        return;
                    }
                }

                Debug.Log("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }

        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            audioSourcesEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourcesEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourcesEffects[i].Stop();
                return;
            }
        }

        Debug.Log("재생 중인" + _name + "사운드가 없습니다");
    }
}
