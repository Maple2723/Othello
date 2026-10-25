using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>
/// NPCの置くマスを決めるクラス
/// </summary>
public class NpcTurnState : GameStateBase
{
    List<Vector2Int> placeableCells = new List<Vector2Int>();
    public NpcTurnState(List<Vector2Int> _placeableCells)
    {
        placeableCells = _placeableCells;
    }
    public override void Enter()
    {
        PlaceStoneAsync().Forget();
    }

    private async UniTaskVoid PlaceStoneAsync()
    {
        await UniTask.Delay(500);

        int rand = Random.Range(0, placeableCells.Count);
        Vector2Int putPosition = placeableCells[rand];

        StoneManager.Instance.CreateStone(putPosition, (StoneManager.StoneType)GameManager.Instance.currenTurn);
        StoneManager.Instance.NpcHighlightPlaceCell(putPosition);

        manager.ChangeState(new ReverseStoneState(putPosition));
    }

    public override void Exit()
    {

    }
}
