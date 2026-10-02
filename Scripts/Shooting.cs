
using Godot;
using System;

public partial class Shooting : Sprite2D
{
	[Export] PackedScene bullet;
	[Export] PackedScene explosion;
	[Export] float speed = 300.0f;
	[Export] float bps = 5.0f;
	[Export] float bulletLifetime = 2.0f;

	[Export] float lineLength = 100.0f;
	[Export] float markerDistance = 50.0f;

	private float fireRate;
	private float rateLimit = 0.0f;

	private Line2D _aimLine;
	private Marker2D _bulletSpawnMarker;

	public override void _Ready()
	{
		_aimLine = GetNode<Line2D>("../Line2D");
		_bulletSpawnMarker = GetNode<Marker2D>("../Marker2D");

		fireRate = 1.0f / bps;
	}

	public override void _Process(double delta)
	{
		Vector2 mousePos = GetGlobalMousePosition();
		Vector2 direction = (mousePos - GlobalPosition).Normalized();

		_aimLine.ClearPoints();
		_aimLine.AddPoint(Vector2.Zero);
		_aimLine.AddPoint(direction * lineLength);

		_bulletSpawnMarker.Position = direction * markerDistance;

		rateLimit -= (float)delta;

		if (Input.IsActionJustPressed("shoot") && rateLimit <= 0.0f)
		{
			if (bullet == null)
			{
				GD.PrintErr("Bullet PackedScene has not been assigned!");
				return;
			}

			RigidBody2D bulletOb = bullet.Instantiate<RigidBody2D>();

			GetTree().CurrentScene.AddChild(bulletOb);

			bulletOb.GlobalPosition = _bulletSpawnMarker.GlobalPosition;
			bulletOb.Rotation = direction.Angle();
			bulletOb.LinearVelocity = direction * speed;

			if (explosion != null)
			{
				Node2D explosionOb = explosion.Instantiate<Node2D>();

				GetTree().CurrentScene.AddChild(explosionOb);

				explosionOb.GlobalPosition =
					_bulletSpawnMarker.GlobalPosition;

				CpuParticles2D particles = null;

				foreach (Node child in explosionOb.GetChildren())
				{
					if (child is CpuParticles2D cpuParticles)
					{
						particles = cpuParticles;
						break;
					}
				}

				if (particles != null)
				{
					particles.OneShot = true;
					particles.Emitting = true;

					double lifetime =
						particles.Lifetime +
						particles.Preprocess;

					GetTree().CreateTimer(lifetime).Timeout +=
						explosionOb.QueueFree;
				}
				else
				{
					GD.PrintErr(
                        "No CPUParticles2D found in the explosion scene."
					);

					explosionOb.QueueFree();
				}
			}

			GetTree().CreateTimer(bulletLifetime).Timeout +=
				bulletOb.QueueFree;

			rateLimit = fireRate;
		}
	}
}
