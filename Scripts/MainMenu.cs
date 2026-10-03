using Godot;

public partial class MainMenu : Control
{
	[Export] private PackedScene startScene;
	[Export] private PackedScene settingsScene;

	private Button startButton;
	private Button settingsButton;
	private Button exitButton;

	public override void _Ready()
	{
		startButton = GetNode<Button>("MarginContainer/VBoxContainer/StartButton");
		settingsButton = GetNode<Button>("MarginContainer/VBoxContainer/Settings");
		exitButton = GetNode<Button>("MarginContainer/VBoxContainer/Exit");

		startButton.Pressed += OnStartPressed;
		settingsButton.Pressed += OnSettingsPressed;
		exitButton.Pressed += OnExitPressed;
	}

	private void OnStartPressed()
	{
		if (startScene == null)
		{
			GD.PrintErr("Start scene has not been assigned.");
			return;
		}

		GetTree().ChangeSceneToPacked(startScene);
	}

	private void OnSettingsPressed()
	{
		if (settingsScene == null)
		{
			GD.PrintErr("Settings scene has not been assigned.");
			return;
		}

		GetTree().ChangeSceneToPacked(settingsScene);
	}

	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
