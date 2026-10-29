/// <summary>
/// ゲームの各状態に共通する処理を定義するクラス
/// </summary>
public abstract class GameStateBase
{
    public GameStateManager manager;	 // インスタンス
    /// <summary> 開始時に一度実行するメソッド </summary>
    public abstract void Enter();
    /// <summary> 毎フレーム実行するメソッド </summary>
    public virtual void Update() { }
    /// <summary> 終了時に一度実行するメソッド </summary>
    public abstract void Exit();

    /// <summary> インスタンスを設定するメソッド </summary>
    /// <param name="_manager"> インスタンス </param>
    public virtual void SetManager(GameStateManager _manager) => manager = _manager;

}