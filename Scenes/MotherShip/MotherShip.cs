using Godot;

public partial class MotherShip : Area2D
{
	[Export] public Vector2 StartPosition { get; set; }
	[Export] public float Speed { get; set; } = 100.0f;
	[Signal] public delegate void OnMotherShipHitEventHandler();
	public int Direction { get; set; }

	private AnimatedSprite2D _explode;
	private Label _scoreLabel;
	private Sprite2D _motherShip;
	private bool _canMove = true;

	private AudioStreamOggVorbis _motherShipSound =
		ResourceLoader.Load<AudioStreamOggVorbis>("res://Assets/Audio/spaceEngine_000.ogg");

	private AudioStreamOggVorbis _motherShipExplodeSound =
		ResourceLoader.Load<AudioStreamOggVorbis>("res://Assets/Audio/lowFrequency_explosion_000.ogg");

	private AudioStreamPlayer2D _audioPlayer = new();

	private uint[] _scores = { 300, 250, 200, 150, 100, 50, 25 };

	public override void _Ready()
	{
		base._Ready();
		_explode = GetNode<AnimatedSprite2D>("Explode");
		_explode.AnimationFinished += OnAnimationFinished;
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_scoreLabel.Visible = false;
		_explode.Visible = false;
		_motherShip = GetNode<Sprite2D>("Sprite2D");
		AreaEntered += OnMotherShipAreaEntered;
		AddChild(_audioPlayer);
		_audioPlayer.Stream = _motherShipSound;
		_audioPlayer.VolumeDb = -5;
		_audioPlayer.Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (!_canMove) return;
		Vector2 position = Position;
		position.X += Speed * (float)delta * Direction;
		Position = position;
	}
	
	private void OnMotherShipAreaEntered(Area2D area)
	{
		SetDeferred("monitoring", false);
		if(area.Name == "LeftKillBoundary" || area.Name == "RightKillBoundary")
		{
			EmitSignal(SignalName.OnMotherShipHit);
			QueueFree();
		}
		
		if(area.Name == "PlayerMissile")
		{
			area.QueueFree();
			_audioPlayer.Stop();
			_audioPlayer.Stream = _motherShipExplodeSound;
			_audioPlayer.VolumeDb = 5;
			_audioPlayer.Play();
			_canMove = false;
			_motherShip.Visible = false;
			_explode.Visible = true;
			_explode.Play();
		}
	}

	private uint GetScore()
	{
		return _scores[GD.Randi() % _scores.Length];
	}
	
	private async void OnAnimationFinished()
	{
		uint score = GetScore();
		_scoreLabel.Text = $"{score}";
		_scoreLabel.Visible = true;
		ScoreBoard.Instance.UpdateScore(score);
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		EmitSignal(SignalName.OnMotherShipHit);
		QueueFree();
	}
}
