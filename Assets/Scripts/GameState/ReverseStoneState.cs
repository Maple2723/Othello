using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 置かれたマスを参照して裏返し処理を行うクラス
/// </summary>
public class ReverseStoneState : GameStateBase
{
    Vector2Int putPosition;
    public ReverseStoneState(Vector2Int _putPosition)
    {
        putPosition = _putPosition;
    }

    public override void Enter()
    {

        // 変数のコピー
        int[,] boardData = GameManager.Instance.boardData;
        int boardSize = GameManager.BOARD_SIZE;

        // 自分の色と相手の石の色を取得
        int thisStone = boardData[putPosition.x, putPosition.y];
        int opponentStone = thisStone == 1 ? 2 : 1;

        // 8方向を探索
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue;

                int nextX = putPosition.x + dx;
                int nextY = putPosition.y + dy;

                // 隣接マスに相手の石がなければ次の方向へ
                if (nextX < 0 || nextX >= boardSize || nextY < 0 || nextY >= boardSize || boardData[nextX, nextY] != opponentStone)
                    continue;

                // 相手の石が続く間、次のマスを調べる
                while (nextX >= 0 && nextX < boardSize && nextY >= 0 && nextY < boardSize && boardData[nextX, nextY] == opponentStone)
                {
                    nextX += dx;
                    nextY += dy;
                }

                // その先に自分の石があれば、間の石を裏返せる
                if (nextX < 0 || nextX >= boardSize || nextY < 0 || nextY >= boardSize || boardData[nextX, nextY] != thisStone)
                    continue;

                // 裏返せる石の座標を表示
                int flipX = putPosition.x + dx;
                int flipY = putPosition.y + dy;

                while (boardData[flipX, flipY] == opponentStone)
                {
                    StoneManager.Instance.FlipStone(new Vector2Int(flipX, flipY));

                    flipX += dx;
                    flipY += dy;
                }
            }
        }

        // ターンを切り替える
        GameManager.Instance.currentTurn = (GameManager.TurnState)(thisStone == 1 ? 2 : 1);
        manager.ChangeState(new StoneCountState());
    }

    public override void Exit()
    {

    }
}
