using AutoBattleRPG.Scripts.Character;
using AutoBattleRPG.Scripts.Stage;
using AutoBattleRPG.Scripts.Utility;

namespace AutoBattleRPG.Scripts.BehaviorTree;

public class MoveTowardsTargetNode : ABtNode<RpgBtData>
{
    public override void Execute()
    {
        if (BtData == null) throw new Exceptions.BtDataIsNull();

        ACharacter character = BtData.Character;
        character.ResetMovement();
        
        // Move to the nearest reachable target
        foreach (ACharacter target in BtData.TargetsByAscendingDistance)
        {
            Tile currentTile = character.CurrentTile!;
            List<(int x, int y)>? path = character.Pathfinder.FindPath((currentTile.X, currentTile.Y), (target.CurrentTile!.X, target.CurrentTile!.Y));
            
            if (path == null) continue;

            character.MoveTowardsPath(path, target);
            return;
        }

        Console.WriteLine("No path to any available targets!");
    }
}