public interface IState
{
    public void Enter();

    public void LogicUpdate();

    public void PhysicsUpdate();

    public void Exit();
}