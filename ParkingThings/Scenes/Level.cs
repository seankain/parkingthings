using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node3D
{
    public uint LevelNumber = 1;
    public int Score = 0;

    public LevelData levelData;

    public GameState State;
    private GameState prevState;

    // How long to stay at level over
    public uint LevelOverTimeSeconds = 5;
    // This counts down when in level over state
    public double LevelOverRemainingSeconds = 0;

    public double LevelRemainingSeconds = LevelDefaults.LevelDefaultTimeSeconds;

    private Player player;

    private Spawner spawner;

    private Hud hud;
    private Menu menu;
    public override void _Ready()
    {
        levelData = new LevelData();
        var root = GetTree().Root;
        hud = root.GetNode<Hud>("/root/Level/Hud");
        menu = root.GetNode<Menu>("/root/Level/Menu");
        menu.OnPlayButtonPressed += (o, e) => { HideMenu(); };
        spawner = root.GetNode<Spawner>("/root/Level/Spawner");
        player = root.GetNode<Player>("/root/Level/Player");
        player.PlayerRespawned += (o, e) => { ResetLevel(); };
        GenerateObstacles();
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
            case GameState.Menu:
                return;
            case GameState.Resetting:
                return;
            default:
                return;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
                ShowMenu();
            }
        }
    }

    public void ShowMenu()
    {
        menu.Show();
        prevState = State;
        State = GameState.Menu;
        GetTree().Paused = true;
    }
    public void HideMenu()
    {
        menu.Hide();
        GetTree().Paused = false;
        State = prevState;
    }

    public void EndLevel()
    {
        hud.ShowMessage("Level Over");
        State = GameState.LevelOver;
        LevelOverRemainingSeconds = LevelOverTimeSeconds;
        var camControl = GetTree().Root.GetNode<CameraControl>("/root/Level/Player/SpringArm3D");
        camControl.StartIdleRotation();
        player.PauseInput();

    }


    private void ResetLevel()
    {
        hud.ShowMessage("GO!");
        hud.ClearScore();
        State = GameState.LevelActive;
        levelData = new LevelData();
        LevelRemainingSeconds = LevelDefaults.LevelDefaultTimeSeconds - (LevelNumber * LevelDefaults.LevelTimeDecrement);
        if (LevelRemainingSeconds <= 0)
        {
            // Force later levels to have minimum time to complete
            LevelRemainingSeconds = LevelDefaults.LevelDefaultMinTimeSeconds;
        }
        player.Respawn();
        player.ResumeInput();
        var camControl = GetTree().Root.GetNode<CameraControl>("/root/Level/Player/SpringArm3D");
        camControl.SnapToDefault();
        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        spawner.ClearVehicles();
        var nodes = GetTree().GetNodesInGroup("ParkingSpace");
        GD.Print($"Nodes count {nodes.Count}");
        var carsToGenerate = LevelNumber * LevelDefaults.VehicleIncrement;
        if (carsToGenerate > nodes.Count)
        {
            carsToGenerate = (uint)nodes.Count;
        }
        var spaceIdxs = new HashSet<int>();
        while (spaceIdxs.Count < carsToGenerate)
        {
            var val = Random.Shared.Next(0, nodes.Count);
            if (!spaceIdxs.Contains(val))
            {
                spaceIdxs.Add(val);
            }
        }
        foreach (var idx in spaceIdxs)
        {
            var npcNode = (Node3D)nodes[idx];
            spawner.SpawnNpcVehicle(npcNode.GlobalPosition + 3 * Vector3.Up, new Vector3(0, 90, 0));
        }
        // foreach (var n in nodes)
        // {
        //     GD.Print(n);
        //     var npcNode = (Node3D)n;
        //     spawner.SpawnNpcVehicle(npcNode.GlobalPosition + 3 * Vector3.Up, new Vector3(0, 90, 0));
        // }
    }

}
