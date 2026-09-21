using Godot;

public abstract partial class InvaderBase : Area2D
{
	private int _direction = 1;
	private bool _droppingDown = false;
	private bool _canMove = false;
	private bool _frame = true;
	private bool _canDropBomb = false;
	private InvaderBombBase invBomb = null;
			
	public AnimatedSprite2D InvAnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	public abstract int Points { get; }
	public abstract string InvaderName { get; }
	[Export]
	public abstract float BombDropChance { get; set; }

	public override void _Ready()
	{
		SignalBroadCaster.Instance.OnMoveTimerTimeOut += OnMoveUpdate;
		SignalBroadCaster.Instance.OnEdgeOfScreen += OnEdgeOfScreen;
		SignalBroadCaster.Instance.OnFinishedDrawingInvaders += OnFinishedDrawingInvaders;
		this.AreaEntered += OnAreaEntered;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnMoveTimerTimeOut -= OnMoveUpdate;
		SignalBroadCaster.Instance.OnEdgeOfScreen -= OnEdgeOfScreen;
	}

	private void OnFinishedDrawingInvaders()
	{
		_canMove = true;
	}

	private void Move()
	{
		if(_canMove)
		{
			Position = new Vector2(Position.X + 15 * _direction, Position.Y);
			if(_frame)
			{
				InvAnimatedSprite.Frame = 1;
			}
			else
			{
				InvAnimatedSprite.Frame = 0;
			}
			_frame = !_frame;
		}
	}

	private void OnMoveUpdate()
	{
		//_canDropBomb = false;
		if(_droppingDown == false)
		{
			Move();
		
			if(DropBomb() == true)// && GodotObject.IsInstanceValid(invBomb) == false)
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
		invBomb = PackedScenes.Instance.InvaderBombs[GD.Randi() % 2].Instantiate<InvaderBombBase>();
		invBomb.Position = Position;
		GetParent().AddChild(invBomb);
	}

	public void OnEdgeOfScreen()
	{
		_droppingDown = true;
	}

	private bool DropBomb()
	{
		if(RandomFloatZeroToOne() <= BombDropChance) return true;
		return false;
	}

	private float RandomFloatZeroToOne()
	{
		return GD.Randf();
	}

	private void OnAreaEntered(Area2D area)
	{
		if(area.Name == "PlayerMissile")
		{
			ScoreDisplay.Instance.UpdateScore(Points);
			SignalBroadCaster.Instance.OnInvaderHit();
			QueueFree();
		}
	}
}
