using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource as_bgm;
    private Dictionary<string, AudioSource> dic_audios = new Dictionary<string, AudioSource>();
    private float audio_volume = 1;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetVolumeData();
    }
    private void GetVolumeData()
    {
        as_bgm.volume = SaveData.Data.bgmVolume;
        audio_volume = SaveData.Data.audVolume;
    }
    public void ChangeVolume_bgm(float f)
    {
        as_bgm.volume = f;
        SaveData.Data.bgmVolume = f;
        SaveData.Save();
    }
    public void ChangeVolume_aud(float f)
    {
        foreach (var item in dic_audios)
        {
            item.Value.volume = f;
        }
        audio_volume = f;
        SaveData.Data.audVolume = f;
        SaveData.Save();
    }
    public void PlayAudio(string audioName,bool loop = false)
    {
        if (dic_audios.TryGetValue(audioName,out AudioSource aus))
        {
            if (aus.isPlaying) return;
            aus.loop = loop;
            aus.Play();
            return;
        }
        AudioClip ac = Resources.Load<AudioClip>(audioName);
        if (ac != null)
        {
            var go = new GameObject(audioName);
            var addaus = go.AddComponent<AudioSource>();
            addaus.clip = ac;
            addaus.volume = audio_volume;
            addaus.loop = loop;
            dic_audios.Add(audioName, addaus);
            addaus.Play();
        }
    }
    public void Stop(string audioName)
    {
        if (dic_audios.TryGetValue(audioName, out AudioSource aus))
        {
            aus.Stop();
        }
    }
}
