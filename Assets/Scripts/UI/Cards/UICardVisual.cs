using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICardVisual : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI content;

    [SerializeField]
    Button button;

    [SerializeField]
    Image background;

    [SerializeField]
    float animationDuration = 0.5f;

    RectTransform rect;

    Action onClick;

    float animTimer = 0;

    Vector3 currentPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Setup(CardData card)
    {
        content.SetText(card.content);
    }

    public void SetAction(Action onClick)
    {
        this.onClick = onClick;
    }

    public void AnimateToNewPosition(Vector3 position)
    {
        StopAllCoroutines();
        animTimer = 0;
        currentPos = rect.position;
        StartCoroutine(Animate(position));
    }

    IEnumerator Animate(Vector3 target)
    {
        while (animTimer < 1)
        {
            animTimer += Time.deltaTime / animationDuration;

            rect.position = Vector3.Lerp(currentPos, target, EaseOutExpo(animTimer));
            yield return null;
        }
    }

    float EaseOutExpo(float time) {
        return time == 1 ? 1 : 1 - Mathf.Pow(2, -10 * time);
    }

    public void OnSelected()
    {
        onClick?.Invoke();
    }
}
