using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public virtual void Execute(GameActor actor) { }
    public virtual void Undo(GameActor actor) { }
}

public class JumpCommand : Command
{
    public override void Execute(GameActor actor)
    {
        actor.Jump();
    }    
}

public class FireCommand : Command
{
    public override void Execute(GameActor actor)
    {
        actor.FireGun();
    }   
}

public class SwapWeaponCommand : Command
{
    public override void Execute(GameActor actor)
    {
        actor.SwapWeapon();
    }  
}

public class SquatCommand : Command
{
    public override void Execute(GameActor actor)
    {
        actor.Squat();
    }
}

public class MoveCommand : Command
{
    private float x;
    private float y;
    private float z;

    public MoveCommand(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override void Execute(GameActor actor)
    {
        Vector3 targetPos = new Vector3(x, y, z);
        actor.MoveTo(targetPos);
    }
}
