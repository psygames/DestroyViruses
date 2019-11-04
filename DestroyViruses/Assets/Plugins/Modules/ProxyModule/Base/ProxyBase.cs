public abstract class ProxyBase<T> where T : ProxyBase<T>
{
    protected virtual void OnInit() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnDestroy() { }

    public static T Ins { get { return ProxyManager.GetProxy<T>(); } }
}