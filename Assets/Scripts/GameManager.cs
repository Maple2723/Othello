using UnityEngine;

/// <summary>
/// ゲームの状態を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /* ----- 盤面データ変数 ----- */
    #region
    /// <summary> 盤面のサイズ </summary>
    public const int BOARD_SIZE = 8;
    /// <summary> 盤面データ </summary>
    public int[,] boardData = new int[BOARD_SIZE, BOARD_SIZE];
    /// <summary> 初期盤面データ </summary>
    public readonly int[,] INITIAL_BOARD_DATA =
    {
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 2, 1, 0, 0, 0 },
        { 0, 0, 0, 1, 2, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0 }
    };
    /// <summary> 盤面に置かれている石を保持 </summary>
    public GameObject[,]stoneData = new GameObject[BOARD_SIZE, BOARD_SIZE]; 
    /// <summary> 盤面上でハイライトするマスを保持 </summary>
    public GameObject[,] highlightCells = new GameObject[BOARD_SIZE, BOARD_SIZE];
    #endregion

    /* ----- ゲーム内処理用変数 ----- */
    #region
    /// <summary> ターン状態を定義 </summary>
    public enum TurnState
    {
        /// <summary> 指定なしを表します。 </summary>
        None,
        /// <summary> 黒のターン </summary>
        Black,
        /// <summary> 白のターン </summary>
        White
    }
    /// <summary> 現在のターン状態 </summary>
    public TurnState currentTurn;
    /// <summary> プレイヤーがどちらのターンか </summary>
    public TurnState playerTurn;
    /// <summary> ターン経過数 </summary>
    public int turnCount;
    /// <summary> 黒の石の数 </summary>
    public int blackStoneCount;
    /// <summary> 白の石の数 </summary>
    public int whiteStoneCount;
    public bool isPassed;
    #endregion

    /// <summary> アクセス用インスタンス </summary>
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        // シングルトン作成
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary> ゲーム内処理で使ったデータを初期化するメソッド </summary>
    public void Initialize()
    {
        // 盤面データやターン状態等を初期化
        boardData = (int[,])INITIAL_BOARD_DATA.Clone();
        currentTurn = TurnState.Black;
        playerTurn = TurnState.None;
        turnCount = 0;
        blackStoneCount = 2;
        whiteStoneCount = 2;
    }

    public void ResetHighlightCells()
    {
        foreach (GameObject cell in highlightCells)
        {
            if (cell != null) Destroy(cell);
        }
        highlightCells = new GameObject[BOARD_SIZE, BOARD_SIZE];
    }
}
