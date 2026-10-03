using Godot;

public partial class Pause : Node2D
{
	[Export] private PackedScene pauseScene;


	private CanvasLayer pauseMenu;
	private bool isPaused = false;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			TogglePause();
			GetViewport().SetInputAsHandled();
		}
	}

	private void TogglePause()
	{
		if (isPaused)
		{
			ResumeGame();
		}
		else
		{
			OpenPauseMenu();
		}
	}

	private void OpenPauseMenu()
	{
		if (pauseScene == null)
		{
			GD.PrintErr("Pause scene has not been assigned.");
			return;
		}

		pauseMenu = pauseScene.Instantiate<CanvasLayer>();

		if (pauseMenu == null)
		{
			GD.PrintErr("Failed to instantiate pause scene.");
			return;
		}

		GetTree().Root.AddChild(pauseMenu);

		pauseMenu.Layer = 100;
		pauseMenu.ProcessMode = ProcessModeEnum.Always;

		isPaused = true;
		GetTree().Paused = true;

		GD.Print("Pause menu opened.");
	}

	private void ResumeGame()
	{
		GetTree().Paused = false;

		if (IsInstanceValid(pauseMenu))
			pauseMenu.QueueFree();

		pauseMenu = null;
		isPaused = false;

		GD.Print("Game resumed.");
	}


}
