using Godot;
using System;
using System.Linq;

public partial class LevelScoreHudElement : VBoxContainer
{
    [Export]
    public Label CarsHitValueLabel;
    [Export]
    public Label LivingThingsValueLabel;
    [Export]
    public Label OffroadingValueLabel;
    [Export]
    public Label InTheLinesValueLabel;
    [Export]
    public Label FinalGradeLabel;
    [Export]
    public AnimationPlayer anim;

    public override void _Ready()
    {
        base._Ready();
    }

    public void PlayScoreAnimation()
    {
        anim.Play("Rollout");
    }

    public void SetValueLabels(LevelData levelData)
    {
        CarsHitValueLabel.Text = levelData.CollisionEvents.Count(x => x == ObstacleType.Vehicle).ToString();
        // Would like to some day have the score card just show distinct counts for all collision types
        LivingThingsValueLabel.Text = levelData.CollisionEvents.Count(x => x == ObstacleType.Person || x == ObstacleType.Wildlife).ToString();
        OffroadingValueLabel.Text = TimeSpan.FromSeconds(levelData.OffroadingTime).ToString();
        InTheLinesValueLabel.Text = "Yes";
        if (levelData.OverLeftLine || levelData.OverRightLine)
        {
            InTheLinesValueLabel.Text = "NO";
        }
        if (levelData.OverLeftLine && levelData.OverRightLine)
        {
            InTheLinesValueLabel.Text = "HOW?!";
        }
        FinalGradeLabel.Text = $"{levelData.Grade}";
    }

}
