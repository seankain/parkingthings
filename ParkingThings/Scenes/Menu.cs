using Godot;
using System;

public delegate void PlayButtonPressed(object sender, EventArgs e);
public partial class Menu : Control
{

    [Export]
    public Button QuitButton;

    [Export]
    public Button PlayButton;

    public PlayButtonPressed OnPlayButtonPressed;

    public override void _Ready()
    {
        QuitButton.Pressed += OnQuitButtonPressed;
        PlayButton.Pressed += () => { OnPlayButtonPressed?.Invoke(this, EventArgs.Empty); };
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

}
