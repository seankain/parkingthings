using Godot;
using System;
using System.Linq;

public enum GameState
{
    LevelActive,
    LevelOver,
    Resetting
}
public partial class Game : Node
{
    public uint Level = 1;
    public int Score = 0;

    public LevelData levelData;

    public GameState State;

    // How long to stay at level over
    public uint LevelOverTimeSeconds = 5;
    // This counts down when in level over state
    public double LevelOverRemainingSeconds = 0;

    public double LevelRemainingSeconds = LevelDefaults.LevelDefaultTimeSeconds;
    public override void _Ready()
    {
        GD.Print("game script ready");
        levelData = new LevelData();
    }

    public override void _Process(double delta)
    {
        switch (State)
        {
            case GameState.LevelActive:
                LevelRemainingSeconds -= delta;
                // Ran out of time
                if (LevelRemainingSeconds <= 0)
                {
                    EndLevel();
                    return;
                }
                // Do other active level stuff, managing scoring/npcs etc
                break;
            case GameState.LevelOver:
                LevelOverRemainingSeconds -= delta;
                // GD.Print($"level over time {LevelOverRemainingSeconds}");
                if (LevelOverRemainingSeconds <= 0)
                {
                    // Done level over idling, start resetting level
                    ResetLevel();
                    return;
                }
                // Show the summary of pass or fail level
                break;
            case GameState.Resetting:
                return;
            default:
                return;
        }
    }

    public void EndLevel()
    {
        var hud = GetTree().Root.GetNode<Hud>("/root/Level/Hud");
        hud.ShowMessage("Level Over");
        State = GameState.LevelOver;
        LevelOverRemainingSeconds = LevelOverTimeSeconds;
        var camControl = GetTree().Root.GetNode<CameraControl>("/root/Level/Player/SpringArm3D");
        camControl.StartIdleRotation();
        var player = GetTree().Root.GetNode<Player>("/root/Level/Player");
        player.PauseInput();

    }


    private void ResetLevel()
    {
        var hud = GetTree().Root.GetNode<Hud>("/root/Level/Hud");
        hud.ShowMessage("GO!");
        State = GameState.LevelActive;
        levelData = new LevelData();
        LevelRemainingSeconds = LevelDefaults.LevelDefaultTimeSeconds - (Level * LevelDefaults.LevelTimeDecrement);
        if (LevelRemainingSeconds <= 0)
        {
            // Force later levels to have minimum time to complete
            LevelRemainingSeconds = LevelDefaults.LevelDefaultMinTimeSeconds;
        }
        var player = GetNode<Player>("/root/Level/Player");
        player.Respawn();
        player.ResumeInput();
        var camControl = GetTree().Root.GetNode<CameraControl>("/root/Level/Player/SpringArm3D");
        camControl.SnapToDefault();
    }

}
