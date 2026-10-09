using Godot;

public partial class RangedCreep : CharacterBody2D
{
	[Export] private float speed = 200.0f;
	[Export] private float damage = 10.0f;
	[Export] private PackedScene bullet;
	[Export] private float fireRate = 1.0f;
	[Export] private float attackRange = 300.0f;
	[Export] private float repathInterval = 0.2f;
	[Export] private float bulletSpeed = 500.0f;

	private Node2D player;
	private NavigationAgent2D navigationAgent;
	private Marker2D bulletSpawn;
	private float fireTimer = 0.0f;
	private float repathTimer = 0.0f;

	public override void _Ready()
	{
		AddToGroup("Enemy");

		player = GetTree().GetFirstNodeInGroup("Player") as Node2D;
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		bulletSpawn = GetNode<Marker2D>("Marker2D");

		navigationAgent.PathDesiredDistance = 4.0f;
		navigationAgent.TargetDesiredDistance = attackRange;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player == null || !GodotObject.IsInstanceValid(player))
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		float distance = GlobalPosition.DistanceTo(player.GlobalPosition);

		fireTimer -= (float)delta;
		repathTimer -= (float)delta;

		if (repathTimer <= 0.0f)
		{
			navigationAgent.TargetPosition = player.GlobalPosition;
			repathTimer = repathInterval;
		}

		if (distance <= attackRange)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();

			if (fireTimer <= 0.0f)
			{
				Shoot();
				fireTimer = fireRate;
			}

			return;
		}

		if (navigationAgent.IsNavigationFinished())
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		Vector2 nextPosition = navigationAgent.GetNextPathPosition();
		Vector2 direction = GlobalPosition.DirectionTo(nextPosition);

		Velocity = direction * speed;
		MoveAndSlide();
	}

	private void Shoot()
	{
		if (bullet == null)
			return;

		Node2D bulletInstance = bullet.Instantiate<Node2D>();
		GetTree().CurrentScene.AddChild(bulletInstance);

		bulletInstance.GlobalPosition = bulletSpawn.GlobalPosition;

		Vector2 direction = bulletSpawn.GlobalPosition.DirectionTo(player.GlobalPosition);
		bulletInstance.Rotation = direction.Angle();

		if (bulletInstance is RigidBody2D rigidBullet)
		{
			rigidBullet.LinearVelocity = direction * bulletSpeed;
		}
		else if (bulletInstance is CharacterBody2D characterBullet)
		{
			characterBullet.Velocity = direction * bulletSpeed;
		}
	}
}
