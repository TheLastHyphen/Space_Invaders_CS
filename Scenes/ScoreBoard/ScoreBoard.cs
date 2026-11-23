using Godot;

public partial class ScoreBoard : CanvasLayer
{
	public static ScoreBoard Instance { get; private set; }

	private Label _scoreLabel;
	private Label _highScoreLabel;
	private uint _score;
	private uint _highScore;
	private uint _extraLifePoints = 1500;
	private uint _extraLifeScoreMultiplier= 2;

	private uint _nextLifePointsAt;
	
	public void InitialiseScores()
	{
		_score = 0;
		_nextLifePointsAt = _extraLifePoints;
	}

	public void UpdateScore(uint points)
	{
		_score += points;
		UpdateScoreLabel();
		UpdateHighScore();
		CheckForExtraLife();
	}

	private void CheckForExtraLife()
	{
		if (_score <= 0) return;
		if (_score < _extraLifePoints) return;

		_nextLifePointsAt += _extraLifePoints;
		SignalBroadCaster.Instance.EmitOnExtraLife();
		_extraLifePoints *= _extraLifeScoreMultiplier;
	}

	public void ClearScore()
	{
		_score = 0;
		UpdateScoreLabel();
	}

	private void UpdateHighScore()
	{
		if(_score < _highScore) return;
		
		_highScore = _score;
		UpdateHighScoreLabel();
	}

	private void UpdateScoreLabel()
	{
		_scoreLabel.Text = _score.ToString("D6");	// pad with zeros
	}

	private void UpdateHighScoreLabel()
	{
		_highScoreLabel.Text = _highScore.ToString("D6");	// pad with zeros
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnPointScored -= OnPointScored;
	}

	public override void _Ready()
	{
		Instance = this;
		_scoreLabel = GetNode<Label>("HBoxContainer/ScoreVBox/ScoreLabel");
		_highScoreLabel = GetNode<Label>("HBoxContainer/HighScoreVBox/HighScoreLabel");
		_score = 0;
		UpdateScoreLabel();
		_highScore = 200;
		UpdateHighScoreLabel();
		InitialiseScores();

		SignalBroadCaster.Instance.OnPointScored += OnPointScored;
	}

	private void OnPointScored(uint points)
	{
		UpdateScore(points);
	}
}
