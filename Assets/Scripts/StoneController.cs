using UnityEngine;
using System.Collections;

public class StoneController : MonoBehaviour
{
    private Coroutine currentRoutine;

    void Start()
    {
        PlayNewRoutine(PutStoneRoutine());
    }

    // 新しいコルーチンを優先して実行（既存は停止）
    private void PlayNewRoutine(IEnumerator routine)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(routine);
    }

    // 初期配置：0.5秒かけてY=0.75へ移動
    IEnumerator PutStoneRoutine()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = new Vector3(startPos.x, 0.75f, startPos.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.localPosition = targetPos;
        currentRoutine = null;
    }

    public void FlipStone(int _stoneData)
    {
        PlayNewRoutine(FlipRoutine());
    }

    // 反転処理：上げる ➔ 回転 ➔ 下げる
    IEnumerator FlipRoutine()
    {
        // 1. 上げる (0.2秒)
        float riseDuration = 0.2f;
        float elapsed = 0f;
        Vector3 startPos = transform.localPosition;
        Vector3 risePos = new Vector3(startPos.x, 1.75f, startPos.z);

        while (elapsed < riseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / riseDuration;
            transform.localPosition = Vector3.Lerp(startPos, risePos, t);
            yield return null;
        }
        transform.localPosition = risePos;

        // 2. 回転する (0.3秒)
        float rotateDuration = 0.3f;
        elapsed = 0f;
        Quaternion startRot = transform.localRotation;

        float extraRotations = 2f; // 2回転
        float targetAngle = (360f * extraRotations) + 180f;
        Quaternion targetRot = startRot * Quaternion.Euler(targetAngle, 0, 0);

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotateDuration;
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
        transform.localRotation = targetRot;

        // 3. 下げる (0.2秒)
        float dropDuration = 0.2f;
        elapsed = 0f;
        Vector3 currentPos = transform.localPosition;
        Vector3 finalPos = new Vector3(currentPos.x, 0.75f, currentPos.z);

        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropDuration;
            transform.localPosition = Vector3.Lerp(currentPos, finalPos, t);
            yield return null;
        }
        transform.localPosition = finalPos;

        // 終了時の角度を180度単位で補正
        Vector3 finalAngles = transform.localEulerAngles;
        finalAngles.x = Mathf.Round(finalAngles.x / 180f) * 180f;
        transform.localEulerAngles = finalAngles;

        currentRoutine = null;
    }
}