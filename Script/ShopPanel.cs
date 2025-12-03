using Godot;
using System;

public partial class ShopPanel : Panel
{
	public override void _Ready()
	{
		this.GuiInput += _on_menu_panel_gui_input;
	}	
	
	private void _on_menu_panel_gui_input(InputEvent @event)
	{
 		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				OpenShopPanel();
			}
		}		
	}
	
	private void OpenShopPanel()
	{
		var shop = GetNode<Control>("../Shop");

		if (shop == null)
		{
			GD.Print("Shop panel not found!");	
		}
		
		shop.Visible = !shop.Visible;
	}	
}
