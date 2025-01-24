using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSoundManager : MonoBehaviour
{
    public SoundHandler cardSource;
    private void Awake()
    {
        cardSource = GameObject.FindGameObjectWithTag("Sound Source").GetComponent<SoundHandler>();
    }
    public void PlaySound()
    {
        cardSource.PlayCardSelectSFX();
    }
}
