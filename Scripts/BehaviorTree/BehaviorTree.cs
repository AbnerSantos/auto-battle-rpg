using AutoBattleRPG.Scripts.Pathfinding;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class BehaviorTree<T>
{
    public ABtNode<T> RootNode { get; set; }
    public T BtData { get; set; }

    public BehaviorTree(ABtNode<T> rootNode, T btData)
    {
        RootNode = rootNode;
        RootNode.BtData = btData;
        BtData = btData;
    }

    public bool Execute()
    {
        try
        {
            RootNode.Execute();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}