using Godot;
using System;

public partial class Hud : CanvasLayer
{

    private Label timerLabel;
    private Label scoreLabel;

    private Label messageLabel;

    private Game game;

    public override void _Ready()
    {
        timerLabel = GetNode<Label>("Timer");
        scoreLabel = GetNode<Label>("Score");
        messageLabel = GetNode<Label>("Message");
    }

    public override void _Process(double delta)
    {
        game = GetNode<Game>("/root/Game");
        UpdateScore(game.Score);
        UpdateLevelTime(game.LevelRemainingSeconds);
    }


    public void ShowMessage(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();
        GetNode<Timer>("MessageTimer").Start();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = score.ToString();
    }

    public void UpdateLevelTime(double secondsRemaining)
    {
        timerLabel.Text = TimeSpan.FromSeconds(secondsRemaining).ToString();
    }
}
