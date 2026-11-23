using Godot;
using System.Threading.Tasks;

public enum InvaderName
{
	Squid = 0,
	Crab,
	Octopus
}

public partial class Level : Node2D
{
	[Signal]
	public delegate void OnIncrementInvaderEventHandler(int number);

	private InvaderSquid _squid;

	[Export]
	public int InvadersInRow { get; set; } = 20;

	[Export]
	public float StartPositionX { get; set; } = 70; //33

	[Export]
	public float StartPositionY { get; set; } = 90;

	[Export]
	public float ColSpacing { get; set; } = 50;

	[Export]
	public float RowSpacing { get; set; } = 30;

	private Vector2 Row1StartPosition => new(StartPositionX, StartPositionY);
	private Vector2 Row2StartPosition => new(StartPositionX, StartPositionY + RowSpacing);
	private Vector2 Row3StartPosition => new(StartPositionX, StartPositionY + RowSpacing * 2);
	private Vector2 Row4StartPosition => new(StartPositionX, StartPositionY + RowSpacing * 3);
	private Vector2 Row5StartPosition => new(StartPositionX, StartPositionY + RowSpacing * 4);

	private int _totalInvaders = 0;

	public async override void _Ready()
	{
		base._Ready();
		await SetupInvaders();
	}

	public async Task SetupInvaders()
	{
		base._Ready();
		_totalInvaders = 0;
		await SetUpRow(PackedScenes.Instance.InvaderSquid, Row1StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderCrab, Row2StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderCrab, Row3StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderOctopus, Row4StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderOctopus, Row5StartPosition, Colors.White);
		SignalBroadCaster.Instance.EmitOnFinishedDrawingInvaders();
	}

	public async Task SetUpRow(PackedScene b, Vector2 rowStart, Color color)
	{
		for(int i = 0; i < InvadersInRow; i++)
		{
			InvaderBase invader = b.Instantiate<InvaderBase>();
			AddChild(invader);
			invader.Position = new Vector2(rowStart.X + (ColSpacing * i), rowStart.Y);
			invader.InvAnimatedSprite.Modulate = color;
			EmitSignal("OnIncrementInvader", ++_totalInvaders);
			await WaitDrawInvader();
		}
	}

	public async Task WaitDrawInvader()
	{
		await ToSignal(GetTree().CreateTimer(0.011), "timeout");
	}
}
