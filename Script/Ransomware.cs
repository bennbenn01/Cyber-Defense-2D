using Godot;
using System;

public partial class Ransomware : EnemyBase
{
	public Ransomware()
	{
		Speed = 30.0f;
		Health = 55;
		InfiltrationDmg = 5;
		CoinReward = 6;
	}

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }	
}
