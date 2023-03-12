namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class ABtNode<T>
{
    public readonly List<ABtNode<T?>> ChildNodes = new();

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

    public void Add(ABtNode<T> node)
    {
        node.BtData = BtData;
        ChildNodes.Add(node!);
    }

    public void Remove(ABtNode<T?> node)
    {
        ChildNodes.Remove(node);
    }
}