using System;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    Boolean isWithoutEffect;
    public static AudioManager Instance;
    public Sound[] sfxSounds;
    public Sound[] musicSounds;
    public AudioSource musicSource1, musicSource2, sfxSource;
    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    public void PlayMusic_1(){
        musicSource1.volume = 0;
        musicSource1.clip = musicSounds[0].clip;
        musicSource1.Play();
    }
    public void PlayMusic_2(){
        musicSource2.volume = 1;
        musicSource2.clip = musicSounds[1].clip;
        musicSource2.Play();
    }

    public void InitPlay(){
        PlayMusic_1();
        PlayMusic_2();
    }
    public void SwitchMusic(){
        if(isWithoutEffect){
            
        }else{

        }
    }
    public void PlaySFX(string name){
        Sound s  = Array.Find(sfxSounds, x => x.name == name);
        if(s==null){
            Debug.Log("sounds not found");
        }else{
            sfxSource.PlayOneShot(s.clip);
        }
    }
}