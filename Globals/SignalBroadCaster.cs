using System;
using Godot;

public partial class SignalBroadCaster : Node
{
	public static SignalBroadCaster Instance { get; private set; }

	public Action OnMoveTimerTimeOut;

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitOnMoveTimerTimeOut()
	{
		OnMoveTimerTimeOut?.Invoke();
	}
}
