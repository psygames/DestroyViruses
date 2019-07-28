namespace DestroyViruses
{
    public class StateBase : StateMachine.State
    {
        public virtual void Change<T>() where T : StateBase, new()
        {
            GameManager.Instance.stateMachine.currentState = new T();
        }
    }
}
