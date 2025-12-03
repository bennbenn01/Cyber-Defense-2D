using Godot;

public partial class TowerManager : Node
{
    public static TowerManager Instance;
    public StaticBody2D CurrentSelectedTower;

    public override void _Ready()
    {
        Instance = this;
    }

  public void SelectTower(StaticBody2D tower)
    {
        if (CurrentSelectedTower != null && CurrentSelectedTower != tower)
        {
            // Hide previous tower's button
            Button prevButton = CurrentSelectedTower.GetNodeOrNull<Button>("DeleteButton");
            if (prevButton != null)
                prevButton.Visible = false;
        }

        CurrentSelectedTower = tower;

        Button newButton = tower.GetNodeOrNull<Button>("DeleteButton");
        if (newButton != null)
            newButton.Visible = !newButton.Visible;

        if (newButton != null && !newButton.Visible)
            CurrentSelectedTower = null;
    }

    public void DeselectCurrentTower()
    {
        if (CurrentSelectedTower != null)
        {
            Button button = CurrentSelectedTower.GetNodeOrNull<Button>("DeleteButton");
            if (button != null)
                button.Visible = false;

            CurrentSelectedTower = null;
        }
    }
}
