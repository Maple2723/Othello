using UnityEngine;

/// <summary>
/// 石と配置マスのハイライトを管理するクラス
/// </summary>
public class StoneManager : MonoBehaviour
{
    public enum StoneType
    {
        Black = 1,
        White
    }

    [SerializeField] private GameObject stonePrefab;							 // 生成する石
    [SerializeField] private GameObject playerHighlightCellPrefab;				 // プレイヤーの配置可能マスを表示プレファブ
    [SerializeField] private GameObject npcHighlightCellPrefab;					 // NPCの配置先を示すプレファブ
    public GameObject npcHighlightCell;											 // 表示中のNPCのハイライト
    private readonly Vector3 STONE_DEFAULT_POS = new Vector3(0f, 0.75f, 0f);	 // 石とハイライトの配置位置の補正量

    public static StoneManager Instance { get; private set; }					 // インスタンス

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

    /// <summary> 石を生成して盤面データへ登録するメソッド </summary>
    /// <param name="_position"> 生成する盤面座標 </param>
    /// <param name="_stoneType"> 配置する石の色 </param>
    public void CreateStone(Vector2Int _position, StoneType _stoneType)
    {
        int _x = _position.x;
        int _y = _position.y;

        Vector3 position = new Vector3(_x, 0f, _y) + STONE_DEFAULT_POS;
        GameObject putStone = Instantiate(stonePrefab, position, Quaternion.identity);

        putStone.GetComponent<StoneController>().Initialize(_stoneType);
        GameManager.Instance.stoneData[_x, _y] = putStone;
        GameManager.Instance.boardData[_x, _y] = (int)_stoneType;
    }

    /// <summary> 石を反転して盤面データを更新するメソッド </summary>
    /// <param name="_position"> 盤面座標 </param>
    public void FlipStone(Vector2Int _position)
    {
        int _x = _position.x;
        int _y = _position.y;

        GameManager.Instance.stoneData[_x, _y].GetComponent<StoneController>().FlipStone((StoneType)GameManager.Instance.boardData[_x, _y]);
        GameManager.Instance.boardData[_x, _y] = GameManager.Instance.boardData[_x, _y] == (int)StoneType.Black ? (int)StoneType.White : (int)StoneType.Black;
    }

    /// <summary> プレイヤーが石を置けるマスをハイライト表示するメソッド </summary>
    /// <param name="_position"> 盤面座標 </param>
    public void PlayerHighlightPlaceableCell(Vector2Int _position)
    {
        int _x = _position.x;
        int _y = _position.y;

        Vector3 position = new Vector3(_x, 0, _y) + STONE_DEFAULT_POS;

        GameObject putHighlightCell = Instantiate(playerHighlightCellPrefab, position, Quaternion.identity);
        GameManager.Instance.highlightCells[_x, _y] = putHighlightCell;
    }

    /// <summary> NPCが石を配置したマスをハイライト表示するメソッド </summary>
    /// <param name="_position"> 盤面座標 </param>
    public void NpcHighlightPlaceCell(Vector2Int _position)
    {
        Vector3 position = new Vector3(_position.x, 0, _position.y) + STONE_DEFAULT_POS;

        npcHighlightCell = Instantiate(npcHighlightCellPrefab, position, Quaternion.identity);
    }

    /// <summary> NPCの置いたマスのハイライトを削除するメソッド </summary>
    public void DestroyNpcHighlightCell()
    {
        if (npcHighlightCell != null) return;
        Destroy(npcHighlightCell);
    }
}
