using Godot;
using System;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public int Coins = 100;
	public int Health = 100;
	public int startWave;
	public int endWave;

	private Label _coinsLabel;
	private Label _healthLabel;
	private Label _waveLabel;

	public override void _Ready()
	{
		Instance = this;
		
		_coinsLabel = GetNode<Label>("UI/Panel/CoinsLabel");
		_healthLabel = GetNode<Label>("UI/Panel/HealthLabel");
		_waveLabel = GetNode<Label>("UI/Panel/WaveLabel");

		UpdateUI();
	}
	
	public bool CanAfford(int cost)
	{
		return Coins >= cost;
	}	
	
	public void AddCoins(int amount)
	{
		Coins += amount;
		UpdateCoins();
	}
		
	public void SpendCoins(int amount)
	{
		Coins -= amount;
		UpdateCoins();
	}	

	public void ReduceHealth(int amount)
	{
		Health -= amount;
		if (Health < 0)
			Health = 0;

		UpdateHealth();
	}
	
	public void SetWave(int startWave, int endWave)
	{


		UpdateWave();
	}
	
	public void GetWave(int startWave, int endWave)
	{

		// Just getting the wave, no need to update the wave UI
		//UpdateWave();
	}

	private void UpdateCoins() => _coinsLabel.Text = $"Coins: {Coins}";
	private void UpdateHealth() => _healthLabel.Text = $"Health: {Health}%";
	private void UpdateWave() => _waveLabel.Text = $"Wave: {startWave} / {endWave}";
	private void UpdateUI()
	{
		UpdateCoins();
		UpdateHealth();
		UpdateWave();
	}
}
