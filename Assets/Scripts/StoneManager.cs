using UnityEngine;

/// <summary>
/// オセロの石の生成や裏返しなどを行うクラス
/// </summary>
public class StoneManager : MonoBehaviour
{
    public enum StoneType
    {
        Black = 1,
        White
    }

    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private GameObject playerHighlightCellPrefab;
    [SerializeField] private GameObject npcHighlightCellPrefab;
    public GameObject npcHighlightCell;

    private readonly Vector3 STONE_DEFAULT_POS = new Vector3(0f, 0.75f, 0f);

    public static StoneManager Instance { get; private set; }

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

    /// <summary> 指定された座標に石を生成するメソッド </summary>
    /// <param name="_x"> 指定された盤面座標X </param>
    /// <param name="_y"> 指定された盤面座標Y </param>
    /// <param name="_stoneType"> 生成する石の色 </param>
    public void CreateStone(Vector2Int _position, StoneType _stoneType)
    {
        int _x = _position.x;
        int _y = _position.y;
        // 石の生成座標を決定
        Vector3 position = new Vector3(_x, 0.5f, _y) + STONE_DEFAULT_POS;
        GameObject putStone = Instantiate(stonePrefab, position, Quaternion.identity);

        // 白なら反対向きにする
        if (_stoneType == StoneType.White)
        {
            for (int i = 0; i < putStone.transform.childCount; i++)
            {
                Transform child = putStone.transform.GetChild(i);
                Vector3 childPosition = child.localPosition;
                childPosition.y *= -1f;
                child.localPosition = childPosition;
            }
        }
        GameManager.Instance.stoneData[_x, _y] = putStone;
        GameManager.Instance.boardData[_x, _y] = (int)_stoneType;
    }

    /// <summary> 指定された座標の石を裏返すメソッド </summary>
    /// <param name="_position"> 裏返す盤面座標 </param>
    public void FlipStone(Vector2Int _position)
    {
        int _x = _position.x;
        int _y = _position.y;
        GameManager.Instance.stoneData[_x, _y].transform.rotation *= Quaternion.Euler(180f, 0f, 0f);
        GameManager.Instance.boardData[_x, _y] = GameManager.Instance.boardData[_x, _y] == 1 ? 2 : 1;
    }

    /// <summary> 石を置けるマスにハイライトを表示するメソッド </summary>
    /// <param name="_x"> ハイライト座標X </param>
    /// <param name="_y"> ハイライト座標Y </param>
    public void PlayerHighlightPlaceableCell(int _x, int _y)
    {
        Vector3 position = new Vector3(_x, 0, _y) + STONE_DEFAULT_POS;

        GameObject putHighlightCell = Instantiate(playerHighlightCellPrefab, position, Quaternion.identity);
        GameManager.Instance.highlightCells[_x, _y] = putHighlightCell;
    }

    public void NpcHighlightPlaceCell(Vector2Int _position)
    {
        Vector3 position = new Vector3(_position.x, 0, _position.y) + STONE_DEFAULT_POS;

        npcHighlightCell = Instantiate(npcHighlightCellPrefab, position, Quaternion.identity);
    }

    public void DestroyNpcHighlightCell()
    {
        if(npcHighlightCell != null)
        Destroy(npcHighlightCell);
    }
}
