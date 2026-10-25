using UnityEngine;

/// <summary>
/// ボードやゲームの初期化を行うクラス
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

    /// <summary> 初期の石を盤面に配置するメソッド </summary>
    private void PlaceInitialStones()
    {
        // 変数をコピー
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
