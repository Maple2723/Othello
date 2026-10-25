using UnityEngine;

/// <summary>
///  ゲームの状態を管理する基底クラス
/// </summary>
public abstract class GameStateBase
{
    public GameStateManager manager;
    /// <summary> 開始時に1回呼ばれるメソッド </summary>
    public abstract void Enter();
    /// <summary> 更新時に毎フレーム呼ばれるメソッド </summary>
    public virtual void Update() { }
    /// <summary> 終了時に1回呼ばれるメソッド </summary>
    public abstract void Exit();

    /// <summary> ステート切り替えを行うためにインスタンスを取得するメソッド </summary>
    /// <param name="_manager"> インスタンス </param>
    public virtual void SetManager(GameStateManager _manager) => manager = _manager;

}