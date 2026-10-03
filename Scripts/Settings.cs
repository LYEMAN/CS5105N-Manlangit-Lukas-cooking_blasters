using Godot;

public partial class Settings : Control
{
	private Button returnButton;
	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;

		returnButton = GetNodeOrNull<Button>("MarginContainer/VBoxContainer/Button");

		if (returnButton == null)
		{
			GD.PrintErr("Return button was not found.");
			return;
		}

		returnButton.Pressed += OnReturnPressed;
	}

	private void OnReturnPressed()
	{
		CanvasLayer pauseMenu = GetMeta("pause_menu").AsGodotObject() as CanvasLayer;

		if (pauseMenu != null && IsInstanceValid(pauseMenu))
		{
			pauseMenu.Show();
		}

		QueueFree();
	}

}
