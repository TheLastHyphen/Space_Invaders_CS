using Godot;

public abstract partial class InvaderBase : Area2D
{
	private int _direction = 1;
	private bool _droppingDown = false;
	private bool _canMove = true;
	private bool _frame = true;
	
	public AnimatedSprite2D InvAnimatedSprite => GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	public abstract int Points { get; }
	public abstract string InvaderName { get; }
	public abstract float BombDropChance { get; }

	public override void _Ready()
	{
		SignalBroadCaster.Instance.OnMoveTimerTimeOut += OnMoveUpdate;
		SignalBroadCaster.Instance.OnEdgeOfScreen += OnEdgeOfScreen;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		SignalBroadCaster.Instance.OnMoveTimerTimeOut -= OnMoveUpdate;
		SignalBroadCaster.Instance.OnEdgeOfScreen -= OnEdgeOfScreen;
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
		if(_droppingDown == false)
		{
			Move();
		
			if(DropBomb() == true)
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
		GD.Print("Dropping a bomb");
		uint index = GD.Randi() % 2;
		InvaderBombBase invBomb = PackedScenes.Instance.InvaderBombs[index].Instantiate<InvaderBombBase>();
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
		float num = GD.Randf();
		GD.Print("bomb chance: ", num);
		return num;
	}
}
