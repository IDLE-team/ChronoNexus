public interface IState
{
    abstract public void Enter();

    abstract public void LogicUpdate();

    abstract public void PhysicsUpdate();

    abstract public void Exit();
}