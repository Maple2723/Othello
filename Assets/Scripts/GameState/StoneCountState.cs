
public class StoneCountState : GameStateBase
{
    bool isGameEnd;

    public StoneCountState(bool _isGameEnd = false) => isGameEnd = _isGameEnd;
    public override void Enter()
    {
        int blackCount = 0;
        int whiteCount = 0;
        foreach (int cell in GameManager.Instance.boardData)
        {
            if (cell == 1)
            {
                blackCount++;
            }
            else if (cell == 2)
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
