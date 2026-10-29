using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>
/// NPCの配置先を決定して石を配置するクラス
/// </summary>
public class NpcTurnState : GameStateBase
{
    private List<Vector2Int> placeableCells = new List<Vector2Int>();	 // NPCが石を配置できるマス
    private const int PUT_WAIT_TIME = 1500;                              // 配置する際の待機時間

    /// <summary> NPCが配置可能なマスを受け取るメソッド </summary>
    /// <param name="_placeableCells"> 配置できる盤面座標の一覧 </param>
    public NpcTurnState(List<Vector2Int> _placeableCells) => placeableCells = _placeableCells;

    public override void Enter()
    {
        PlaceStoneAsync().Forget();
    }

    public override void Exit()
    {

    }

    /// <summary> 待機後に配置先をランダムに選んで石を配置するメソッド </summary>
    /// <returns> NPCの石の配置を実行する非同期処理 </returns>
    private async UniTaskVoid PlaceStoneAsync()
    {
        // 配置座標を決定する
        int rand = Random.Range(0, placeableCells.Count);
        Vector2Int putPosition = placeableCells[rand];

        await UniTask.Delay(PUT_WAIT_TIME);  // 待機

        StoneManager.Instance.CreateStone(putPosition, (StoneManager.StoneType)GameManager.Instance.currentTurn);
        StoneManager.Instance.NpcHighlightPlaceCell(putPosition);

        manager.ChangeState(new ReverseStoneState(putPosition));
    }
}
