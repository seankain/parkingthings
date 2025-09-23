using Godot;
using System;
using System.ComponentModel.Design;

public delegate void PlayButtonPressed(object sender, EventArgs e);
public delegate void ResumeButtonPressed(object sender, EventArgs e);
public partial class Menu : CanvasLayer
{

    [Export]
    public Button QuitButton;

    [Export]
    public Button PlayButton;

    public PlayButtonPressed OnPlayButtonPressed;

    public ResumeButtonPressed OnResumeButtonPressed;

    // Using the same menu for both in game and main menu so this changes based on if we're in level or not
    // the play button vs resume button cause different actions downstream. Originally I wanted to have a hidden
    // extra button in the vcontainer but I wasn't sure how to get the positioning right so I'm just going to 
    // go with this until it doesnt work
    private bool playMode = true;

    public override void _Ready()
    {
        QuitButton.Pressed += OnQuitButtonPressed;
        PlayButton.Pressed += () => { if (playMode) { OnPlayButtonPressed?.Invoke(this, EventArgs.Empty); } else { OnResumeButtonPressed?.Invoke(this, EventArgs.Empty); } };
    }


    public void SetResumeMode()
    {
        PlayButton.Text = "Resume";
        playMode = false;

    }
    public void SetPlayMode()
    {
        PlayButton.Text = "Play";
        playMode = true;
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

}
