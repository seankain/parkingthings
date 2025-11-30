using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node3D
{
	public uint LevelNumber = 1;
	public int Score = 0;

	public bool PlayerInSpace = false;

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

	private bool menuVisble = false;
	public override void _Ready()
	{
		levelData = new LevelData();
		var root = GetTree().Root;
		hud = root.GetNode<Hud>("/root/Main/Level/Hud");
		menu = root.GetNode<Menu>("/root/Main/Menu");
		menu.SetResumeMode();
		spawner = root.GetNode<Spawner>("/root/Main/Level/Spawner");
		player = root.GetNode<Player>("/root/Main/Level/Player");
		player.PlayerRespawned += (o, e) => { ResetLevel(); };
		player.PlayerHitObstacle += (o, e) => { HandlePlayerObstacleHit(e); };
		GenerateObstacles();
		//ShowMenu();
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
					if (levelData.GradeAsNumeric <= 2)
					{
						NextLevel();
					}
					else
					{
						ResetLevel();
					}
					
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
	
	private void HandlePlayerObstacleHit(PlayerHitObstacleArgs e)
	{
		levelData.CollisionEvents.Add(e.ObstacleType);
	}

	public void Pause()
	{
		prevState = State;
		State = GameState.Menu;
		GetTree().Paused = true;
	}
	public void Unpause()
	{
		GetTree().Paused = false;
		State = prevState;
	}

	public void EndLevel()
	{
		//hud.ShowMessage("Level Over");
		State = GameState.LevelOver;
		LevelOverRemainingSeconds = LevelOverTimeSeconds;
		var camControl = GetTree().Root.GetNode<CameraControl>("/root/Main/Level/Player/SpringArm3D");
		camControl.StartIdleRotation();
		player.PauseInput();
		if (GetPlayerInSpace())
		{
			hud.ShowLevelScore();
		}
		else
		{
			hud.ShowMessage("FAILED TO PARK");
		}
	}

	private void ResetLevel()
	{
		hud.ShowMessage("GO!");
		hud.ClearScore();
		State = GameState.LevelActive;
		levelData = new LevelData();
		LevelRemainingSeconds = ((double)LevelDefaults.LevelDefaultTimeSeconds) - ((double)(LevelNumber * LevelDefaults.LevelTimeDecrement));
		if (LevelRemainingSeconds <= LevelDefaults.LevelDefaultMinTimeSeconds)
		{
			// Force later levels to have minimum time to complete
			LevelRemainingSeconds = LevelDefaults.LevelDefaultMinTimeSeconds;
		}
		player.Respawn();
		player.ResumeInput();
		var camControl = GetTree().Root.GetNode<CameraControl>("/root/Main/Level/Player/SpringArm3D");
		camControl.SnapToDefault();
		GenerateObstacles();
		hud.HideLevelScore();
	}

	private void NextLevel()
	{
		LevelNumber += 1;
		levelData = new();
		hud.ShowMessage("GO!");
		hud.ClearScore();
		State = GameState.LevelActive;
		levelData = new LevelData();
		LevelRemainingSeconds = ((double)LevelDefaults.LevelDefaultTimeSeconds) - ((double)(LevelNumber * LevelDefaults.LevelTimeDecrement));
		GD.Print($"Level Time: {LevelRemainingSeconds}");
		if (LevelRemainingSeconds <= 0)
		{
			GD.Print($"level remaining seconds less than zero, setting to default of {LevelRemainingSeconds}");
			// Force later levels to have minimum time to complete
			LevelRemainingSeconds = LevelDefaults.LevelDefaultMinTimeSeconds;
		}
		player.Respawn();
		player.ResumeInput();
		var camControl = GetTree().Root.GetNode<CameraControl>("/root/Main/Level/Player/SpringArm3D");
		camControl.SnapToDefault();
		GenerateObstacles();
		hud.HideLevelScore();
	}

	private void GenerateObstacles()
	{
		spawner.ClearVehicles();
		var nodes = GetTree().GetNodesInGroup("ParkingSpace");
		var carsToGenerate = LevelNumber * LevelDefaults.VehicleIncrement;
		if (carsToGenerate > nodes.Count)
		{
			carsToGenerate = (uint)nodes.Count - 1;
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
			spawner.SpawnNpcVehicle(npcNode.GlobalPosition + 1 * Vector3.Up, new Vector3(0, 90, 0));
		}
		// foreach (var n in nodes)
		// {
		//     GD.Print(n);
		//     var npcNode = (Node3D)n;
		//     spawner.SpawnNpcVehicle(npcNode.GlobalPosition + 3 * Vector3.Up, new Vector3(0, 90, 0));
		// }
	}

	private void RandomEvent()
    {
		var parkingNodes = GetTree().GetNodesInGroup("ParkingSpace");
		var parkingSpace = parkingNodes[Random.Shared.Next(0,parkingNodes.Count)];
		var location = (Node3D)parkingSpace.GetNode("NpcSpawnPoint");
        spawner.SpawnNpcHuman(location.GlobalPosition,location.GlobalRotationDegrees);
    }

	private bool GetPlayerInSpace()
	{
		var nodes = GetTree().GetNodesInGroup("ParkingSpace");
		foreach (var n in nodes)
		{
			if (((ParkingSpace)n).PlayerInBounds) { return true; }
		}
		return false;

	}

}
