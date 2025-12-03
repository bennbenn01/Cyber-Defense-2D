using Godot;
using System;

public partial class ClosePanel : Panel
{
	public override void _Ready()
	{
		this.GuiInput += _on_close_panel_gui_input;
	}

	private void _on_close_panel_gui_input(InputEvent @event)
	{
 		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				CloseShopPanel();
			}
		}		
	}

	private void CloseShopPanel()
	{
		var shop = GetNode<Control>("../../Shop");

		if (shop == null)
		{
			GD.Print("Shop panel not found!");	
		}
		
		shop.Visible = shop.Visible = false;
	}		
}
