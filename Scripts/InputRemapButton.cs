using Godot;

public partial class InputRemapButton : Button
{
	[Export] private StringName inputAction;

	private bool listening = false;

	public override void _Ready()
	{
		Pressed += StartListening;
	}

	private void StartListening()
	{
		if (listening)
			return;

		listening = true;
		Text = "Listening for input...";
	}

	public override void _Input(InputEvent @event)
	{
		if (!listening)
			return;

		if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
		{
			RemapInput(keyEvent);
			GetViewport().SetInputAsHandled();
		}
		else if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			RemapInput(mouseEvent);
			GetViewport().SetInputAsHandled();
		}
	}

	private void RemapInput(InputEvent inputEvent)
	{
		if (!InputMap.HasAction(inputAction))
		{
			GD.PrintErr("Input action does not exist: ", inputAction);
			StopListening();
			return;
		}

		InputMap.ActionEraseEvents(inputAction);
		InputMap.ActionAddEvent(inputAction, inputEvent);

		Text = inputEvent.AsText();

		listening = false;

		GD.Print(
			"Remapped ",
			inputAction,
			" to ",
			inputEvent.AsText()
		);
	}

	private void StopListening()
	{
		listening = false;
		Text = inputAction.ToString();
	}
}
