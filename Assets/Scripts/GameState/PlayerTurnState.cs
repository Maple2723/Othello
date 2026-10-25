using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの置くマスを決めるクラス
/// </summary>
public class PlayerTurnState : GameStateBase
{
    public override void Enter()
    {

    }

    public override void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                int x = Mathf.RoundToInt(hit.point.x);
                int y = Mathf.RoundToInt(hit.point.z);
                StoneManager.StoneType stoneType = (StoneManager.StoneType)GameManager.Instance.playerTurn;
                StoneManager.Instance.CreateStone(new Vector2Int(x, y), stoneType);

                manager.ChangeState(new ReverseStoneState(new Vector2Int(x, y)));
            }
        }
    }
    public override void Exit()
    {
        GameManager.Instance.ResetHighlightCells();
        StoneManager.Instance.DestroyNpcHighlightCell();
    }
}
