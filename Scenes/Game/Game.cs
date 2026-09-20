using Godot;

public partial class Game : Node2D
{
	private Timer moveTimer = null;
	private Label _numberOfInvadersLabel = null;
	private Area2D _rightBoundary;
	private Area2D _leftBoundary;
	private Area2D invBomb = null;

	public static float LeftBoundaryX { get; private set; }
	public static float RightBoundaryX { get; private set; }

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);

		if(inputEvent.IsActionPressed("Exit") == true)
		{
			TidyUp();
			SceneManager.Instance.LoadMainUIScene();
		}

		// if(inputEvent.IsActionPressed("IncScore") == true)
		// {
		// 	ScoreDisplay.Instance.UpdateScore(20);
		// }
	}

	public override void _Ready()
	{
		_numberOfInvadersLabel = GetNode<Label>("HBoxContainer/InvadersRemainLabel");
		_rightBoundary = GetNode<Area2D>("RightBoundary");
		RightBoundaryX = _rightBoundary.Position.X;
		_leftBoundary = GetNode<Area2D>("LeftBoundary");
		LeftBoundaryX = _leftBoundary.Position.X;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders += OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnUpdateNumberOfInvaders += OnUpdateNumberOfInvaders;
		Level lev = PackedScenes.Instance.level.Instantiate<Level>();
		lev.Position = new Vector2(50, 50);
		AddChild(lev);
		RenderingServer.SetDefaultClearColor(Colors.Black);
		ScoreDisplay.Instance.ClearScore();
		moveTimer = GetNode<Timer>("MoveTimer");
		moveTimer.Timeout += OnMoveTimerTimeOut;

		// Create player
		Player player = PackedScenes.Instance.Player.Instantiate<Player>();
		player.Position = new Vector2(100, 100);
		AddChild(player);
	}

	public override void _Draw()
	{
		base._Draw();
		DrawLine(new Vector2(0, 600), new Vector2(1152, 600), Colors.Green, 5, false);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if(GodotObject.IsInstanceValid(invBomb) == false)
		{
			invBomb = null;
		}
	}

	public void OnMoveTimerTimeOut()
	{
		SignalBroadCaster.Instance.EmitOnMoveTimerTimeOut();
	}

	public void OnFinishedDrawingInvaders()
	{
		moveTimer?.Start();
		SignalBroadCaster.Instance.EmitOnCanFireMissile();
	}

	private void OnUpdateNumberOfInvaders(int number)
	{
		_numberOfInvadersLabel.Text = number.ToString();
	}

	public void TidyUp()
	{
		moveTimer.Timeout -= OnMoveTimerTimeOut;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders -= OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnUpdateNumberOfInvaders -= OnUpdateNumberOfInvaders;
	}

	public void OnRightBoundary_AreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnEdgeOfScreen();
	}

	public void OnLeftBoundary_AreaEntered(Area2D area)
	{
		SignalBroadCaster.Instance.EmitOnEdgeOfScreen();
	}
}