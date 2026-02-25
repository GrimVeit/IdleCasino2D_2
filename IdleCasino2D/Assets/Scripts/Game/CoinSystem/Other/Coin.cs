using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [SerializeField] private ClickItem clickItemCoin;
    [SerializeField] private SkeletonAnimation skeletonAnimationCoin;
    [SerializeField] private float spawnRadius = 1f;
    [SerializeField] private float lifeTime = 8f;
    [SerializeField] private float jumpPower = 1f;
    [SerializeField] private int numJumps = 1;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float fadeDuration = 0.5f;

    private int _countMondey;
    private IEnumerator timer;
    private Tween tweenScale;
    private bool isRewarded = false;

    public void Initialize()
    {
        clickItemCoin.OnClick += Collect;

        if (timer != null) StopCoroutine(timer);

        timer = Timer();
        Coroutines.Start(timer);
    }

    public void Dispose()
    {
        if (timer != null) StopCoroutine(timer);

        clickItemCoin.OnClick -= Collect;
    }

    public void SetData(int countMoney)
    {
        _countMondey = countMoney;
    }

    private void Collect()
    {
        if (isRewarded) return;

        isRewarded = true;

        OnCollectCoin?.Invoke(this, _countMondey);
    }

    private void OnDestroy()
    {
        tweenScale?.Kill();
    }

    public void PlaySpawnAnimation()
    {
        tweenScale?.Kill();

        tweenScale = transform.DOScale(1.4f, fadeDuration - 0.3f);

        Vector3 randomOffset = new(
            Random.Range(-spawnRadius, spawnRadius),
            Random.Range(-spawnRadius, spawnRadius),
            0f
        );

        Vector3 targetPosition = transform.position + randomOffset;

        transform.DOJump(
            targetPosition,
            jumpPower,
            numJumps,
            jumpDuration
        );
    }

    public void PlayCollectDestroyAnimation()
    {
        tweenScale?.Kill();

        tweenScale = transform.DOScale(0.2f, 0.6f);

        if (timer != null) StopCoroutine(timer);

        skeletonAnimationCoin.AnimationState.SetAnimation(0, "coins_flying", false)
            .Complete += _ =>
            {
                Destroy(gameObject);
            };
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);

        Collect();
    }

    #region Output

    public event Action<Coin, int> OnCollectCoin;

    #endregion
}
