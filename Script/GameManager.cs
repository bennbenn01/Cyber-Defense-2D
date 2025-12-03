using Godot;
using System;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public int SelectedStage = 1;

	public int Coins = 100;
	public int Health = 100;
	
	public int startWave = 1;
	public int endWave = 3;

	public int enemyDeaths = 0;
	public int enemyInfiltrated = 0;
	public int deathsPerWave = 35;

	private Label _coinsLabel;
	private Label _healthLabel;
	private Label _waveLabel;
	
	private bool gameStop = false;
	public string Message = "";

	public bool IsPlacingTower = false;

	private PackedScene endScene = GD.Load<PackedScene>("res://Sprite/ui/game_over.tscn");

	public override void _Ready()
	{
        if (Instance == null)
            Instance = this;
        else
        {
            QueueFree();
            return;
        }
	}

	public void InitUI()
	{
    	Node currentScene = GetTree().CurrentScene;
    	if (currentScene == null) return;

		_coinsLabel = currentScene.GetNodeOrNull<Label>("UI/Panel/CoinsLabel");
		_healthLabel = currentScene.GetNodeOrNull<Label>("UI/Panel/HealthLabel");
		_waveLabel = currentScene.GetNodeOrNull<Label>("UI/Panel/WaveLabel");

    	UpdateUI();
	}
	
	public bool CanAfford(int cost)
	{
		return Coins >= cost;
	}	
	
	public void AddCoins(int amount)
	{
		Coins += amount;
		UpdateUI();
	}
		
	public void SpendCoins(int amount)
	{
		Coins -= amount;
		UpdateUI();
	}	

	public void ReduceHealth(int amount)
	{
		if (gameStop) return;
		
		Health -= amount;
		if (Health < 0) Health = 0;

		if (Health <= 0) GameOver("Failed!");

		UpdateUI();
	}
	
	public void SetWave(int start, int end)
	{
		startWave = start;
		endWave = end;
		
		UpdateUI();
	}
	
	public void GetWave(int start, int end)
	{
		start = startWave;
		end = endWave;
	
	    UpdateUI();
	}	
	
	public void RegisterEnemyDeaths()
	{
		if (gameStop) return;
		
		enemyDeaths++;
		GD.Print($"Enemy killed: {enemyDeaths}/{deathsPerWave}");	
		
		CheckWaveCompletion();
		UpdateUI();
	}

	public void RegisterInfiltration()
	{
		if (gameStop) return;

		enemyInfiltrated++;
		GD.Print($"Enemy Infiltrated: {enemyInfiltrated}/{deathsPerWave}");

		CheckWaveCompletion();
	}

	public void CheckWaveCompletion()
	{
		if ((enemyDeaths + enemyInfiltrated) >= deathsPerWave)
		{
			if (Health > 0 && startWave >= endWave)
			{
				GD.Print("Defended successfully!");
				GameOver("Defended successfully!");
			}
			else if (Health > 0)
			{
				AdvanceWave();
			}
			else 
			{
				GD.Print("Game Over! Health reached 0.");
				GameOver("Game Over! Failed to defended");				
			}
		}
	}
	
	public bool AdvanceWave()
	{
		if (gameStop) return true;
		
		enemyDeaths = 0;
		enemyInfiltrated = 0;
		startWave++;
		
		if (startWave > endWave)
		{
			GD.Print("Stage defended successfully!");
			return true;
		}
		
		UpdateUI();
		return false;
	}
	
	private void GameOver(string msg)
	{
		if (gameStop) return;
		gameStop = true;
		
		GD.Print(msg);

		Message = msg;

		if (msg == "Defended successfully!")
		{
			if (!StageProgress.Instance.Stage1Completed)
			{
				StageProgress.Instance.Stage1Completed = true;
				StageProgress.Instance.SaveProgress();
				GD.Print("Stage 1 completed and progress saved.");
			} 
			else if (!StageProgress.Instance.Stage2Completed) 
			{
				StageProgress.Instance.Stage2Completed = true;
				StageProgress.Instance.SaveProgress();
				GD.Print("Stage 2 completed and progress saved.");
			}
		}
		
		GetTree().CallDeferred("change_scene_to_packed", endScene);
	}

	private void UpdateUI()
	{
   		if (IsInstanceValid(_coinsLabel)) _coinsLabel.Text = $"Coins: {Coins}";
        if (IsInstanceValid(_healthLabel)) _healthLabel.Text = $"Health: {Health}%";
        if (IsInstanceValid(_waveLabel)) _waveLabel.Text = $"Wave: {startWave} / {endWave}";
	}	

	public void ResetGame()
	{
		Coins = 100;
		Health = 100;

		startWave = 1;
		endWave = 3;

		enemyDeaths = 0;
		enemyInfiltrated = 0;
		gameStop = false;
		
		Message = "";
	}
}
