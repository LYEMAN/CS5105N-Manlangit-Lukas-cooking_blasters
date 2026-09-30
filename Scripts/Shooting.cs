using Godot;
using System;

public partial class Shooting : Sprite2D
{
	[Export] PackedScene bullet;
	[Export] float speed = 2000.0f;
	[Export] float bps = 5.0f;

	private float fireRate;
	private float rateLimit = 0.0f; 
	private Line2D _aimLine;
private Marker2D _bulletSpawnMarker;

[Export] float lineLength = 500.0f;
[Export] float markerDistance = 200.0f;

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

	// Draw aiming line
	_aimLine.ClearPoints();
	_aimLine.AddPoint(Vector2.Zero);
	_aimLine.AddPoint(direction * lineLength);

	// Move bullet spawn point along the aiming line
	_bulletSpawnMarker.Position = direction * markerDistance;

	// Shooting
	rateLimit -= (float)delta;

	if (Input.IsActionJustPressed("shoot") && rateLimit <= 0.0f)
	{
		RigidBody2D bulletOb = bullet.Instantiate<RigidBody2D>();

		GetTree().CurrentScene.AddChild(bulletOb);

		bulletOb.GlobalPosition = _bulletSpawnMarker.GlobalPosition;
		bulletOb.Rotation = direction.Angle();
		bulletOb.LinearVelocity = direction * speed;

		rateLimit = fireRate;
	}
}
}
