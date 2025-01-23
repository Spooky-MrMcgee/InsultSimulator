using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICardVisual : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI content, cost;

    [SerializeField]
    Button button;

    [SerializeField]
    Image background;

    [SerializeField]
    float animationDuration = 0.5f;

    [SerializeField]
    public Sprite subjectCard, predicateCard, complimentCard;

    RectTransform rect;

    Action onClick;

    float animTimer = 0;

    Vector3 currentPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Setup(CardData card, bool showPrice)
    {
        var price = showPrice ? $"�{card.cost}" : "";

        cost.SetText(price);

        content.SetText(card.content);
        
        if(showPrice)

        if(card is SubjectCardData)
        {
            background.sprite = subjectCard;
        }
        else if(card is PredicateCardData)
        {
            background.sprite = predicateCard;
        }
        else if(card is ComplimentCardData)
        {
            background.sprite = complimentCard;
        }
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

    public void SetInteractable(bool on)
    {
        button.interactable = on;
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
