using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// 勝ち負けの判定を行うクラス
/// </summary>
public class WinnerCheckState : GameStateBase
{
    public override void Enter()
    {
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
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public override void Exit()
    {

    }
}
