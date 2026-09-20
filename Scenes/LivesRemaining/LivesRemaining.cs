using Godot;

public partial class LivesRemaining : Node2D
{
	private TextureRect _life1;
	private TextureRect _life2;
	private int _remainingLives = 3;
	private Label _liveLabel;

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnPlayerHit -= OnAreaEntered;
	}

	public override void _Ready()
	{
		_life1 = GetNode<TextureRect>("HBoxContainer/Life1");
		_life2 = GetNode<TextureRect>("HBoxContainer/Life2");
		_liveLabel = GetNode<Label>("HBoxContainer/LivesLabel");
		_liveLabel.Text = "3";
		SignalBroadCaster.Instance.OnPlayerHit += OnAreaEntered;
	}

	public void UpdateLives()
	{
		_remainingLives -= 1;
		switch(_remainingLives)
		{
			case 0: _liveLabel.Text = "0";
					break;
			case 1: _life2.Hide();
					_liveLabel.Text = "1";
					break;
			case 2: _life1.Hide();
					_liveLabel.Text = "2";
					break;
		}
	}

	private void OnAreaEntered()
	{
		UpdateLives();
	}
}
