using UnityEngine;

/// <summary>
/// 盤面とゲームデータを初期化するクラス
/// </summary>
public class BoardInitializationState : GameStateBase
{
    public override void Enter()
    {
        GameManager.Instance.Initialize();
        PlaceInitialStones();
        manager.ChangeState(new TurnSelectionState());
    }

    public override void Exit()
    {

    }

    /// <summary> 盤面データ通りに石を配置するメソッド </summary>
    private void PlaceInitialStones()
    {
        int[,] boardData = GameManager.Instance.boardData;

        for(int x = 0; x < GameManager.BOARD_SIZE; x++)
        {
            for (int y = 0; y < GameManager.BOARD_SIZE; y++)
            {
                if (boardData[x, y] != 0)
                {
                    StoneManager.Instance.CreateStone(new Vector2Int(x, y), (StoneManager.StoneType)(boardData[x, y]));
                }
            }
        }
    }
}
