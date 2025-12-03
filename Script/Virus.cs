using Godot;
using System;

public partial class Virus : EnemyBase
{
	public Virus()
	{
		Speed = 30.0f;
		Health = 45;
		InfiltrationDmg = 3;
		CoinReward = 4;
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }	
}
