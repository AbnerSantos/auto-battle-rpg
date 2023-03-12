namespace AutoBattleRPG.Scripts.BehaviorTree;

public abstract class ABtConditionalSelectorNode<T> : ABtNode<T>
{
    private readonly ABtNode<T> _ifFalse, _ifTrue;
    
    protected ABtConditionalSelectorNode(ABtNode<T> ifFalse, ABtNode<T> ifTrue)
    {
        _ifFalse = ifFalse;
        _ifTrue = ifTrue;
        Add(ifFalse!);
        Add(ifTrue!);
    }

    public override void Execute()
    {
        if (CheckCondition()) _ifTrue.Execute();
        else _ifFalse.Execute();
    }

    protected abstract bool CheckCondition();
}