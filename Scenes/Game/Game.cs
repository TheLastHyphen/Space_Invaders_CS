using Godot;

public partial class Game : Node
{

	private PackedScene level = ResourceLoader.Load<PackedScene>("res://Scenes/Level/Level.tscn");
	private Timer moveTimer = null;

	private Label _numberOfInvadersLabel = null;

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);

		if(inputEvent.IsActionPressed("Exit") == true)
		{
			TidyUp();
			SceneManager.Instance.LoadMainUIScene();
		}

		if(inputEvent.IsActionPressed("IncScore") == true)
		{
			ScoreDisplay.Instance.UpdateScore(20);
		}
	}

	public override void _Ready()
	{
		_numberOfInvadersLabel = GetNode<Label>("HBoxContainer/InvadersRemainLabel");
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders += OnFinishedDrawingInvaders;
		SignalBroadCaster.Instance.OnUpdateNumberOfInvaders += OnUpdateNumberOfInvaders;
		Level lev = level.Instantiate<Level>();
		lev.Position = new Vector2(100, 100);
		AddChild(lev);
		RenderingServer.SetDefaultClearColor(Colors.Black);
		ScoreDisplay.Instance.ClearScore();
		moveTimer = GetNode<Timer>("MoveTimer");
		moveTimer.Timeout += OnMoveTimer_TimeOut;	
	}

	public void OnMoveTimer_TimeOut()
	{
		SignalBroadCaster.Instance.EmitOnMoveTimerTimeOut();
	}

	public void OnFinishedDrawingInvaders()
	{
		moveTimer.Start();
	}

	private void OnUpdateNumberOfInvaders(int number)
	{
		_numberOfInvadersLabel.Text = number.ToString();
	}

	public void TidyUp()
	{
		moveTimer.Timeout -= OnMoveTimer_TimeOut;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders -= OnFinishedDrawingInvaders;
	}
}