using Godot;

public partial class Line2d : Line2D
{
	private Vector2 mousepos = Vector2.Zero;
	private Vector2 direction = Vector2.Zero;
	private RayCast2D ray_cast;

	[Export]
	private float length = 500.0f;

	public override void _Ready()
	{
		ray_cast = GetNode<RayCast2D>("RayCast2D");  
		Width = 10;
		DefaultColor = Colors.Red;

		GD.Print($"Width = {Width}");
		GD.Print($"Color = {DefaultColor}");
		
	}

	public override void _Process(double delta)
	{
		mousepos = GetGlobalMousePosition();

		direction = (mousepos - GlobalPosition).Normalized();

		ray_cast.TargetPosition = direction * length;
		ray_cast.ForceRaycastUpdate();

		ClearPoints();
		AddPoint(Vector2.Zero);
		AddPoint(ray_cast.TargetPosition);
	}
}
