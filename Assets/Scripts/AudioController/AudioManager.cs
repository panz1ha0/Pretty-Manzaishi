using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
public class AudioManager : MonoBehaviour
{
    Boolean Effected;
    public static AudioManager Instance;
    public Sound[] sfxSounds;
    public Sound[] LaughterSounds;
    public Sound[] musicSounds;
    public AudioSource musicSource1, musicSource2, sfxSource;
    // private void Start() {
    //     InitPlay();
    // }
    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    public void PlayMusic_1(){
        musicSource1.volume = 0.7f;
        musicSource1.clip = musicSounds[0].clip;
        musicSource1.Play();
    }
    public void PlayMusic_2(){
        musicSource2.volume = 0;
        musicSource2.clip = musicSounds[1].clip;
        musicSource2.Play();
    }

    public void InitPlay(){
        PlayMusic_1();
        PlayMusic_2();
    }
    public void SwitchMusic(){
        CrossFadeMusic(musicSource1, musicSource2, 0.7f);
    }
    private void CrossFadeMusic(AudioSource sourceA, AudioSource sourceB, float targetVolume)
    {
        //Debug.Log("Target at fading");
        if (sourceB.volume == 0)
        {
            StartCoroutine(FadeVolume(sourceA, 0, 0.1f));
            StartCoroutine(FadeVolume(sourceB, targetVolume, 0.1f));
        }
        else
        {
            StartCoroutine(FadeVolume(sourceB, 0, 0.1f));
            StartCoroutine(FadeVolume(sourceA, targetVolume, 0.1f));
        }
    }

    private IEnumerator FadeVolume(AudioSource source, float targetVolume, float speed)
    {
        float startVolume = source.volume;
        //Debug.Log("fading start at"+source.volume);
        while (!Mathf.Approximately(source.volume, targetVolume))
        {
            Debug.Log("3");
            source.volume = Mathf.Lerp(startVolume, targetVolume, speed);
            startVolume = source.volume;
            yield return new WaitForFixedUpdate();
        }
        source.volume = targetVolume;
        //Debug.Log("fading end at"+source.volume);
    }

    public void PlaySFX(string name){
        Sound s  = Array.Find(sfxSounds, x => x.name == name);
        if(s==null){
            Debug.Log("sounds not found");
        }else{
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlayLaughter(){
        int num = UnityEngine.Random.Range(0,2);
        sfxSource.PlayOneShot(LaughterSounds[num].clip);
    }
}