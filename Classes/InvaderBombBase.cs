using System.Collections.Generic;
using Godot;

public abstract partial class InvaderBombBase : Area2D
{
	private int _speed = 0;
	public AnimatedSprite2D InvAnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	
	public abstract int Points { get; }

	public override void _Ready()
	{
		_speed = (int)GD.RandRange(75.0f, 400.0f);
	}

	public override void _Process(double delta)
	{
		Vector2 pos = Position;
		pos.Y += 1 * (float)delta * _speed;
		Position = pos;
	}

	public void OnAreaEntered(Area2D area)
	{
		if(area.Name == "FloorBoundary")
		{
			QueueFree();
		}
		else if(area.Name == "Player")
		{
			QueueFree();
		}
		else if(area.Name == "PlayerMissile")
		{
			QueueFree();
		}
	}
}
