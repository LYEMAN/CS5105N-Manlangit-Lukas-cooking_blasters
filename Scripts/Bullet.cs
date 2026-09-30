using Godot;
using System;


public partial class Bullet : Node2D
{  
	private Vector2 direction = Vector2.Zero; 
	private Vector2 mouspos = Vector2.Zero;  
	private float speed = 200.0f;  
	private Vector2 velocity  = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
