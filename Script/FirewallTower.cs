using Godot;
using System;
using System.Collections.Generic;

public partial class FirewallTower : StaticBody2D
{
	private PackedScene fireball = GD.Load<PackedScene>("res://Sprite/projectile/fireball.tscn");
	private int bulletDamage = 2;

	private List<Node> currentTargets = new();
	private Node2D currentTarget;

	private bool projectileInAir = false;

	private Button deleteButton;

	public override void _Ready()
	{
		deleteButton = GetNode<Button>("DeleteButton");
		deleteButton.Pressed += on_delete_button_pressed;
	}

	public override void _Process(double delta)
	{
		currentTargets.RemoveAll(n => n == null || !IsInstanceValid(n));

		if (currentTarget == null || !IsInstanceValid(currentTarget))
			SelectTarget();

		if (currentTarget != null && !projectileInAir)
			FireProjectile();
	}

	private void SelectTarget()
	{
		currentTarget = null;

		foreach (var node in currentTargets)
		{
			if (node is Malware m)
			{
				currentTarget = m;
				break;
			}
			else if (node is Virus v)
			{
				currentTarget = v;
				break;
			}
			else if (node is Ransomware r)
			{
				currentTarget = r;
				break;
			}
			else if (node is PathFollow2D pf && pf.GetChildCount() > 0)
			{
				var child = pf.GetChild(0); 
				if (child is Malware m2)
				{
					currentTarget = m2;
					break;					
				} 
				else if (child is Virus v2)
				{
					currentTarget = v2;
					break;
				}
				else if (child is Ransomware r2)
				{
					currentTarget = r2;
					break;
				}
			}
		}
	}

	private void FireProjectile()
	{
		projectileInAir = true; 

		Fireball proj = (Fireball)fireball.Instantiate();
		proj.TargetNode = currentTarget;
		proj.projectileDamage = bulletDamage;

		Marker2D aim = GetNode<Marker2D>("Aim");
		proj.GlobalPosition = aim.GlobalPosition;
		proj.LookAt(currentTarget.GlobalPosition);

		GetNode("FireballContainer").CallDeferred("add_child", proj);

		proj.Connect("tree_exited", Callable.From(() =>
		{
			projectileInAir = false;
		}));
	}

	public override void _Input(InputEvent @event)
	{
		if (GameManager.Instance.IsPlacingTower) return;

		if (GetViewport().GuiGetHoveredControl() != null) return;		

		if (@event is InputEventMouseButton mb && mb.Pressed 
		&& mb.ButtonIndex == MouseButton.Left)
		{
			Vector2 mousePos = GetGlobalMousePosition();
			bool insideTower = GetGlobalRect().HasPoint(mousePos);

			if (!insideTower)
			{
				deleteButton.Visible = false;
				return;
			}

			if (!deleteButton.Visible)
			{
				if (!IsCloseToTower(mousePos)) return;
			}

			deleteButton.Visible = !deleteButton.Visible;
		}
	}

	public Rect2 GetGlobalRect()
	{
		Sprite2D sprite = GetNode<Sprite2D>("FirewallTower");

		if (sprite == null || sprite.Texture == null)
			return new Rect2(GlobalPosition, Vector2.Zero);

		Vector2 size = sprite.Texture.GetSize();
		Vector2 pos = GlobalPosition - (size * 0.5f);

		return new Rect2(pos, size);
	}	

	private bool IsCloseToTower(Vector2 mousePos)
	{
		Node2D towersNode = GetTree().CurrentScene.GetNode<Node2D>("Towers");

		float myDistance = GlobalPosition.DistanceTo(mousePos);

		foreach (Node child in towersNode.GetChildren())
		{
			if (child is FirewallTower other && other != this)
			{
				float otherDist = other.GlobalPosition.DistanceTo(mousePos);

				if (otherDist < myDistance)
					return false;
			}
		}

		return true;
	}

	private void on_delete_button_pressed()
	{
		QueueFree();
	}

	private void _on_tower_body_entered(Node body)
	{
		if (
			body is Malware or Virus or Ransomware && 
			!currentTargets.Contains(body)
		)
			currentTargets.Add(body);
	}

	private void _on_tower_body_exited(Node body)
	{
		if (currentTargets.Contains(body))
			currentTargets.Remove(body);
			
		if (body == currentTarget)
			currentTarget = null;
	}
}
