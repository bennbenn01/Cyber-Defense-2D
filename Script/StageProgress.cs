using Godot;
using System;

public partial class StageProgress : Node
{
    public static StageProgress Instance;

    private const string SaveFilePath = "user://stageprogress.save";

    public bool Stage1Completed = false;
    public bool Stage2Completed = false;

    public override void _Ready()
    {
        Instance = this;
        LoadProgress();
    }

    public void SaveProgress()
    {
        using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);

        file.StoreLine(Stage1Completed ? "1" : "0");
        file.StoreLine(Stage2Completed ? "1" : "0");
    }

    public void LoadProgress()
    {
        if (!FileAccess.FileExists(SaveFilePath)) return;

        using var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);

        Stage1Completed = file.GetLine() == "1";
        Stage2Completed = file.GetLine() == "1";
    }
}