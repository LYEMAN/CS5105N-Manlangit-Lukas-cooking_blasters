using Godot;

public partial class PauseMenu : CanvasLayer
{
	[Export] private PackedScene settingsScene;
	[Export] private PackedScene mainMenuScene;


	private Button resumeButton;
	private Button settingsButton;
	private Button mainMenuButton;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		resumeButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/Resume");
		settingsButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/Settings");
		mainMenuButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/MainMenu");

		resumeButton.Pressed += OnResumePressed;
		settingsButton.Pressed += OnSettingsPressed;
		mainMenuButton.Pressed += OnMainMenuPressed;
	}

	private void OnResumePressed()
	{
		GetTree().Paused = false;
		QueueFree();
	}

	private void OnSettingsPressed()
	{
		if (settingsScene == null)
		{
			GD.PrintErr("Settings scene has not been assigned.");
			return;
		}

		Visible = false;

		Control settingsMenu = settingsScene.Instantiate<Control>();

		GetTree().Root.AddChild(settingsMenu);

		settingsMenu.ProcessMode = ProcessModeEnum.Always;
		settingsMenu.SetMeta("pause_menu", this);
	}

	private void OnMainMenuPressed()
	{
		if (mainMenuScene == null)
		{
			GD.PrintErr("Main menu scene has not been assigned.");
			return;
		}

		GetTree().Paused = false;
		GetTree().ChangeSceneToPacked(mainMenuScene);
		QueueFree();
	}

}
