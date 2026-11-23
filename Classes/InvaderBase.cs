using Godot;

public abstract partial class InvaderBase : Area2D
{
	[Export] public abstract float BombDropChance { get; set; }
	
	private int _direction = 1;
	private bool _droppingDown;
	private bool _canMove;
	private bool _frame = true;
	private bool _canDropBomb;
	private InvaderBombBase _invBomb;
	private int _moveSoundIndex = 0;
	
	public abstract AnimatedSprite2D InvAnimatedSprite { get; }

	protected abstract uint Points { get; }
	protected abstract string InvaderName { get; }

	public override void _Ready()
	{
		base._Ready();
		InvAnimatedSprite.Animation = "Move";
		SignalBroadCaster.Instance.OnMoveTimerTimeOut += OnMoveUpdate;
		SignalBroadCaster.Instance.OnEdgeOfScreen += OnEdgeOfScreen;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders += OnFinishedDrawingInvaders;
		AreaEntered += OnAreaEntered;
		InvAnimatedSprite.AnimationFinished += OnAnimationFinished;
	}

	private void OnMoveTimerTimeOut()
	{
		OnMoveUpdate();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnMoveTimerTimeOut -= OnMoveUpdate;
		SignalBroadCaster.Instance.OnEdgeOfScreen -= OnEdgeOfScreen;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders -= OnFinishedDrawingInvaders;
	}

	private void OnFinishedDrawingInvaders()
	{
		_canMove = true;
	}

	private void Move()
	{
		if (!_canMove) return;

		Position = new Vector2(Position.X + 15 * _direction, Position.Y);
		InvAnimatedSprite.Frame = _frame ? 1 : 0;
		_frame = !_frame;
	}

	private void OnMoveUpdate()
	{
		if(!_droppingDown)
		{
			Move();
		
			if(DropBomb())
			{
				CreateInvaderBomb();
			}
		}
		else
		{
			Position = new Vector2(Position.X, Position.Y + 20);
			_direction *= -1;
			_droppingDown = false;
		}
	}

	private void CreateInvaderBomb()
	{
		_invBomb = PackedScenes.Instance.InvaderBombs[GD.Randi() % 2].Instantiate<InvaderBombBase>();
		_invBomb.Position = Position;
		GetParent().AddChild(_invBomb);
	}

	private void OnEdgeOfScreen()
	{
		_droppingDown = true;
	}

	private bool DropBomb()
	{
		return RandomFloat() <= BombDropChance; 
	}

	private float RandomFloat()
	{
		return GD.Randf();
	}

	private async void OnAreaEntered(Area2D area)
	{
		//GD.Print(area.Name);
		if (area.Name == "LeftBoundary" || area.Name == "RightBoundary") return;
		if (area.Name == "FloorBoundary")
		{
			GD.Print("Invader hit the floor");
			SignalBroadCaster.Instance.EmitOnInvaderHitFloor();
		}
		if (area.Name != "PlayerMissile") return;

		AreaEntered -= OnAreaEntered;
		_canMove = false;
		SetDeferred("monitorable", false);
		SignalBroadCaster.Instance.EmitOnPointScored(Points);
		InvAnimatedSprite.Animation = "Explode";
		InvAnimatedSprite.Scale = new Vector2(2.0f, 2.0f);
		InvAnimatedSprite.Play("Explode");
		SoundManager.Instance.PlayStream(SoundEffect.InvaderKilled);
		SignalBroadCaster.Instance.EmitOnInvaderKilled();
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}
}
