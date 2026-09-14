using System;
using Godot;

public abstract partial class InvaderBase : Node2D
{
	private int _direction = 1;
	private bool _droppingDown = false;
	private bool _canMove = true;
	private bool _frame = true;

	public abstract int InvaderPoints { get; }
	public abstract String InvaderName { get; }
	public abstract AnimatedSprite2D AnimatedSprite { get; }

	public override void _Ready()
	{
		SignalBroadCaster.Instance.OnMoveTimerTimeOut += OnMoveUpdate;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnMoveTimerTimeOut -= OnMoveUpdate;
	}

	public override void _Process(double delta)
	{
		
	}

	private void Move()
	{
		if(_canMove)
		{
			Position = new Vector2(Position.X + 15 * _direction, Position.Y);
			if(_frame)
			{
				AnimatedSprite.Frame = 1;
			}
			else
			{
				AnimatedSprite.Frame = 0;
			}
			_frame = !_frame;
		}
	}

	private void OnMoveUpdate()
	{
		if(_droppingDown == false)
		{
			Move();
		}
		else
		{
			Position = new Vector2(Position.X, Position.Y + 20);
			_direction *= -1;
			_droppingDown = false;
		}
	}
}
