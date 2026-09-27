using Godot;

public partial class ScoreDisplay : Control
{
	public static ScoreDisplay Instance { get; private set; }

	private Label _scoreLabel;
	private Label _highScoreLabel;
	private uint _score;
	private uint _highScore;
	private uint _extraLifePoints = 1500;

	private uint _nextLifePointsAt;
	public uint NextLifePointsAt 
	{
		get => _nextLifePointsAt;
		set => _nextLifePointsAt = value;
	}

	public void InitialiseScores()
	{
		_score = 0;
		NextLifePointsAt = _extraLifePoints;
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
		if(_score > 0)
		{
			if(_score >= _nextLifePointsAt)
			{
				_nextLifePointsAt += _extraLifePoints;
				SignalBroadCaster.Instance.EmitOnExtraLife();
			}
		}
	}

	public void ClearScore()
	{
		_score = 0;
		UpdateScoreLabel();
	}

	private void UpdateHighScore()
	{
		if(_score > _highScore)
		{
			_highScore = _score;
			UpdateHighScoreLabel();
		}
	}

	private void UpdateScoreLabel()
	{
		_scoreLabel.Text = _score.ToString("D6");	// pad with zeros
	}

	private void UpdateHighScoreLabel()
	{
		_highScoreLabel.Text = _highScore.ToString("D6");	// pad with zeros
	}

	public override void _Ready()
	{
		Instance = this;
		_scoreLabel = GetNode<Label>("HBoxContainer/ScoreVBox/ScoreLabel");
		_highScoreLabel = GetNode<Label>("HBoxContainer/VBoxContainer/HighScoreLabel");
		_score = 0;
		UpdateScoreLabel();
		_highScore = 200;
		UpdateHighScoreLabel();
		InitialiseScores();
	}
}
