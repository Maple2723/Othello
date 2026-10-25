using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 置けるマスのハイライト、勝ち負け判定等を行うクラス
/// </summary>
public class PlaceableCellCheckState : GameStateBase
{
    private readonly Vector2Int[] directions =
    {
        new Vector2Int(-1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int( 1, -1),
        new Vector2Int(-1,  0),
        new Vector2Int( 1,  0),
        new Vector2Int(-1,  1),
        new Vector2Int( 0,  1),
        new Vector2Int( 1,  1)
    };

    List<Vector2Int> npcPlaceableCells = new List<Vector2Int>();

    int placeableCellCount;

    public override void Enter()
    {
        UiManager.Instance.UpdateStoneCountText();

        // 置けるマスを調べてターンを進める
        FindPlaceableCells();
    }

    public override void Exit() { }

    /// <summary> 盤面上の置けるマスを調べるメソッド </summary>
    private void FindPlaceableCells()
    {
        // 変数のコピー
        int boardSize = GameManager.BOARD_SIZE;
        int[,] boardData = GameManager.Instance.boardData;
        var currentTurn = GameManager.Instance.currentTurn;
        var playerTurn = GameManager.Instance.playerTurn;

        // 置けるマスを調べる
        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                if (boardData[x, y] != 0) continue;

                Vector2Int putPosition = new Vector2Int(x, y);

                if (CanPlaceAt(putPosition, currentTurn))
                {
                    // 置けるマスが見つかったことを定義しておく
                    placeableCellCount++;
                    GameManager.Instance.isPassed = false;

                    // プレイヤーのターンならハイライト、NPCのターンなら配置可能マスをリストに追加
                    if (currentTurn == playerTurn)
                    {
                        StoneManager.Instance.PlayerHighlightPlaceableCell(x, y);
                    }
                    else
                    {
                        npcPlaceableCells.Add(putPosition);
                    }
                }
            }
        }
        TurnChanging();
    }

    /// <summary> 石が置けるかどうかを全方向調べるメソッド </summary>
    /// <param name="_putPosition"> 石の座標 </param>
    /// <param name="_currentTurn"> 現在のターン </param>
    /// <returns> 石が置けるかどうか </returns>
    private bool CanPlaceAt(Vector2Int _putPosition, GameManager.TurnState _currentTurn)
    {
        foreach (var direction in directions)
        {
            if (SearchDirection(_putPosition, direction, _currentTurn))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary> ターン切り替えを行うメソッド </summary>
    private void TurnChanging()
    {
        // 変数のコピー
        var currentTurn = GameManager.Instance.currentTurn;
        var playerTurn = GameManager.Instance.playerTurn;

        // 2回目連続で見つからなかったら終了
        if (placeableCellCount == 0)
        {
            if (GameManager.Instance.isPassed)
            {
                manager.ChangeState(new StoneCountState(true));
                return;
            }
            else
            {
                // 手番を相手に切り替える
                currentTurn = currentTurn == GameManager.TurnState.Black ? GameManager.TurnState.White : GameManager.TurnState.Black;

                GameManager.Instance.currentTurn = currentTurn;
                GameManager.Instance.isPassed = true;


                // 検索結果を初期化
                placeableCellCount = 0;
                npcPlaceableCells.Clear();

                // 相手の置けるマスを再検索
                FindPlaceableCells();
                if (placeableCellCount != 0)
                    UiManager.Instance.ShowPassTextAsync().Forget();
                return;
            }
        }

        // 置けるマスが見つかった場合は配置ターンへ移る
        if (currentTurn == playerTurn)
        {
            manager.ChangeState(new PlayerTurnState());
        }
        else
        {
            manager.ChangeState(new NpcTurnState(npcPlaceableCells));
        }

        UiManager.Instance.UpdateTurnText();
        GameManager.Instance.turnCount++;
    }

    /// <summary> 石を置けるかどうかを調べるメソッド </summary>
    /// <param name="_position"> 石の位置 </param>
    /// <param name="_direction"> 探索方向 </param>
    /// <param name="_currentTurn">現在のターン。</param>
    /// <returns> 置けるマスかどうか </returns>
    private bool SearchDirection(Vector2Int _position, Vector2Int _direction, GameManager.TurnState _currentTurn)
    {
        // 変数のコピー
        int boardSize = GameManager.BOARD_SIZE;
        int[,] boardData = GameManager.Instance.boardData;

        // 現在のターンを基に自分の色と相手の色を判定
        int myStone = (int)_currentTurn;
        int opponentStone = myStone == 1 ? 2 : 1;

        // 次のマスの座標を計算
        int nextX = _position.x + _direction.x;
        int nextY = _position.y + _direction.y;

        // 盤面外、または相手の石が周りに存在しない場合は処理を抜ける
        if (nextX < 0 || nextX >= boardSize || nextY < 0 || nextY >= boardSize || boardData[nextX, nextY] != opponentStone)
            return false;

        // 相手の石が続く間、次のマスを調べてその先に自分の石が存在すれば有効
        while (nextX >= 0 && nextX < boardSize && nextY >= 0 && nextY < boardSize && boardData[nextX, nextY] == opponentStone)
        {
            nextX += _direction.x;
            nextY += _direction.y;
        }
        return nextX >= 0 && nextX < boardSize && nextY >= 0 && nextY < boardSize && boardData[nextX, nextY] == myStone;
    }
}