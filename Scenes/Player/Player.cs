using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class Player : Area2D
{
	private float playerSpeed = 350.0f;
	private bool _canMove = true;
	private bool _canFire = false;
	private float _playerBaseWidth = 0.0f;
	private Sprite2D _player;
	private AnimatedSprite2D _playerExplode;
	private Area2D missile = null;
	private AudioStream _missileFire = ResourceLoader.Load<AudioStream>("res://Assets/Audio/PlayerFire_alt.wav");
	private AudioStreamPlayer audioPlayer = new();

	// for Demo
	private bool _moveLeft = false;
	private bool _moveRight = false;
	private bool _flip = false;
	private bool _demoFire = false;
	private int _numberOfHits = 0;

	public override void _ExitTree()
	{
		base._ExitTree();
		//_playerExplode.AnimationFinished -= OnAnimationFinished;
	}

	public override void _Ready()
	{
		base._Ready();
		_player = GetNode<Sprite2D>("PlayerSprite");
		_playerExplode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_playerExplode.AnimationFinished += OnAnimationFinished;
		_playerExplode.Hide();
		_playerBaseWidth = GetNode<Sprite2D>("PlayerSprite").Texture.GetSize().X;
		SignalBroadCaster.Instance.OnCanFireMissile += OnCanFireMissile;
		AddChild(audioPlayer);
		audioPlayer.Stream = _missileFire;
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);
		if(_canFire == false) return;

		if(inputEvent.IsActionPressed("PlayerFire") && GodotObject.IsInstanceValid(missile) == false)
		{
			missile = PackedScenes.Instance.PlayerMissile.Instantiate<PlayerMissile>();			
			missile.Position = GlobalPosition;
			GetParent().AddChild(missile);
			audioPlayer.Play();			
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(_canMove == false) return;
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
		if(area.Name == "LeftBoundary" || area.Name == "RightBoundary") return;
		++_numberOfHits;
		_canMove = false;
		_canFire = false;
		_player.Hide();
		_playerExplode.Show();
		_playerExplode.Play();
	}

	private async void OnAnimationFinished()
	{
		SignalBroadCaster.Instance.EmitOnPlayerHit();
		_playerExplode.Hide();
		if(_numberOfHits < 3) _player.Show();
		_canMove = true;
		_canFire = true;
	}
}