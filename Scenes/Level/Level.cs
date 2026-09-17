using Godot;
using System;
using System.Data.SqlTypes;
using System.Threading.Tasks;

public enum InvaderName
{
	Squid = 0,
	Crab,
	Octopus
}

public partial class Level : Node2D
{
	private InvaderSquid _squid;

	[Export]
	int InvadersInRow { get; set; } = 200;
	const float InvaderXSpacing = 50.0f;
	const float InvaderYSpacing = 50.0f;
	private Vector2 row1StartPosition => new(33,90);
	private Vector2 row2StartPosition => new(33,120);
	private Vector2 row3StartPosition => new(33,150);
	private Vector2 row4StartPosition => new(33,180);
	private Vector2 row5StartPosition => new(33,210);

	private int _totalInvaders = 0;

	public async override void _Ready()
	{
		base._Ready();
		await SetupInvaders();
	}

	public async Task SetupInvaders()
	{
		_totalInvaders = 0;
		await SetUpRow(PackedScenes.Instance.InvaderSquid, row1StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderCrab, row2StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderCrab, row3StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderOctopus, row4StartPosition, Colors.White);
		await SetUpRow(PackedScenes.Instance.InvaderOctopus, row5StartPosition, Colors.White);
		SignalBroadCaster.Instance.EmitOnFinishedDrawingInvaders();
	}

	public async Task SetUpRow(PackedScene b, Vector2 rowStart, Color color)
	{
		for(int i = 0; i < InvadersInRow; i++)
		{
			InvaderBase invader = b.Instantiate<InvaderBase>();
			invader.Position = new Vector2(rowStart.X + (InvaderXSpacing * i), rowStart.Y);
			invader.InvAnimatedSprite.Modulate = color;
			AddChild(invader);
			SignalBroadCaster.Instance.OnUpdateNumberOfInvaders(++_totalInvaders);
			await WaitDrawInvader();
		}
	}

	public async Task WaitDrawInvader()
	{
		await ToSignal(GetTree().CreateTimer(0.011), "timeout");
	}
}
