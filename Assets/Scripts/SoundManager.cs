using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //���� �̸�
    public AudioClip clip; //��
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

                        Debug.Log("�Ҹ� �鸲");

                        return;
                    }
                }

                Debug.Log("��� ���� AudioSource�� ������Դϴ�");
                return;
            }
        }

        Debug.Log(_name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
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

        Debug.Log("��� ����" + _name + "���尡 �����ϴ�");
    }
}
