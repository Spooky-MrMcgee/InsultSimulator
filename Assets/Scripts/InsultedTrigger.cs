using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsultedTrigger : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    float animTimer;


    private void Start()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        InsultManager.Instance.Insulted += Insulted;
    }

    private void OnDestroy()
    {
        InsultManager.Instance.Insulted -= Insulted;
    }

    public void Insulted()
    {
        StopAllCoroutines();
        animTimer = 0;
        StartCoroutine(InsultedAnimation());
    }

    public IEnumerator InsultedAnimation()
    {
        while (animTimer < 1)
        {
            animTimer += Time.deltaTime;
            if (InsultManager.Instance.currentPlayerState == InsultManager.PlayerState.PlayerOne)
            {
                GameManager.Instance.playerOne.characterMesh.SetBlendShapeWeight(0, animTimer * 100);
            }
            else
                GameManager.Instance.playerTwo.characterMesh.SetBlendShapeWeight(0, animTimer * 100);
            yield return null;

        }
        animTimer = 1;
        StartCoroutine(InsultedAnimationReverse());
    }

    public IEnumerator InsultedAnimationReverse()
    {
        while (animTimer > 0)
        {
            animTimer -= Time.deltaTime;
            if (InsultManager.Instance.currentPlayerState == InsultManager.PlayerState.PlayerOne)
                GameManager.Instance.playerOne.characterMesh.SetBlendShapeWeight(0, animTimer * 100);
            else
                GameManager.Instance.playerTwo.characterMesh.SetBlendShapeWeight(0, animTimer * 100);
            yield return null;
        }
    }
}
