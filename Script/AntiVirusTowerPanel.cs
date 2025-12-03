using Godot;
using System;

public partial class AntiVirusTowerPanel : Panel
{
	private PackedScene towerScene = GD.Load<PackedScene>("res://Sprite/tower/anti_virus_tower.tscn");
    private PackedScene previewScene = GD.Load<PackedScene>("res://Sprite/tower/anti_virus_tower_preview.tscn");

    private StaticBody2D preview;
    private bool dragging = false;

    private Node2D towersNode;
    private Area2D towerBaseArea;
    private Godot.Collections.Array<CollisionShape2D> towerBaseShapes;

    private const int TOWER_PRICE = 45;

    public override void _Ready()
    {
		towersNode = GetTree().CurrentScene.GetNode<Node2D>("Towers");
		towerBaseArea = GetTree().CurrentScene.GetNode<Area2D>("Metallic Tower Base/Area2D");        

        towerBaseShapes = new Godot.Collections.Array<CollisionShape2D>();
        foreach (Node child in towerBaseArea.GetChildren())
        {
            if (child is CollisionShape2D cs)
                towerBaseShapes.Add(cs);
        }

        this.GuiInput += _on_gui_input;
    }

    public override void _Process(double delta)
    {
        if (dragging && preview != null)
            preview.GlobalPosition = GetGlobalMousePosition();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!dragging) return;

        if (@event is InputEventMouseButton MB &&
			MB.ButtonIndex == MouseButton.Left &&
			MB.Pressed)
        {
            TowerPlacement();
        }

        if (@event is InputEventMouseButton MB2 &&
			MB2.ButtonIndex == MouseButton.Right &&
			MB2.Pressed) 
        {
            CancelDragTower();
        }
    }

    private void _on_gui_input(InputEvent @event)
    {
        // Start Drag of Tower
        if (@event is InputEventMouseButton MB1 && 
			MB1.ButtonIndex == MouseButton.Left &&
			MB1.Pressed	&& !dragging)
        {
			if (!GameManager.Instance.CanAfford(TOWER_PRICE))
			{
				GD.Print("Not enough coints to buy the tower!");
				return;
			}
			
			StartDragTower();
        }
    }

    private void StartDragTower()
    {
		dragging = true;
		
		var shopPanel = GetTree().Root.GetNode<Control>("Main/Shop/Shop");		
		if (shopPanel != null) shopPanel.Visible = false;		
		
		preview = towersNode.GetNodeOrNull<StaticBody2D>("AntiVirusTowerPreview");
		if (preview == null)
		{
			preview = previewScene.Instantiate<StaticBody2D>();
			preview.Name = "AntiVirusTowerPreview";
			towersNode.AddChild(preview);
		}		
		
		var sprite = preview.GetNode<Sprite2D>("AntiVirusTower");
		sprite.Modulate = new Color(1, 1, 1, 0.4f);		
    }

    private CollisionShape2D GetTowerBaseUnderMouse(Vector2 point)
    {
        foreach (var cs in towerBaseShapes)
        {
            if (cs.Shape != null)
            {
                var rect = GetCollisionShapeGlobalRect(cs);
                if (rect.HasPoint(point))
                    return cs;
            }
        }

        return null;
    }

    private Rect2 GetCollisionShapeGlobalRect(CollisionShape2D cs)
    {
        if (cs.Shape is RectangleShape2D rect)
        {
            Vector2 size = rect.Size;
            Vector2 pos = cs.GlobalPosition - size * 0.5f;
            return new Rect2(pos, size);
        }
        
        return new Rect2(cs.GlobalPosition, Vector2.One);
    }    

    private bool IsTowerAlreadyPlaced(CollisionShape2D selectedShape)
    {
        var baseRect = GetCollisionShapeGlobalRect(selectedShape);
        foreach (Node child in towersNode.GetChildren())
        {
            if (child is FirewallTower tower1)
            {
                if (baseRect.HasPoint(tower1.GlobalPosition))
                    return true;
            }

            if (child is AntiVirusTower tower2)
            {
                if (baseRect.HasPoint(tower2.GlobalPosition))
                    return true;  
            }

            if (child is IdsTower tower3)
            {
                if (baseRect.HasPoint(tower3.GlobalPosition))
                    return true;                  
            }	
        }

        return false;
    }

    private bool TowerPlacement()
    {
		Vector2 mousePosition = GetGlobalMousePosition();
		
        CollisionShape2D selectedShape = GetTowerBaseUnderMouse(mousePosition);
        if (selectedShape == null)
        {
            GD.Print("Cannot place tower here! Must be on Metallic Tower Base.");
            return false;
        }		
			
        if (IsTowerAlreadyPlaced(selectedShape))
        {
            GD.Print("Another tower already exists here!");
            return false;
        }
		
		if (!GameManager.Instance.CanAfford(TOWER_PRICE))
		{
			GD.Print("Not enough coins to place the tower!");
			CancelDragTower();
			return false;			
		}
		
		GameManager.Instance.SpendCoins(TOWER_PRICE);		
		
		if (preview.GetParent() != null)
			preview.GetParent().RemoveChild(preview); 
		
        preview?.QueueFree();
        preview = null;
        dragging = false;
		
		AntiVirusTower tower = towerScene.Instantiate<AntiVirusTower>();
        Rect2 rect = GetCollisionShapeGlobalRect(selectedShape);
		tower.GlobalPosition = rect.Position + rect.Size * 0.5f;
		
		towersNode.AddChild(tower);        

        return true;
    }

    private void CancelDragTower()
    {
		dragging = false;
		
		if (preview.GetParent() != null)
			preview.GetParent().RemoveChild(preview); 		
		
        preview?.QueueFree();
        preview = null;    
    }
}
