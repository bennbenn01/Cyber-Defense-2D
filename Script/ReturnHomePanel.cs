using Godot;
using System;

public partial class ReturnHomePanel : Panel
{
	public override void _Ready()
	{
		this.GuiInput += _on_return_home_panel_gui_input;
	}

	private void _on_return_home_panel_gui_input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				ReturnToStartMenu();
			}
		}
	}

	private void ReturnToStartMenu()
	{
		if (GameManager.Instance != null)
			GameManager.Instance.ResetGame();

		GetTree().ChangeSceneToFile("res://Sprite/ui/start_menu.tscn");
	}
}
