using Godot;

public partial class Bullet : RigidBody2D
{
	[Export] private float damage = 25.0f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		GD.Print("Bullet hit: ", body.GetPath());

		Health health = FindHealth(body);

		if (health != null)
		{
			GD.Print("Damaging enemy for ", damage);
			health.TakeDamage(damage);
		}
		else
		{
			GD.PrintErr("No Health found on ", body.GetPath());
		}

		QueueFree();
	}

	private Health FindHealth(Node node)
	{
		if (node is Health health)
			return health;

		foreach (Node child in node.GetChildren())
		{
			Health result = FindHealth(child);

			if (result != null)
				return result;
		}

		return null;
	}
}
