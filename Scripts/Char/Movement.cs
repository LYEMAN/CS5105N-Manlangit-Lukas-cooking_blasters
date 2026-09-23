using Godot;
using System;

public partial class Movement : CharacterBody2D{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;  
	public const float Friction = 6000.0f; 
	private Vector2 velocity;  
	private Vector2 direction = Vector2.Zero;
	private float speed = Speed;  

	public override void _PhysicsProcess(double delta){
		velocity = Velocity;  
		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down"); 
		if(direction != Vector2.Zero)
		{
		velocity = direction * speed;  
		}  
		else
		{
		 velocity = velocity.MoveToward(Vector2.Zero, Friction); 
		}
		Velocity = velocity; 
		MoveAndSlide();
	}
}
