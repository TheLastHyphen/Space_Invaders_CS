using System.Security.Principal;
using Godot;

public partial class Player : Area2D
{
	private Vector2 _playerPosition = new Vector2(238,558);
	private float playerSpeed = 500.0f;
	private bool _canMove = true;
	private bool _canMoveRight = true;
	private bool _canMoveLeft = true;
	private bool _canFire = true;
	private float _playerBaseWidth = 0.0f;
	
	// for Demo
	private bool _moveLeft = false;
	private bool _moveRight = false;
	private bool _flip = false;
	private bool _demoFire = false;

	public override void _Ready()
	{
		base._Ready();
		Position = _playerPosition;
		_playerBaseWidth = GetNode<Sprite2D>("PlayerSprite").Texture.GetSize().X;
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		float dir = Input.GetAxis("PlayerLeft", "PlayerRight");
		Vector2 pos = Position;
		pos.X += dir * (float)delta * playerSpeed;

		if(Input.IsActionPressed("PlayerLeft") && _canMoveLeft)
		{
			Position = pos;
		}
		else if(Input.IsActionPressed("PlayerRight") && _canMoveRight)
		{
			Position = pos;
		}
	}

	public void OnAreaEntered(Area2D area)
	{
		if(area.Name == "RightBoundary")
		{
			 _canMoveRight = false;
			_canMoveLeft = true;
		}
		else if(area.Name == "LeftBoundary")
		{
			_canMoveLeft = false;
			_canMoveRight = true;
		}
	}
}
