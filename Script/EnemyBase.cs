using Godot;
using System;

public partial class EnemyBase : CharacterBody2D
{
    public float Speed = 30.0f;
    public int Health = 0;
    public int InfiltrationDmg = 0;
    public int CoinReward = 0;

    public override void _PhysicsProcess(double delta)
	{
		var parentNode = GetParent() as PathFollow2D;
		if (parentNode != null)
		{
			parentNode.Progress += Speed * (float)delta;
			
			var path = parentNode.GetParent() as Path2D;
			if (path != null) 
			{
				if (parentNode.Progress >= path.Curve.GetBakedLength()) 
				{
					GameManager.Instance.ReduceHealth(InfiltrationDmg);
            		GameManager.Instance.RegisterInfiltration(); 					
					QueueFree();
				}	
				
				if (Health <= 0) 
				{
					GameManager.Instance.AddCoins(CoinReward);
					GameManager.Instance.RegisterEnemyDeaths();
					QueueFree();
				}
			}
		}
	}
}