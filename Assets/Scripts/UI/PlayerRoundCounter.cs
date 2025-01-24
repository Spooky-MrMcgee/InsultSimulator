using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoundCounter : MonoBehaviour
{
    [SerializeField] Image[] counters;
    [SerializeField] Sprite empty, full;

    public void SetScore(int rounds)
    {
        for (int i = 0; i < counters.Length; i++)
        {
            counters[i].sprite = i < rounds ? full : empty;
        }
    }
}
