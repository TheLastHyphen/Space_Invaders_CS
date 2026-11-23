using Godot;

public abstract partial class InvaderBombBase : Area2D
{
	private int _speed = 0;
	public AnimatedSprite2D InvAnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	public AnimatedSprite2D InvBombExplode => GetNode<AnimatedSprite2D>("Explode");
	private bool _canMove = true;

	private AudioStreamOggVorbis _missleHitSound =
		ResourceLoader.Load<AudioStreamOggVorbis>("res://Assets/Audio/impactMetal_001.ogg");

	private AudioStreamPlayer2D _audioplayer = new();
	
	protected abstract uint Points { get; }

	public override void _Ready()
	{
		_speed = (int)GD.RandRange(75.0f, 400.0f);
		InvBombExplode.AnimationFinished += OnAnimationFinished;
		_audioplayer.VolumeDb = -5;
		AddChild(_audioplayer);
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if(!_canMove) return;
		Vector2 pos = Position;
		pos.Y += 1 * (float)delta * _speed;
		Position = pos;
	}

	public void OnAreaEntered(Area2D area)
	{
		SetDeferred("monitorable", false);
		_canMove = false;
		if(area.Name == "PlayerMissile")
		{
			SignalBroadCaster.Instance.EmitOnPointScored(Points);
		}

		_audioplayer.Stream = _missleHitSound;
		_audioplayer.Play();
		InvAnimatedSprite.Hide();
		InvBombExplode.Play();
		//QueueFree();
	}
}
