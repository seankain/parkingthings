using Godot;
using System;
using System.Linq;

public enum GameState
{
    LevelActive,
    LevelOver,
    Resetting,
    Menu
}
public partial class Game : Node
{

    [Export]
    public PackedScene LevelScene;

    private Hud hud;
    private Menu menu;

    private Level level;

    public override void _Ready()
    {
        var root = GetTree().Root;
        menu = root.GetNode<Menu>("/root/Main/Menu");
        menu.SetPlayMode();
        menu.OnPlayButtonPressed += (o, e) => { LoadLevel(); menu.Hide(); };
        menu.OnResumeButtonPressed += (o, e) => { level.Unpause(); menu.Hide(); };
        menu.Show();
    }

    public override void _Process(double delta)
    {

    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (Input.IsActionPressed("Pause"))
            {
                if (level.State == GameState.Menu)
                {
                    //already paused so unpausing with same input
                    level.Unpause();
                    menu.Hide();
                }
                else
                {
                    level.Pause();
                    menu.Show();
                }
            }
        }
    }

    private void LoadLevel()
    {
        var p = GD.Load<PackedScene>(LevelScene.ResourcePath);
        var level_instance = p.Instantiate();
        AddChild(level_instance);
        this.level = (Level)level_instance;

    }


}
