using Godot;

[GlobalClass]
public partial class Game : Node2D
{
	private Timer _moveTimer;
	private Timer _mothershipTimer;
	
	private Label _numberOfInvadersLabel;
	private Area2D _rightBoundary;
	private Area2D _rightKillBoundary;
	private Area2D _leftKillBoundary;
	private Area2D _leftBoundary;
	private int _numberOfInvaders;
	private int _numberOfPlayerShots = 0;
	
	private Marker2D _motherShipLeftStart;
	private Marker2D _motherShipRightStart;
	private Vector2 _motherShipStartPosition;
	private int _motherShipDir = 0;
	private MotherShip _motherShip;
	
	private float _gameEngineSpeed = 1.0f;

	private int _moveSoundIndex;
	private const float MoveTimerWaitTime = 1.0f;
	private const float MinimumStepTime = 0.04f;
	[Export]
	public float MovementTimer
	{
		get => _movementTimer;
		set => _movementTimer = value;
	}

	[Export] public Vector2 FloorLineStart = new(0, 600);
	[Export] public Vector2 FloorLineEnd =new(1152, 600);
	
	private float _movementTimer = 1.0f;

	private float _movementTimerStep;
	private bool _allowMove;
	private Level _level;
	private AudioStreamPlayer _audioPlayer = new();
	private AudioStreamPlayer2D _playerMissileSound;
	
	private AnimatedSprite2D _invaderExplode;
	private Player _player;
	private PlayerMissile _playerMissile;
	private AnimatedSprite2D _playerMissileExplode;
	private LivesRemaining _livesRemaining;

	public static float LeftBoundaryX { get; private set; }
	public static float RightBoundaryX { get; private set; }

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);

		if(inputEvent.IsActionPressed("Exit"))
		{
			SceneManager.Instance.LoadMainUIScene();
		}

		if(inputEvent.IsActionPressed("MuteSounds"))
		{
			States.Instance.MuteSound();
		}

		if(inputEvent.IsActionPressed("AddScore_Debug"))
		{
			ScoreBoard.Instance.UpdateScore(1500);
		}

		if (inputEvent.IsActionPressed("EngineTimeScaleSlower"))
		{
			_gameEngineSpeed -= 0.25f;
			if (_gameEngineSpeed <= 0.0625f)
				_gameEngineSpeed = 0.0625f;
			
			Engine.TimeScale = _gameEngineSpeed;
			GD.Print($"Engine Time Scale={Engine.TimeScale}");
		}
		
		if (inputEvent.IsActionPressed("EngineTimeScaleFaster"))
		{
			_gameEngineSpeed += 1.0f;
			if (_gameEngineSpeed >= 16.0f)
				_gameEngineSpeed = 16.0f;
			
			Engine.TimeScale = _gameEngineSpeed;
			GD.Print($"Engine Time Scale={Engine.TimeScale}");
		}

		if (inputEvent.IsActionPressed("ResetEngineTimeScale"))
		{
			_gameEngineSpeed = 1.0f;
			Engine.TimeScale = 1.0f;
		}
	}

	public override void _Ready()
	{
		States.Instance.Debug = true;
		GetTree().Paused = false;
		_numberOfInvadersLabel = GetNode<Label>("HBoxContainer/InvadersRemainLabel");
		
		_rightBoundary = GetNode<Area2D>("RightBoundary");
		RightBoundaryX = _rightBoundary.Position.X;
		_rightKillBoundary = GetNode<Area2D>("RightKillBoundary");
		
		_leftBoundary = GetNode<Area2D>("LeftBoundary");
		_leftKillBoundary = GetNode<Area2D>("LeftKillBoundary");
		LeftBoundaryX = _leftBoundary.Position.X;
		
		_mothershipTimer = GetNode<Timer>("MotherShipTimer");
		_mothershipTimer.Timeout += OnMothershipTimer;

		_motherShipLeftStart = GetNode<Marker2D>("MotherShipLeftStart");
		_motherShipRightStart = GetNode<Marker2D>("MotherShipRightStart");
		
		_player = GetNode<Player>("Player");
		_player.CanMove = false;
		_player.CanFire = false;
		_player.OnMissileFire += OnMissileFire;
		_player.OnPlayerKilled += OnPlayerKilled;
		_player.OnPlayerHitByInvader += OnPlayerHitByInvader; 
		_playerMissileExplode = GetNode<AnimatedSprite2D>("Effects/PlayerMissileExplode");
		
		_livesRemaining = GetNode<LivesRemaining>("LivesRemaining");
		_livesRemaining.OnZeroLives += OnZeroLives;

		SignalBroadCaster.Instance.OnFinishedDrawingInvaders += OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnInvaderHit += OnInvaderHit;
		SignalBroadCaster.Instance.OnInvaderKilled += OnInvaderKilled;
		SignalBroadCaster.Instance.OnInvaderHitFloor += OnInvaderHitFloor; 
	
		RenderingServer.SetDefaultClearColor(Colors.Black);
		ScoreBoard.Instance.ClearScore();
		
		_moveTimer = GetNode<Timer>("MoveTimer");
		_moveTimer.WaitTime = MoveTimerWaitTime;
		_moveTimer.Timeout += OnMoveTimerTimeOut;

		AddChild(_audioPlayer);
		LoadLevelScene();
		ScoreBoard.Instance.InitialiseScores();
	}

	private Vector2 GetMotherShipStartPosition()
	{
		Vector2 pos = new();
		int r = GD.RandRange(0, 1) == 0 ? -1 : 1;

		switch (r)
		{
			case 1: pos = _motherShipLeftStart.Position; break;	// LHS
			case -1: pos = _motherShipRightStart.Position; break;	// RHS
		}
		
		_motherShipDir = r;
		return pos;
	}
	
	private void OnMothershipTimer()
	{
		_mothershipTimer.Stop();
		_motherShip = PackedScenes.Instance.MotherShip.Instantiate<MotherShip>();
		_motherShip.Position = GetMotherShipStartPosition();
		_motherShip.Direction = _motherShipDir;
		_motherShip.OnMotherShipHit += OnMotherShipHit;
		_motherShip.Direction = _motherShipDir;
		AddChild(_motherShip);
	}

	private void OnMotherShipHit()
	{
		_mothershipTimer.Start();
	}

	private void OnInvaderHitFloor()
	{
		OnZeroLives();
	}

	private void OnPlayerHitByInvader()
	{
		OnZeroLives();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		_moveTimer.Timeout -= OnMoveTimerTimeOut;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders -= OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnInvaderHit -= OnInvaderHit;
		SignalBroadCaster.Instance.OnInvaderKilled -= OnInvaderKilled;
		QueueFree();
	}

	public override void _Draw()
	{
		base._Draw();
		DrawLine(FloorLineStart, FloorLineEnd, Colors.Green, 5, false);
	}

	#region *** Player Handlers ***
	private void OnZeroLives()
	{
		OnPlayerZeroLives();
	}

	private void OnPlayerKilled()
	{
		_livesRemaining.UpdateLives();
	}

	private void OnMissileHit(Area2D area)
	{
		_playerMissile.Modulate = 
			area.Name == "CeilingBoundary" ?
				_playerMissileExplode.Modulate = Colors.Red :
				_playerMissileExplode.Modulate = Colors.White;
		
		_playerMissileExplode.Position = _playerMissile.Position;
		_playerMissile.OnMissileHit -= OnMissileHit;
		_playerMissile.QueueFree();
		_playerMissileExplode.Show();
		_playerMissileExplode.Play("Explode");
	}

	private void OnMissileFire()
	{
		if(GodotObject.IsInstanceValid(_playerMissile)) return;
		_playerMissile = PackedScenes.Instance.PlayerMissile.Instantiate<PlayerMissile>();
		AddChild((_playerMissile));
		_playerMissile.OnMissileHit += OnMissileHit;
		_playerMissile.Position = _player.Position;
		SoundManager.Instance.PlayStream(SoundEffect.PlayerMissile);
		_numberOfPlayerShots++;
		GD.Print($"Shot = {_numberOfPlayerShots}");
	}

	private async void OnPlayerZeroLives()
	{
		_player.Hide();
		GetTree().Paused = true;
		GameOver gameOver = PackedScenes.Instance.GameOver.Instantiate<GameOver>();
		GetParent().AddChild(gameOver);
		await ToSignal(GetTree().CreateTimer(5.0), "timeout");
		gameOver.QueueFree();
		SceneManager.Instance.LoadMainUIScene();
	}
	#endregion  *** Player Handlers ***

	#region *** Invader Handlers ***
	private void OnIncrementInvader(int number)
	{
		_numberOfInvaders = number;
		UpdateInvadersRemainLabel();
	}

	private void OnInvaderKilled()
	{
		_numberOfInvaders--;
		AdjustInvaderMovementSpeed();
		UpdateInvadersRemainLabel();
		CheckForLevelComplete();
	}

	private void OnMoveTimerTimeOut()
	{
		if (!_allowMove) return;

		SignalBroadCaster.Instance.EmitOnMoveTimerTimeOut();
		if (States.Instance.IsSoundsMute) return;
		PlayMoveSound();
	}

	private void PlayMoveSound()
	{
		SoundManager.Instance.PlayStream(SoundEffect.InvaderMove);
	}

	private void OnInvaderHit(Area2D area)
	{
		if (area.Name != "PlayerMissile") return;
		
		AdjustInvaderMovementSpeed();
		CheckForLevelComplete();
	}

	private void AdjustInvaderMovementSpeed()
	{
		_movementTimer -= _movementTimerStep;
		if(_movementTimer <= 0) _movementTimer = _movementTimerStep;
		_moveTimer.WaitTime = _movementTimer;
	}

	private void UpdateInvadersRemainLabel()
	{
		_numberOfInvadersLabel.Text = $"{_numberOfInvaders}";
	}

	private void CalculateInvaderMovementStep()
	{
		// t = (Time - minT) / N-1
		_movementTimerStep = (MoveTimerWaitTime - MinimumStepTime) / (_numberOfInvaders - 1);
	}
	
	#endregion  *** Invader Handlers ***

	#region *** Level Handlers ***
	private void LoadLevelScene()
	{
		_moveTimer.Stop();
		_moveTimer.WaitTime = MoveTimerWaitTime;
		_movementTimer = MoveTimerWaitTime;
		_level = PackedScenes.Instance.level.Instantiate<Level>();
		AddChild(_level);
		_level.OnIncrementInvader += OnIncrementInvader;
		_level.Position = new Vector2(50, 50);
		_moveTimer.Start();
		_mothershipTimer.Start();
	}

	private void OnFinishedDrawingInvaders()
	{
		CalculateInvaderMovementStep();
		_moveTimer?.Start();
		SignalBroadCaster.Instance.EmitOnCanFireMissile();
		_allowMove = true;
		_player.CanMove = true;
		_player.CanFire = true;
	}

	private void CheckForLevelComplete()
	{
		if(_numberOfInvaders != 0) return;
		
		if (GodotObject.IsInstanceValid(_motherShip))
		{
			_mothershipTimer.Stop();
			_motherShip.QueueFree();
		}
		SoundManager.Instance.StopStream(SoundEffect.InvaderMove);
		_level.QueueFree();
		CallDeferred("LoadLevelScene"); // wait until flushing complete before loading next level
	}
	#endregion *** Level Handlers ***

	#region *** Collision Handlers ***

	private void OnRightBoundary_AreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnEdgeOfScreen();
	}

	private void OnLeftBoundary_AreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnEdgeOfScreen();
	}
	#endregion *** Collision Handlers ***
}