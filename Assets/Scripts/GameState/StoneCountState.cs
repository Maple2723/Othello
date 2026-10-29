
/// <summary>
/// 石を集計して次の状態へ遷移するクラス
/// </summary>
public class StoneCountState : GameStateBase
{
    bool isGameEnd;	 // 石の集計後に勝敗判定フラグ

    /// <summary> 集計後に勝敗判定へ進むかを設定するメソッド </summary>
    /// <param name="_isGameEnd"> ゲームを終了するかのフラグ </param>
    public StoneCountState(bool _isGameEnd = false) => isGameEnd = _isGameEnd;
    public override void Enter()
    {
        // 盤面上の石の数を数える
        int blackCount = 0;
        int whiteCount = 0;
        foreach (int cell in GameManager.Instance.boardData)
        {
            if (cell == (int)StoneManager.StoneType.Black)
            {
                blackCount++;
            }
            else if (cell == (int)StoneManager.StoneType.White)
            {
                whiteCount++;
            }
        }

        GameManager.Instance.blackStoneCount = blackCount;
        GameManager.Instance.whiteStoneCount = whiteCount;

        if (isGameEnd)
        {
            manager.ChangeState(new WinnerCheckState());
            return;
        }
        manager.ChangeState(new PlaceableCellCheckState());
    }

    public override void Exit()
    {

    }
}
