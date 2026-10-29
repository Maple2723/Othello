using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// 石の個数と手番の表示を管理するクラス
/// </summary>
public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI blackCountText;	 // 黒い石の個数を表示
    [SerializeField] private TextMeshProUGUI whiteCountText;	 // 白い石の個数を表示
    [SerializeField] private TextMeshProUGUI turnText;			 // 現在の手番の表示
    [SerializeField] private TextMeshProUGUI passText;			 // パス通知の表示

    public static UiManager Instance { get; private set; }		 // インスタンス
    void Awake()
    {
        // シングルトン作成
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    /// <summary> プレイヤーとNPCの石の色と個数を表示するメソッド </summary>
    public void UpdateStoneCountText()
    {
        string blackText = "";			 // 黒い石の担当を含む表示文字列
        string whiteText = "";			 // 白い石の担当を含む表示文字列


        blackText = "<color=black><size=200%>●</size></color>";
        whiteText = "<color=white><size=200%>●</size></color>";

        if (GameManager.Instance.playerTurn == GameManager.TurnState.Black)
        {
            blackText += " <color=white><size=70%>YOU</size><size=60%> (Black)  </size></color> ";
            whiteText += " <color=white><size=70%>NPC</size><size=60%> (white)    </size></color>";
        }
        else
        {
            blackText += " <color=white><size=70%>NPC</size><size=60%> (Black)  </size></color> ";
            whiteText += " <color=white><size=70%>YOU</size><size=60%> (white)    </size></color>";
        }

        blackCountText.text = $"{blackText}<size=130%>{GameManager.Instance.blackStoneCount}</size>";
        whiteCountText.text = $"{whiteText}<size=130%>{GameManager.Instance.whiteStoneCount}</size>";
    }

    /// <summary> 現在の手番と石の色を表示するメソッド </summary>
    public void UpdateTurnText()
    {
        string enemyTurnColor = "";		 // NPCの石の色を示す表示文字列
        string playerTurnColor = "";	 // プレイヤーの石の色を示す表示文字列

        if (GameManager.Instance.playerTurn == GameManager.TurnState.Black)
        {
            playerTurnColor = " <color=black><size=200%>●</size></color>";
            enemyTurnColor = " <color=white><size=200%>●</size></color>";
        }
        else
        {
            playerTurnColor = " <color=white><size=200%>●</size></color>";
            enemyTurnColor = " <color=black><size=200%>●</size></color>";
        }

        if (GameManager.Instance.currentTurn == GameManager.Instance.playerTurn)
        {
            turnText.text = playerTurnColor + "<size=120%>Your turn</size>";
        }
        else
        {
            turnText.text = enemyTurnColor + "<size=120%>NPC turn</size>";
        }

    }

    /// <summary> パスの通知を一定時間表示するメソッド </summary>
    public async UniTaskVoid ShowPassTextAsync()
    {
        passText.text = "Pass";

        await UniTask.Delay(750);

        passText.text = "";
    }
}
