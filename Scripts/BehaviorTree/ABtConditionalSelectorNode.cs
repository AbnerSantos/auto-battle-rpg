namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class ABtConditionalSelectorNode<T> : ABtNode<T>
{
    private readonly ABtNode<T> IfFalse, IfTrue;
    
    protected ABtConditionalSelectorNode(ABtNode<T> ifFalse, ABtNode<T> ifTrue)
    {
        IfFalse = ifFalse;
        IfTrue = ifTrue;
        Add(ifFalse!);
        Add(ifTrue!);
    }

    public override void Execute()
    {
        if (CheckCondition()) IfTrue.Execute();
        else IfFalse.Execute();
    }

    protected abstract bool CheckCondition();
}