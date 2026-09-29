using Godot;
using System;

public partial class Shooting : Node2D
{
	private PackedScene projectile = (PackedScene)ResourceLoader.Load("res://Bullet.tscn"); 
	private Vector2 mouspos = Vector2.Zero; 
	private Vector2 direction = Vector2.Zero;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{ 
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("shoot")){
			

		
		}
	}
}
