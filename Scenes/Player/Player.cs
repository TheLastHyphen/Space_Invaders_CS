using Godot;

public partial class Player : Area2D
{
	private Vector2 _playerPosition = new Vector2(238,558);
	private float playerSpeed = 500.0f;
	private bool _canMove = true;
	private bool _canFire = false;
	private float _playerBaseWidth = 0.0f;
	private Area2D missile = null;
	
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
		SignalBroadCaster.Instance.OnCanFireMissile += OnCanFireMissile;
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);
		if(_canFire == false) return;
		if(inputEvent.IsActionPressed("PlayerFire") && GodotObject.IsInstanceValid(missile) == false)
		{
			missile = PackedScenes.Instance.PlayerMissile.Instantiate<PlayerMissile>();
			missile.Position = Position;
			GetParent().AddChild(missile);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Vector2 pos = Position;
		float dir = Input.GetAxis("PlayerLeft", "PlayerRight");
		pos.X += dir * (float)delta * playerSpeed;

		if(pos.X >= Game.RightBoundaryX)
		{
			pos.X = Game.RightBoundaryX;
		}
		else if(pos.X <= Game.LeftBoundaryX)
		{
			pos.X = Game.LeftBoundaryX;
		}
		Position = pos;
	}

	public void OnCanFireMissile()
	{
		_canFire = true;
	}

	public void OnAreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnPlayerHit();
	}
}
