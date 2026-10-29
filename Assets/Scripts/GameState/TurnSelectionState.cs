using UnityEngine;

/// <summary>
/// 先手と後手を決定するクラス
/// </summary>
public class TurnSelectionState : GameStateBase
{
    public override void Enter()
    {
        // プレイヤーの先手と後手をランダムに決定する
        StoneManager.StoneType rand = (StoneManager.StoneType)Random.Range((int)StoneManager.StoneType.Black, (int)StoneManager.StoneType.White + 1);	 
        GameManager.Instance.playerTurn = (GameManager.TurnState)rand;

        manager.ChangeState(new PlaceableCellCheckState());
    }

    public override  void Exit()
    {

    }
}
