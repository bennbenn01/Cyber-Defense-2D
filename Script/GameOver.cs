using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	private Label msgLabel;
	private Button returnMenuBtn;

	public override void _Ready()
	{
		msgLabel = GetNode<Label>("Panel/Label");
		returnMenuBtn = GetNode<Button>("Panel/Button");

		msgLabel.Text = GameManager.Instance.Message;
		returnMenuBtn.Pressed += ReturnMenu;
	}

	private void ReturnMenu()
	{
		GetTree().ChangeSceneToFile("res://Sprite/ui/start_menu.tscn");
	}
}
