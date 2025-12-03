using Godot;
using System;

public partial class SelectionMenu : CanvasLayer
{
	private Button stage1Btn;
	private Button stage2Btn;
	private Button stage3Btn;
	private Button endlessBtn;

	public override void _Ready()
	{
		stage1Btn = GetNode<Button>("Panel/VBoxContainer/Stage1Button");
		stage2Btn = GetNode<Button>("Panel/VBoxContainer/Stage2Button");
		stage3Btn = GetNode<Button>("Panel/VBoxContainer/Stage3Button");
		endlessBtn = GetNode<Button>("Panel/VBoxContainer/EndlessStageButton");

		CallDeferred(nameof(UpdateLockState));
		StageButtons();
	}

	private void UpdateLockState()
	{
		stage1Btn.Disabled = false;

		stage2Btn.Disabled = !StageProgress.Instance.Stage1Completed;
		stage3Btn.Disabled = !StageProgress.Instance.Stage2Completed;

		endlessBtn.Disabled = false;
	}

	private void StageButtons()
	{
		stage1Btn.Pressed += () => LoadStage(1);
		stage2Btn.Pressed += () => LoadStage(2);
		stage3Btn.Pressed += () => LoadStage(3);
		endlessBtn.Pressed += () => LoadEndless(0);
	}

	private void LoadStage(int stage)
	{
		GameManager.Instance.SelectedStage = stage;
		GameManager.Instance.SetWave(1, 3);
			
		GetTree().ChangeSceneToFile("res://main.tscn");
	}

	private void LoadEndless(int stage)
	{
		GameManager.Instance.SelectedStage = stage;
		GameManager.Instance.SetWave(1, 9999);
		GetTree().ChangeSceneToFile("res://main.tscn");
	}
}
