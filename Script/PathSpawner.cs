using Godot;
using System;
using System.Collections.Generic;

public partial class PathSpawner : Node2D
{
	private PackedScene stage1Scene = GD.Load<PackedScene>("res://Sprite/spawn/stage_1.tscn");
	private PackedScene stage2Scene = GD.Load<PackedScene>("res://Sprite/spawn/stage_2.tscn");
	private PackedScene stage3Scene = GD.Load<PackedScene>("res://Sprite/spawn/stage_3.tscn");
	private PackedScene endlessScene = GD.Load<PackedScene>("res://Sprite/spawn/endless.tscn");

	private Timer spawnTimer;
	
	private bool waveActive = false;
	private int enemiesSpawned = 0;
	private int enemiesPerWave = 35;

	public override void _Ready()
	{
		spawnTimer = GetNode<Timer>("Timer");
		spawnTimer.Timeout += SpawnEnemy;
		
		CallDeferred(nameof(StartWave)); 
	}
	
	public override void _Process(double delta)
	{
		if (!waveActive && GameManager.Instance.enemyDeaths == 0)
		{
			CallDeferred(nameof(StartWave));
		}
	}

	private PackedScene GetSelectedStageScene()
	{
		return GameManager.Instance.SelectedStage switch
		{
			1 => stage1Scene,
			2 => stage2Scene,
			3 => stage3Scene,
			0 => endlessScene,
			_ => stage1Scene
		};
	}

	private List<string> GetAllowedEnemiesForStage(int stage)
	{
		return stage switch
		{
			1 => new List<string> { "Malware" },
			2 => new List<string> { "Malware", "Virus" },
			3 => new List<string> { "Malware", "Virus", "Ransomware" },
			0 => new List<string> { "Malware", "Virus", "Ransomware" },
			_ => new List<string> { "Malware" }
		};
	}

	private void StartWave()
	{
		if (GameManager.Instance == null)
		{
			GD.Print("Waiting for GameManager...");
			CallDeferred(nameof(StartWave));
			return;
		}
		
		if (GameManager.Instance.startWave > GameManager.Instance.endWave)
		{
			GD.Print("Defended successfully.");
			spawnTimer.Stop();
			return;			
		}	
		
		waveActive = true;
		enemiesSpawned = 0;
		
		GD.Print($"Starting Wave {GameManager.Instance.startWave}");
		
		spawnTimer.Start();
	}
	
	private void SpawnEnemy()
	{
		if (!waveActive) return;
		
		if (enemiesSpawned >= enemiesPerWave)
		{
 			waveActive = false;
			spawnTimer.Stop();
			GD.Print($"Wave {GameManager.Instance.startWave} spawn complete.");
			return;
		}
		
		Node2D tempStage = (Node2D)GetSelectedStageScene().Instantiate();
		AddChild(tempStage);
		
		PathFollow2D pathFollow = tempStage.GetNode<PathFollow2D>("PathSpawner");
		if (pathFollow == null)
		{
			GD.Print("PathFollow2D not found in stage scene!");
			return;
		}

		Random rng = new Random();
		List<string> allowedEnemies = GetAllowedEnemiesForStage(GameManager.Instance.SelectedStage);
        string enemyName = allowedEnemies[rng.Next(allowedEnemies.Count)];

        string path = $"res://Sprite/enemies/{enemyName.ToLower()}.tscn";
        EnemyBase enemyInstance = (EnemyBase)GD.Load<PackedScene>(path).Instantiate();

        pathFollow.AddChild(enemyInstance);

		if (GameManager.Instance.SelectedStage >= 1 && GameManager.Instance.SelectedStage <= 3) {
			int wave = GameManager.Instance.startWave;
			enemyInstance.Health += (wave - 1) * 15;
		}

        GD.Print($"Spawned {enemyInstance.GetType().Name} with HP: {enemyInstance.Health}");

		enemiesSpawned++;
	}
}
