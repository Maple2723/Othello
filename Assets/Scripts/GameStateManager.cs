using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameStateBase currentState;
    void Start()
    {
        ChangeState(new BoardInitializationState());
    }

    void Update()
    {
        currentState?.Update();
    }

    /// <summary> ステートを切り替えるメソッド </summary>
    /// <param name="_nextState"> 次のステート </param>
    public void ChangeState(GameStateBase _nextState)
    {
        currentState?.Exit();
        currentState = _nextState;
        currentState.SetManager(this);
        currentState?.Enter();
    }
}
