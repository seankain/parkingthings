using Godot;
using System;

public enum GameState
{
    LevelActive,
    LevelOver,
    Resetting
}
public partial class Game : Node
{
    public uint Level = 0;
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
                    State = GameState.LevelOver;
                    // Fill up the countdown timer
                    LevelOverRemainingSeconds = LevelOverTimeSeconds;
                    return;
                }
                // Do other active level stuff, managing scoring/npcs etc
                break;
            case GameState.LevelOver:
                LevelOverRemainingSeconds -= delta;
                if (LevelOverRemainingSeconds <= 0)
                {
                    // Done level over idling, start resetting level
                    State = GameState.Resetting;
                    return;
                }
                // Show the summary of pass or fail level
                break;
            case GameState.Resetting:
                ResetLevel();
                return;
            default:
                return;
        }
    }

    private void SwitchToSpectatorCamera()
    {

        var SpectatorCamera = GetNode<Camera3D>("SpectatorCamera");
        SpectatorCamera.MakeCurrent();

    }
    private void SwitchToPlayerCamera()
    {
        var playerCamera = GetNode("Player/Camera3D");
    }


    private void ResetLevel()
    {
        levelData = new LevelData();
        LevelRemainingSeconds = LevelDefaults.LevelDefaultTimeSeconds - (Level * LevelDefaults.LevelTimeDecrement);
        if (LevelRemainingSeconds <= 0)
        {
            // Force later levels to have minimum time to complete
            LevelRemainingSeconds = LevelDefaults.LevelDefaultMinTimeSeconds;
        }
        State = GameState.LevelActive;
    }

}
