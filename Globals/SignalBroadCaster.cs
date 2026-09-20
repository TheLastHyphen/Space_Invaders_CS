using System;
using Godot;

public partial class SignalBroadCaster : Node
{
	public static SignalBroadCaster Instance { get; private set; }

	public Action OnMoveTimerTimeOut;
	public Action OnFinishedDrawingInvaders;
	public Action<int> OnUpdateNumberOfInvaders;
	public Action OnEdgeOfScreen;
	public Action OnCanFireMissile;
	public Action OnInvaderHit;
	public Action OnPlayerHit;

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitOnMoveTimerTimeOut()
	{
		OnMoveTimerTimeOut?.Invoke();
	}

	public void EmitOnFinishedDrawingInvaders()
	{
		OnFinishedDrawingInvaders?.Invoke();
	}

	public void EmitOnUpdateNumberOfInvaders(int number)
	{
		OnUpdateNumberOfInvaders?.Invoke(number);
	}

	public void EmitOnEdgeOfScreen()
	{
		OnEdgeOfScreen?.Invoke();
	}

	public void EmitOnCanFireMissile()
	{
		OnCanFireMissile?.Invoke();
	}

	public void EmitOnInvaderHit()
	{
		OnInvaderHit?.Invoke();
	}

	public void EmitOnPlayerHit()
	{
		OnPlayerHit?.Invoke();
	}
}
