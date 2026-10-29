using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 勝敗判定と再開始の入力を処理するクラス
/// </summary>
public class WinnerCheckState : GameStateBase
{
    public override void Enter()
    {

        // 石の数によって勝敗を決める
        // 仮仕様：コンソール上で表示
        int blackCount = GameManager.Instance.blackStoneCount;
        int whiteCount = GameManager.Instance.whiteStoneCount;
        if (blackCount > whiteCount)
        {
            Debug.Log("黒の勝ち→" + "黒：" + blackCount + " | 白：" + whiteCount);
        }
        else if (blackCount < whiteCount)
        {
            Debug.Log("白の勝ち" + "黒：" + blackCount + " | 白：" + whiteCount);
        }
        else
        {
            Debug.Log("引き分け" + "黒：" + blackCount + " | 白：" + whiteCount);
        }
    }

    public override void Update()
    {

        // 仮仕様：右クリックしたら初期化して再度プレイ
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary> 状態を終了するメソッド </summary>
    public override void Exit()
    {

    }
}
