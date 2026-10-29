using UnityEngine;

/// <summary>
/// 石のアニメーションを制御するクラス
/// </summary>
public class StoneController : MonoBehaviour
{
    [SerializeField] private Animator animator;	 // アニメーター

    /// <summary> 設置アニメーションを再生するメソッド </summary>
    /// <param name="_type"> 配置する石の色 </param>
    public void Initialize(StoneManager.StoneType _type)
    {
        Play(_type == StoneManager.StoneType.Black ? "PutBlack" : "PutWhite");
    }

    /// <summary> 反転アニメーションを再生するメソッド </summary>
    /// <param name="_stoneColor"> 反転前の石の色 </param>
    public void FlipStone(StoneManager.StoneType _stoneColor)
    {
        Play(_stoneColor == StoneManager.StoneType.Black ? "FlipToWhite" : "FlipToBlack");
    }

    /// <summary> 指定したアニメーションを再生するメソッド </summary>
    /// <param name="stateName"> 再生するアニメーション </param>
    private void Play(string stateName)
    {
        if (animator == null) return;

        animator.Play(stateName);
    }
}
