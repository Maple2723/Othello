using UnityEngine;

/// <summary>
/// 盤面と手番を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /* ----- 盤面データ変数 ----- */
    #region
    public const int BOARD_SIZE = 8;												 // 盤面のサイズ
    public int[,] boardData = new int[BOARD_SIZE, BOARD_SIZE];						 // 盤面データ
    public readonly int[,] INITIAL_BOARD_DATA =										 // 開始時の盤面データ
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
    public GameObject[,]stoneData = new GameObject[BOARD_SIZE, BOARD_SIZE];			 // 盤面に配置された石を保存
    public GameObject[,] highlightCells = new GameObject[BOARD_SIZE, BOARD_SIZE];	 // ハイライトしたマスを保存
    #endregion

    /* ----- ゲーム内処理用変数 ----- */
    #region
    /// <summary> ターン状態を定義 </summary>
    public enum TurnState
    {
        None,   // 初期化用
        Black,	 // 黒のターン
        White	 // 白のターン
    }
    public TurnState currentTurn;								 // 現在の手番
    public TurnState playerTurn;								 // プレイヤーの手番
    public int blackStoneCount;									 // 盤面上の黒石の数
    public int whiteStoneCount;									 // 盤面上の白石の数
    public bool isPassed;										 // 直前の手番がパスしたかのフラグ
    #endregion

    public static GameManager Instance { get; private set; }	// インスタンス

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


    /// <summary> データを初期化するメソッド </summary>
    public void Initialize()
    {
        // 盤面データやターン状態等を初期化
        boardData = (int[,])INITIAL_BOARD_DATA.Clone();
        currentTurn = TurnState.Black;
        playerTurn = TurnState.None;
        blackStoneCount = 2;
        whiteStoneCount = 2;
    }

    /// <summary> ハイライトを削除するメソッド </summary>
    public void ResetHighlightCells()
    {
        foreach (GameObject cell in highlightCells)
        {
            if (cell != null) Destroy(cell);
        }
        highlightCells = new GameObject[BOARD_SIZE, BOARD_SIZE];
    }
}
