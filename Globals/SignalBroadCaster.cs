using System;
using Godot;

public partial class SignalBroadCaster : Node
{
	public static SignalBroadCaster Instance { get; private set; }

	public Action OnMoveTimerTimeOut;
	public Action OnMoveInvader;
	public Action OnFinishedDrawingInvaders;
	//public Action<int> OnUpdateNumberOfInvaders;
	public Action OnEdgeOfScreen;
	public Action OnCanFireMissile;
	public Action<Area2D> OnInvaderHit;
	public Action OnInvaderKilled;
	public Action OnInvaderHitFloor;
	public Action OnPlayerHit;
	public Action<Area2D> OnInvaderBombHit;
	public Action<Vector2, String> OnPlayerMissileHit;
	public Action OnExtraLife;
	public Action<uint> OnPointScored;

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitOnMoveInvader()
	{
		OnMoveInvader?.Invoke();
	}

	public void EmitOnPointScored(uint points)
	{
		OnPointScored?.Invoke(points);
	}

	public void EmitOnMoveTimerTimeOut()
	{
		OnMoveTimerTimeOut?.Invoke();
	}

	public void EmitOnFinishedDrawingInvaders()
	{
		OnFinishedDrawingInvaders?.Invoke();
	}

	public void EmitOnEdgeOfScreen()
	{
		OnEdgeOfScreen?.Invoke();
	}

	public void EmitOnCanFireMissile()
	{
		OnCanFireMissile?.Invoke();
	}

	public void EmitOnInvaderHit(Area2D area)
	{
		OnInvaderHit?.Invoke(area);
	}

	public void EmitOnPlayerHit()
	{
		OnPlayerHit?.Invoke();
	}

	public void EmitOnInvaderBombHit(Area2D area)
	{
		OnInvaderBombHit?.Invoke(area);
	}

	public void EmitOnPlayerMissileHit(Vector2 position, String name)
	{
		OnPlayerMissileHit?.Invoke(position, name);
	}

	public void EmitOnExtraLife()
	{
		OnExtraLife?.Invoke();
	}

	public void EmitOnInvaderKilled()
	{
		OnInvaderKilled?.Invoke();
	}

	public void EmitOnInvaderHitFloor()
	{
		OnInvaderHitFloor?.Invoke();
	}
}
