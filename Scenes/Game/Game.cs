using Godot;

[GlobalClass]
public partial class Game : Node2D
{
	private Timer moveTimer = null;
	private Label _numberOfInvadersLabel = null;
	private Area2D _rightBoundary;
	private Area2D _leftBoundary;
	private int _numberOfInvaders = 0;
	private int _moveSoundIndex = 0;
	private Audio audio = new();
	private float _moveTimerWaitTime = 1.0f;
	private float _movementTimer = 1.0f;
	private float _movementTimerStep = 0;
	private Level _level;
	private AudioStreamPlayer audioPlayer = new();

	public static float LeftBoundaryX { get; private set; }
	public static float RightBoundaryX { get; private set; }

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);

		if(inputEvent.IsActionPressed("Exit") == true)
		{
			SceneManager.Instance.LoadMainUIScene();
		}
	}

	public override void _Ready()
	{
		GetTree().Paused = false;
		_numberOfInvadersLabel = GetNode<Label>("HBoxContainer/InvadersRemainLabel");
		_rightBoundary = GetNode<Area2D>("RightBoundary");
		RightBoundaryX = _rightBoundary.Position.X;
		_leftBoundary = GetNode<Area2D>("LeftBoundary");
		LeftBoundaryX = _leftBoundary.Position.X;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders += OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnUpdateNumberOfInvaders += OnUpdateNumberOfInvaders;
		SignalBroadCaster.Instance.OnInvaderHit += OnInvaderHit;
		SignalBroadCaster.Instance.OnPlayerZeroLives += OnPlayerZeroLives;
	
		RenderingServer.SetDefaultClearColor(Colors.Black);
		ScoreDisplay.Instance.ClearScore();
		moveTimer = GetNode<Timer>("MoveTimer");
		moveTimer.WaitTime = _moveTimerWaitTime;
		moveTimer.Timeout += OnMoveTimerTimeOut;

		// Create player
		Player player = PackedScenes.Instance.Player.Instantiate<Player>();
		player.Position = new Vector2(100, 100);
		AddChild(player);

		AddChild(audioPlayer);
		LoadLevelScene();
	}

	private void LoadLevelScene()
	{
		moveTimer.Stop();
		moveTimer.WaitTime = _moveTimerWaitTime;
		_movementTimer = _moveTimerWaitTime;
		_level = PackedScenes.Instance.level.Instantiate<Level>();
		_level.Position = new Vector2(50, 50);
		AddChild(_level);
		moveTimer.Start();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		moveTimer.Timeout -= OnMoveTimerTimeOut;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders -= OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnUpdateNumberOfInvaders -= OnUpdateNumberOfInvaders;
		SignalBroadCaster.Instance.OnInvaderHit -= OnInvaderHit;
		SignalBroadCaster.Instance.OnPlayerZeroLives -= OnPlayerZeroLives;
	}

	public override void _Draw()
	{
		base._Draw();
		DrawLine(new Vector2(0, 600), new Vector2(1152, 600), Colors.Green, 5, false);
	}

	public void OnMoveTimerTimeOut()
	{
		SignalBroadCaster.Instance.EmitOnMoveTimerTimeOut();
		PlayMoveSound();
	}

	private void PlayMoveSound()
	{
		if(_moveSoundIndex > 3) _moveSoundIndex = 0;
		audioPlayer.VolumeLinear = 1.0f;
		audioPlayer.Stream = audio.MoveSounds[_moveSoundIndex++];
		audioPlayer.Play();
	}

	public void OnFinishedDrawingInvaders()
	{
		moveTimer?.Start();
		SignalBroadCaster.Instance.EmitOnCanFireMissile();
	}

	private void CheckForLevelComplete()
	{
		if(_numberOfInvaders == 0)
		{
			_level.QueueFree();
			CallDeferred("LoadLevelScene"); // wait until flushing complete before loading next level
		}
	}

	private async void OnPlayerZeroLives()
	{
		GetTree().Paused = true;
		GameOver gameOver = PackedScenes.Instance.GameOver.Instantiate<GameOver>();
		GetParent().AddChild(gameOver);
		await ToSignal(GetTree().CreateTimer(5.0), "timeout");
		gameOver.QueueFree();
		SceneManager.Instance.LoadMainUIScene();
	}

	private void CheckForGameOver()
	{
		if(_numberOfInvaders == 0)
		{
			_level.QueueFree();
			LoadLevelScene();
		}
	}

	private void OnUpdateNumberOfInvaders(int number)
	{
		_numberOfInvaders = number;
		_numberOfInvadersLabel.Text = number.ToString();
		_movementTimerStep = _movementTimer / _numberOfInvaders;
	}

	public void OnRightBoundary_AreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnEdgeOfScreen();
	}

	public void OnLeftBoundary_AreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnEdgeOfScreen();
	}

	private void OnInvaderHit()
	{
		OnUpdateNumberOfInvaders(_numberOfInvaders - 1);
		AdjustInvaderMovementSpeed();
		CheckForLevelComplete();
	}

	private void AdjustInvaderMovementSpeed()
	{
		_movementTimer -= _movementTimerStep;
		if(_movementTimer <= 0) _movementTimer = _movementTimerStep;
		moveTimer.WaitTime = _movementTimer;
	}
}