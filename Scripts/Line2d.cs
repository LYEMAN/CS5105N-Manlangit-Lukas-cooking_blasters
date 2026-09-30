using Godot;

public partial class Line2d : Line2D
{
	private Vector2 mousepos = Vector2.Zero;
	private Vector2 direction = Vector2.Zero;
	private RayCast2D ray_cast; 
	private float markerDistance = 200.0f;

	[Export]
	private float length = 500.0f;
	private Marker2D marker;
	public override void _Ready()
	{
		ray_cast = GetNode<RayCast2D>("RayCast2D");  
		
		 
   		
		GD.Print("RayCast: ", ray_cast);
	GD.Print("Marker: ", marker);
		
	}

	public override void _Process(double delta)
	{
		mousepos = GetGlobalMousePosition();

		direction = (mousepos - GlobalPosition).Normalized();
		
		ray_cast.TargetPosition = direction * length;
		ray_cast.ForceRaycastUpdate();
		

		marker.Position = ray_cast.TargetPosition.Normalized() * markerDistance;		ClearPoints();
		AddPoint(Vector2.Zero);
		AddPoint(ray_cast.TargetPosition);
	}
}
