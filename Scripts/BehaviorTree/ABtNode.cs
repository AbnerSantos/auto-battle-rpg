namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class ABtNode<T>
{
    protected readonly List<ABtNode<T?>> ChildNodes = new();

    private T? _btData;
    public T? BtData
    {
        get => _btData;
        set
        {
            _btData = value;
            foreach (ABtNode<T?> node in ChildNodes)
            {
                node.BtData = _btData;
            }
        }
    }

    public abstract void Execute();

    protected void Add(ABtNode<T?> node)
    {
        ChildNodes.Add(node);
        node.BtData = BtData;
    }

    protected void Remove(ABtNode<T?> node)
    {
        ChildNodes.Remove(node);
    }
}