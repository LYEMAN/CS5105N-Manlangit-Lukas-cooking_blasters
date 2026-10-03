
using Godot;

public partial class Health : Node2D
{
	[Signal]
	public delegate void HealthChangedEventHandler(float currentHealth, float maxHealth);

	[Export] private float maxHealth = 100.0f;
	[Export] private float currentHealth = 100.0f;

	public float MaxHealth => maxHealth;
	public float CurrentHealth => currentHealth;

	public override void _Ready()
	{
		currentHealth = maxHealth;

		GD.Print("Health initialized: ", currentHealth);

		EmitSignal(
			SignalName.HealthChanged,
			currentHealth,
			maxHealth
		);
	}

	public void TakeDamage(float damage)
	{
		GD.Print("Taking damage: ", damage);
		GD.Print("Health before: ", currentHealth);

		currentHealth -= damage;

		if (currentHealth < 0.0f)
			currentHealth = 0.0f;

		GD.Print("Health after: ", currentHealth);

		EmitSignal(
			SignalName.HealthChanged,
			currentHealth,
			maxHealth
		);

		if (currentHealth <= 0.0f)
			Die();
	}

	private void Die()
	{
		GD.Print("Player died");
		GetParent().GetParent().QueueFree();
	}
}
