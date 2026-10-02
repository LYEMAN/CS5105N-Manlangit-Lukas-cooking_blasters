using Godot;
using System;

public partial class Movement : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	public const float Friction = 6000.0f;
	[Export] private AnimationPlayer animationPlayer;
	[Export] private CpuParticles2D leftFootParticles;
	[Export] private CpuParticles2D rightFootParticles;
	private Vector2 velocity;
	private Vector2 direction = Vector2.Zero;
	private float speed = Speed;

	public override void _PhysicsProcess(double delta)
	{
		//GD.Print("Movement Process is running");
		velocity = Velocity;
		direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * speed;
		}
		else
		{
			velocity = velocity.MoveToward(Vector2.Zero, Friction);
		}
		Velocity = velocity;
		MoveAndSlide();

		if (Velocity.Length() > 0)
		{
			if (animationPlayer.CurrentAnimation != "Walk")
			{
				animationPlayer.Play("Walk");
				if (direction.X < 0)
				{
					leftFootParticles.Emitting = true;
					rightFootParticles.Emitting = false;
				}
				else if (direction.X > 0)
				{
					rightFootParticles.Emitting = true;
					leftFootParticles.Emitting = false;
				}
				else
				{
					leftFootParticles.Emitting = true;
					rightFootParticles.Emitting = true;
				}

			}
		}
		else
		{
			if (animationPlayer.CurrentAnimation != "Idle")
			{
				animationPlayer.Play("Idle");
				leftFootParticles.Emitting = false;
				rightFootParticles.Emitting = false;
			}
		}
	}
}
