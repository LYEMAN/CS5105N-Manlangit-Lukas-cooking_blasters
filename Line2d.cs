using Godot;
using System;

public partial class Line2d : Line2D
{ 
	Vector2 mousepos = Vector2.Zero;  
	private RayCast2D ray_cast; 
	private Line2D line_2D;
	public override void _Ready()
	{ 
		line_2D = GetNode<Line2D>("Line2D"); 
		ray_cast = GetNode<RayCast2D>("RayCast2D");  
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{  
		
		mousepos = GetGlobalMousePosition(); 
		ray_cast.TargetPosition = ray_cast.ToLocal(mousepos);  
		ray_cast.ForceRaycastUpdate(); 		
		//line_2D.ClearPoints(); 
		line_2D.AddPoint(Vector2.Zero); 
		
		line_2D.AddPoint(ray_cast.TargetPosition);
		
		
	}
}
