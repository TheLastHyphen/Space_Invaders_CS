using Godot;

public partial class ScoreDisplay : Control
{
	public static ScoreDisplay Instance { get; private set; }

	private Label _scoreLabel;
	private Label _highScoreLabel;
	private int _score;
	private int _highScore;

	public void UpdateScore(int points)
	{
		_score += points;
		UpdateScoreLabel();
		UpdateHighScore();
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
	}
}
