using System.Threading.Tasks;
using Godot;

public partial class MainUi : Control
{
	private Label _playLabel;
	private Label _spaceLabel;
	private Label _invaderLabel;
	private Label _startGameLabel;
	private VBoxContainer _scoreTableVbox;
	private Label _scoreAdvLabel;
	private Label _mysteryPointsLabel;
	private Label _squidPointsLabel;
	private Label _crabPointsLabel;
	private Label _octopusPointsLabel;

	private const string PlayText = "PLAY";
	private const string SpaceText = "SPACE";
	private const string InvaderText = "INVADERS";
	private const string StartGameText = "PRESS 1 TO START THE GAME";
	private const string MysteryPointsText = "= ? MYSTERY POINTS";
	private const string SquidPointsText = "= 30 POINTS";
	private const string CrabPointsText = "= 20 POINTS";
	private const string OctopusPointsText = "= 10 POINTS";


	private const float charDelay = 0.06f;
	int count = 0;
	
	public override void _UnhandledInput(InputEvent inputEvent)
	{
		base._UnhandledInput(inputEvent);

		if(inputEvent.IsActionPressed("StartGame"))
		{
			SceneManager.Instance.LoadGameScene();
		}
	}

	public async override void _Ready()
	{
		RenderingServer.SetDefaultClearColor(Colors.Black);
		ScoreDisplay.Instance.ClearScore();
		_scoreTableVbox = GetNode<VBoxContainer>("ScoreTableVBox");
		_scoreTableVbox.Hide();
		_scoreAdvLabel = GetNode<Label>("ScoreAdvLabel");
		_scoreAdvLabel.Hide();

		_playLabel = GetNode<Label>("PlayLabel");
		_spaceLabel = GetNode<Label>("SpaceInvHBox/SpaceLabel");
		_invaderLabel = GetNode<Label>("SpaceInvHBox/InvaderLabel");
		_startGameLabel = GetNode<Label>("StartGameLabel");
		_mysteryPointsLabel = GetNode<Label>("ScoreTableVBox/SpaceShip/mysteryTextLabel");
		_squidPointsLabel = GetNode<Label>("ScoreTableVBox/Squid/SquidTextLabel");
		_crabPointsLabel = GetNode<Label>("ScoreTableVBox/Crab/CrabTextLabel");
		_octopusPointsLabel = GetNode<Label>("ScoreTableVBox/Octopus/OctopusTextLabel");
		ClearLabels();
		await DrawText();
		await Delay(1.5f);
		await DrawScoreTable();
	}

	private void ClearLabels()
	{
		_playLabel.Text = "";
		_spaceLabel.Text = "";
		_invaderLabel.Text = "";
		_startGameLabel.Text = "";
		_mysteryPointsLabel.Text = "";
		_squidPointsLabel.Text = "";
		_crabPointsLabel.Text = "";
		_octopusPointsLabel.Text = "";
	}

	private async Task DrawText()
	{
		await WaitDrawChar(_playLabel, charDelay, PlayText);
		await WaitDrawChar(_spaceLabel, charDelay, SpaceText);
		await WaitDrawChar(_invaderLabel, charDelay, InvaderText);		
	}

	private async Task DrawScoreTable()
	{
		_scoreAdvLabel.Show();
		_scoreTableVbox.Show();
		// don't wait, draw at the same time score table displayed
		WaitDrawChar(_startGameLabel, 0.1f, StartGameText);
		await WaitDrawChar(_mysteryPointsLabel, charDelay, MysteryPointsText);
		await WaitDrawChar(_squidPointsLabel, charDelay, SquidPointsText);
		await WaitDrawChar(_crabPointsLabel, charDelay, CrabPointsText);
		await WaitDrawChar(_octopusPointsLabel, charDelay, OctopusPointsText);
	}

	private async Task WaitDrawChar(Label label, float delay, string text)
	{	
		foreach(char c in text)
		{
			label.Text += c;
			await Delay(delay);
		}
	}

	private async Task Delay(float delay)
	{
		await ToSignal(GetTree().CreateTimer(delay), "timeout");
	}
}
