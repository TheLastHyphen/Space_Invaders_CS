using System;
using System.Diagnostics;
using Godot;

public partial class Player : Area2D
{
	[Export] public bool IsInvinsible { get; set; } = false;
	[Export] public float InvincibleTime { get; set; } = 5.0f;
	[Export] public float PlayerSpeed { get; set; } = 350.0f;

	[Signal] public delegate void OnPlayerKilledEventHandler();
	[Signal] public delegate void OnPlayerHitByInvaderEventHandler();

	public Action OnMissileFire;

	public bool CanMove { get; set; }
	public bool CanFire { get; set; }
	
	//private float playerSpeed = 350.0f;
	private float _playerBaseWidth = 0.0f;
	private Sprite2D _player;
	private AnimatedSprite2D _playerExplode;

	private AudioStreamPlayer2D _playerExplodeSound;
		//ResourceLoader.Load<AudioStreamOggVorbis>("res://Assets/Audio/explosionCrunch_004.ogg");

	//private AudioStreamPlayer2D _audioPlayer = new();

	private Timer _invincibleTimer;
	
	// for Demo
	private bool _moveLeft = false;
	private bool _moveRight = false;
	private bool _flip = false;
	private bool _demoFire = false;
	private int _numberOfHits = 0;
	private float _animTime = 0.0625f;

	public override void _ExitTree()
	{
		base._ExitTree();
		//_playerExplode.AnimationFinished -= OnAnimationFinished;
	}

	public override void _Ready()
	{
		base._Ready();
		_player = GetNode<Sprite2D>("PlayerSprite");
		AreaEntered += OnAreaEntered;
		
		_playerExplode = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_playerExplode.AnimationFinished += OnAnimationFinished;
		_playerExplode.Hide();
		_playerExplodeSound = GetNode<AudioStreamPlayer2D>("ExplodeSound");
		_playerBaseWidth = GetNode<Sprite2D>("PlayerSprite").Texture.GetSize().X;
		
		_invincibleTimer = GetNode<Timer>("InvincibleTimer");
		_invincibleTimer.WaitTime = InvincibleTime;
		_invincibleTimer.Timeout += OnInvincibleTimeout;

		_player.Modulate = IsInvinsible ? Colors.BlueViolet : _player.Modulate = Colors.Green;
		//AddChild(_audioPlayer);
	}

	private void OnInvincibleTimeout()
	{
		IsInvinsible = false;
		_invincibleTimer.Stop();
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);
		if(!CanFire) return;

		if(inputEvent.IsActionPressed("PlayerFire"))
		{
			OnMissileFire?.Invoke();
		}

		if(inputEvent.IsActionPressed("KillPlayer"))
		{
			PlayerHit();
		}		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(!CanMove) return;

		Vector2 pos = Position;
		float dir = Input.GetAxis("PlayerLeft", "PlayerRight");
		pos.X += dir * (float)delta * PlayerSpeed;		
		Position = pos;
		Position = Position.Clamp(Game.LeftBoundaryX, Game.RightBoundaryX);		
	}

	private void PlayerHit(Area2D area = null)
	{
		if (IsInvinsible) return;
		
		if(area?.Name == "LeftBoundary" || area?.Name == "RightBoundary") return;
		if (area?.GetType().BaseType == typeof(InvaderBase))
			EmitSignal(SignalName.OnPlayerHitByInvader);
		
		++_numberOfHits;
		CanMove = false;
		CanFire = false;
		_player.Hide();
		_playerExplode.Show();
		_playerExplode.Play();
		_playerExplodeSound.Play();
		_invincibleTimer.Start();
		IsInvinsible = true;
		PlayTween();
	}
	
	public void OnAreaEntered(Area2D area)
	{
		PlayerHit(area);
	}

	private void PlayTween()
	{
		Tween tween = CreateTween();
		int loops = (int)(_invincibleTimer.WaitTime / (_animTime * 2) );
		tween.SetLoops(loops);
		tween.TweenProperty(_player, "modulate", Colors.Transparent, _animTime);
		tween.TweenProperty(_player, "modulate", _player.Modulate, _animTime);
	}

	private async void OnAnimationFinished()
	{
		EmitSignal(SignalName.OnPlayerKilled);
		_playerExplode.Hide();
		CanMove = true;
		CanFire = true;
		_player.Show();
	}
}