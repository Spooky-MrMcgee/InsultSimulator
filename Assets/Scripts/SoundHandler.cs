using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    public static SoundHandler soundHandler;
    [SerializeField] AudioSource SFXSource, musicSource;
    [SerializeField] private AudioClip cardClip, buttonClip, shopMusic, insultMusic;
    private void Awake()
    {
        soundHandler = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += UpdateMusic;
    }

    void UpdateMusic(GameManager.GameState gameState)
    {
        if (gameState == GameManager.GameState.Shop)
        {
            musicSource.clip = shopMusic;
            musicSource.Play();
        }
        else if (gameState == GameManager.GameState.Insult)
        {
            musicSource.clip = insultMusic;
            musicSource.Play();
        }
    }
    public void PlayCardSelectSFX()
    {
        SFXSource.clip = cardClip;
        SFXSource.Play();
    }

    public void PlayButtonSelectSFX()
    {
        SFXSource.clip = buttonClip;
        SFXSource.Play();
    }
}
