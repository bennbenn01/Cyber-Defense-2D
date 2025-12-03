using Godot;
using System;

public partial class ReturnStartMenuPanel : Panel
{
	public override void _Ready()
	{
		this.GuiInput += _on_return_start_menu_panel_gui_input;
	}

	private void _on_return_start_menu_panel_gui_input(InputEvent @event)
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
		GameManager.Instance.ResetGame();
		GetTree().ChangeSceneToFile("res://Sprite/ui/start_menu.tscn");
	}	
}
