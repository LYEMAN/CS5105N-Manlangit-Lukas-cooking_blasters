using Godot;

public partial class HUD : CanvasLayer
{
	private ProgressBar healthBar;
	private Health health;

	public override void _Ready()
	{
		GD.Print("HUD READY");

		healthBar = GetNodeOrNull<ProgressBar>("HealthBar");

		if (healthBar == null)
		{
			GD.PrintErr("FAILED: HealthBar was not found.");
			return;
		}

		Node player = GetParent().GetParent();

		GD.Print("Player found: ", player.GetPath());

		health = player.GetNodeOrNull<Health>("Node2D");

		if (health == null)
		{
			GD.PrintErr("FAILED: Health was not found.");
			return;
		}

		GD.Print("Health found: ", health.GetPath());
		GD.Print("Current Health: ", health.CurrentHealth);
		GD.Print("Max Health: ", health.MaxHealth);

		healthBar.MinValue = 0;
		healthBar.MaxValue = health.MaxHealth;
		healthBar.Value = health.CurrentHealth;
	}

	public override void _Process(double delta)
	{
		if (health == null)
			return;

		healthBar.MaxValue = health.MaxHealth;
		healthBar.Value = health.CurrentHealth;
	}
}
