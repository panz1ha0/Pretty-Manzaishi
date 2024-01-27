using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public void playSFX(string name){
        AudioManager.Instance.PlaySFX(name);
    }
    public void startPlayBGM(){
        AudioManager.Instance.InitPlay();
    }

    public void switchMusic(){
        AudioManager.Instance.SwitchMusic();
    }

    public void PlayLaughter(){
        AudioManager.Instance.PlayLaughter();
    }
}
