using Godot;

public partial class Cam : Camera2D
{
	[Export]
	public Node2D Target { get; set; }

	public override void _PhysicsProcess(double delta)
	{
		if (Target != null)
		{
			GlobalPosition = Target.GlobalPosition; 
		}
	}
}
