
using Godot;

public partial class meleeCreep : CharacterBody2D
{
	[Export] private float speed = 200.0f;
	[Export] private float damage = 10.0f;
	[Export] private float repathInterval = 0.2f;

	private Node2D player;
	private Area2D damageArea;
	private NavigationAgent2D navigationAgent;
	private float repathTimer = 0.0f;

	public override void _Ready()
	{
		AddToGroup("Enemy");

		player = GetTree().GetFirstNodeInGroup("Player") as Node2D;

		damageArea = GetNode<Area2D>("DamageArea");
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

		navigationAgent.PathDesiredDistance = 4.0f;
		navigationAgent.TargetDesiredDistance = 16.0f;

		damageArea.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (!body.IsInGroup("Player"))
			return;

		Health health = body.GetNodeOrNull<Health>("Node2D");

		if (health == null)
		{
			GD.PrintErr("Could not find Health on ", body.GetPath());
			return;
		}

		health.TakeDamage(damage);
	}

	public override void _PhysicsProcess(double delta) // The AI edited only this function to path instead of moving directly to the player
	{
		if (player == null || !GodotObject.IsInstanceValid(player)) // if the player does not exist, pass a velocity of zero to moveandslide and return
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		repathTimer -= (float)delta; //cooldown timer for repathing, based on time between frames (delta)

		if (repathTimer <= 0.0f) // if repath cooldown is up, set the navigationagent of the enemy to the players current position 
		{
			navigationAgent.TargetPosition = player.GlobalPosition; 
			repathTimer = repathInterval;
		}

		if (GlobalPosition.DistanceTo(player.GlobalPosition) // if the enemy is touching the player, pass a velocity of zero to moveandslide and return
			<= navigationAgent.TargetDesiredDistance)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		if (navigationAgent.IsNavigationFinished()) // if the enemy has no path to the player, pass a velocity of zero to moveandslide and return
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
}
