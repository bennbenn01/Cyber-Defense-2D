using Godot;
using System;

public partial class StartMenu : CanvasLayer
{
	public override void _Ready()
	{
		GetNode<Button>("Panel/VBoxContainer/StartButton").Pressed += StartGame;
		GetNode<Button>("Panel/VBoxContainer/ExitButton").Pressed += () => GetTree().Quit();
	}

	private void StartGame()
	{
		GetTree().ChangeSceneToFile("res://Sprite/ui/selection_menu.tscn");
	}
}
