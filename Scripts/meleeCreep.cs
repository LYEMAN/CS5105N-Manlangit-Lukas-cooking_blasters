using Godot;

public partial class meleeCreep : CharacterBody2D
{
	[Export] private float speed = 200.0f;
	[Export] private float damage = 10.0f;

	private Node2D player;
	private Area2D damageArea;

	public override void _Ready()
	{  
		AddToGroup("Enemy");
		player = GetTree().GetFirstNodeInGroup("Player") as Node2D;

		damageArea = GetNode<Area2D>("DamageArea");
		damageArea.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print("DamageArea detected: ", body.GetPath());

		Health health = body.GetNodeOrNull<Health>("Node2D");

		if (health == null)
		{
			GD.PrintErr("Could not find Health on ", body.GetPath());
			return;
		}

		GD.Print("Health component found!");

		health.TakeDamage(damage);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player == null || !IsInstanceValid(player))
			return;

		Vector2 direction = GlobalPosition.DirectionTo(player.GlobalPosition);

		Velocity = direction * speed;

		MoveAndSlide();
	}
}
