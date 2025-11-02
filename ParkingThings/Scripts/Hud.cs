using Godot;
using System;

public partial class Hud : CanvasLayer
{

    private Label timerLabel;
    private Timer messageTimer;
    private Label scoreLabel;

    private Label messageLabel;

    private Level level;

    private LevelScoreHudElement levelScore;

    public override void _Ready()
    {
        timerLabel = GetNode<Label>("Timer");
        messageTimer = GetNode<Timer>("MessageTimer");
        messageTimer.Timeout += () =>
        {
            messageLabel.Hide();
        };
        scoreLabel = GetNode<Label>("Score");
        messageLabel = GetNode<Label>("Message");
        levelScore = GetNode<LevelScoreHudElement>("LevelScoreVBox");
    }

    public override void _Process(double delta)
    {
        level = GetNode<Level>("/root/Main/Level");
        UpdateScore(level.Score);
        UpdateLevelTime(level.LevelRemainingSeconds);

    }

    public void ShowLevelScore()
    {
        levelScore.SetValueLabels(level.levelData);
        levelScore.Show();
        levelScore.PlayScoreAnimation();
    }

    public void HideLevelScore()
    {
        levelScore.Hide();
    }

    public void ShowMessage(string text)
    {
        messageLabel.Text = text;
        messageLabel.Show();
        messageTimer.Start(3.0);
        GetNode<Timer>("MessageTimer").Start();
    }

    public void UpdateScore(int score)
    {
        scoreLabel.Text = score.ToString();
    }

    public void ClearScore()
    {
        scoreLabel.Text = "0";
    }

    public void UpdateLevelTime(double secondsRemaining)
    {
        timerLabel.Text = TimeSpan.FromSeconds(secondsRemaining).ToString();
    }
}
