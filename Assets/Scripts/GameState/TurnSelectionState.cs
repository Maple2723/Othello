using UnityEngine;

/// <summary>
/// ゲーム開始時に先行後攻を決定するクラス
/// </summary>
public class TurnSelectionState : GameStateBase
{
    public override void Enter()
    {
        // 一旦ランダムで先行後攻を決定
        int rand = Random.Range(1, 3);
        GameManager.Instance.playerTurn = (GameManager.TurnState)rand;

        manager.ChangeState(new PlaceableCellCheckState());
    }

    public override  void Exit()
    {

    }
}
