using DG.Tweening;
using UnityEngine;

public class RandomJump : MonoBehaviour
{

    private void Start()
    {
        var randomTime = Random.Range(0.25f, 0.75f);
        var randomY = Random.Range(50, 100);

        var loopJump = DOTween.Sequence();
        loopJump.SetLoops(-1, LoopType.Yoyo);
        loopJump.Append(transform.GetComponent<RectTransform>().DOAnchorPosY(78.1f + randomY, randomTime));
        loopJump.PrependInterval(0.45f);
    }
}
