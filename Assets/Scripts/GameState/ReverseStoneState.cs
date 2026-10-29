using UnityEngine;

/// <summary>
/// 配置した石で挟んだ相手の石を反転するクラス
/// </summary>
public class ReverseStoneState : GameStateBase
{
    Vector2Int putPosition;	 // 配置した盤面座標

    /// <summary> 配置した石の座標を受けとるメソッド </summary>
    /// <param name="_putPosition"> 配置した盤面座標 </param>
    public ReverseStoneState(Vector2Int _putPosition) => putPosition = _putPosition;

    public override void Enter()
    {
        int[,] boardData = GameManager.Instance.boardData;
        int boardSize = GameManager.BOARD_SIZE;

        // 自分の色と相手の石の色を取得
        int myStone = boardData[putPosition.x, putPosition.y];
        int opponentStone = myStone == (int)StoneManager.StoneType.Black ? (int)StoneManager.StoneType.White : (int)StoneManager.StoneType.Black;

        // 全方向を探索
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int nextX = putPosition.x + x;
                int nextY = putPosition.y + y;

                // 隣接マスに相手の石がなければ次の方向へ
                if (nextX < 0 || nextX >= boardSize || nextY < 0 || nextY >= boardSize || boardData[nextX, nextY] != opponentStone)
                    continue;

                // 相手の石が続く間、次のマスを調べる
                while (nextX >= 0 && nextX < boardSize && nextY >= 0 && nextY < boardSize && boardData[nextX, nextY] == opponentStone)
                {
                    nextX += x;
                    nextY += y;
                }

                // その先に自分の石が無ければ次の方向へ
                if (nextX < 0 || nextX >= boardSize || nextY < 0 || nextY >= boardSize || boardData[nextX, nextY] != myStone)
                    continue;

                int flipX = putPosition.x + x;	 // 裏返す石の盤面X座標
                int flipY = putPosition.y + y;	 // 裏返す石の盤面Y座標

                // 裏返す
                while (boardData[flipX, flipY] == opponentStone)
                {
                    StoneManager.Instance.FlipStone(new Vector2Int(flipX, flipY));

                    flipX += x;
                    flipY += y;
                }
            }
        }

        // ターンを切り替える
        GameManager.Instance.currentTurn = (GameManager.TurnState)(myStone == 1 ? 2 : 1);
        manager.ChangeState(new StoneCountState());
    }

    public override void Exit()
    {

    }
}
