using Godot;

public partial class Game : Node
{

	private Timer moveTimer = null;

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);

		if(inputEvent.IsActionPressed("Exit") == true)
		{
			moveTimer.Timeout -= OnMoveTimer_TimeOut;
			SceneManager.Instance.LoadMainUIScene();
		}

		if(inputEvent.IsActionPressed("IncScore") == true)
		{
			ScoreDisplay.Instance.UpdateScore(20);
		}
	}

	public override void _Ready()
	{
		RenderingServer.SetDefaultClearColor(Colors.Black);
		ScoreDisplay.Instance.ClearScore();
		moveTimer = GetNode<Timer>("MoveTimer");
		moveTimer.Timeout += OnMoveTimer_TimeOut;
		moveTimer.Start();		
	}

	public void OnMoveTimer_TimeOut()
	{
		SignalBroadCaster.Instance.EmitOnMoveTimerTimeOut();
	}
}