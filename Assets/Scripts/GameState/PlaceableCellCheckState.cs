using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 配置可能なマスを探索して次の手番を決定するクラス
/// </summary>
public class PlaceableCellCheckState : GameStateBase
{
    private readonly Vector2Int[] directions =						 // 配置可否を調べる8方向の座標差分
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
    List<Vector2Int> npcPlaceableCells = new List<Vector2Int>();	 // NPCの配置可能座標
    int placeableCellCount;											 // 石を配置できるマスの数

    public override void Enter()
    {
        UiManager.Instance.UpdateStoneCountText();

        FindPlaceableCells();
    }

    public override void Exit()
    {
        UiManager.Instance.UpdateTurnText();
    }

    /// <summary> 配置可能なマスを調べてターンを進めるメソッド </summary>
    private void FindPlaceableCells()
    {
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
                    // 置けるマスが見つかったら記録してパスに入らないようにする
                    placeableCellCount++;
                    GameManager.Instance.isPassed = false;

                    // プレイヤーのターンならハイライト、NPCのターンなら配置可能マスをリストに追加
                    if (currentTurn == playerTurn)
                    {
                        StoneManager.Instance.PlayerHighlightPlaceableCell(new Vector2Int(x, y));
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

    /// <summary> 配置可能なマスの有無に応じてターンを進めるメソッド </summary>
    private void TurnChanging()
    {
        var currentTurn = GameManager.Instance.currentTurn;
        var playerTurn = GameManager.Instance.playerTurn;

        // パスの場合
        if (placeableCellCount == 0)
        {
            // 2連続パスの場合ゲームを終了する
            if (GameManager.Instance.isPassed)
            {
                manager.ChangeState(new StoneCountState(true));
                return;
            }
            // 手番を相手に切り替える
            currentTurn = currentTurn == GameManager.TurnState.Black ? GameManager.TurnState.White : GameManager.TurnState.Black;
            GameManager.Instance.currentTurn = currentTurn;

            // 2連続パスの判定のためにフラグを立てる
            GameManager.Instance.isPassed = true;

            // 検索結果を初期化
            npcPlaceableCells.Clear();

            // 相手の置けるマスを再検索
            FindPlaceableCells();
            if (placeableCellCount != 0) UiManager.Instance.ShowPassTextAsync().Forget();

            return;
        }

        // 置けるマスが見つかった場合は配置ターンへ移る
        if (currentTurn == playerTurn)
        {
            manager.ChangeState(new PlayerTurnState());
            return;
        }

        manager.ChangeState(new NpcTurnState(npcPlaceableCells));
    }

    /// <summary> 石を配置できるか全方向を調べて判定するメソッド </summary>
    /// <param name="_putPosition"> 置いた石の盤面座標 </param>
    /// <param name="_currentTurn"> 石を配置した時のターンの色 </param>
    /// <returns> 置けるマスか否か </returns>
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

    /// <summary> 相手の石を挟めるか判定するメソッド </summary>
    /// <param name="_position"> 配置した石の盤面座標 </param>
    /// <param name="_direction"> 探索方向 </param>
    /// <param name="_currentTurn"> 配置する石の色 </param>
    /// <returns> 石を挟める場合はTRUE </returns>
    private bool SearchDirection(Vector2Int _position, Vector2Int _direction, GameManager.TurnState _currentTurn)
    {
        int boardSize = GameManager.BOARD_SIZE;
        int[,] boardData = GameManager.Instance.boardData;

        int myStone = (int)_currentTurn;
        int opponentStone = myStone == (int)GameManager.TurnState.Black ? (int)GameManager.TurnState.White : (int)GameManager.TurnState.Black;

        // 検索する方向の座標
        int nextX = _position.x + _direction.x;
        int nextY = _position.y + _direction.y;

        // 盤面外、または相手の石が周りに存在しない
        if (nextX < 0 || nextX >= boardSize || nextY < 0 || nextY >= boardSize || boardData[nextX, nextY] != opponentStone)
            return false;

        // 盤面内で相手の石が続いておかれている場合は探索を進める
        while (nextX >= 0 && nextX < boardSize && nextY >= 0 && nextY < boardSize && boardData[nextX, nextY] == opponentStone)
        {
            nextX += _direction.x;
            nextY += _direction.y;
        }

        // 探索してその先に自分の石があったらTRUE
        return nextX >= 0 && nextX < boardSize && nextY >= 0 && nextY < boardSize && boardData[nextX, nextY] == myStone;
    }
}