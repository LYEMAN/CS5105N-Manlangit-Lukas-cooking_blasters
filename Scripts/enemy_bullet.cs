
using Godot;

public partial class enemy_bullet : RigidBody2D
{
	[Export] private float damage = 10.0f;
	[Export] private float lifetime = 3.0f;

	private float lifeTimer = 0.0f;

	public override void _Ready()
	{
		AddToGroup("EnemyBullet");

		ContactMonitor = true;
		MaxContactsReported = 4;

		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		lifeTimer += (float)delta;

		if (lifeTimer >= lifetime)
		{
			QueueFree();
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Enemy"))
			return;

		if (!body.IsInGroup("Player"))
			return;

		Health health = body.GetNodeOrNull<Health>("Node2D");

		if (health == null)
			health = body as Health;

		if (health != null)
		{
			health.TakeDamage(damage);
		}
		else
		{
			GD.PrintErr("EnemyBullet: Could not find Health on ", body.GetPath());
		}

		QueueFree();
	}
}
