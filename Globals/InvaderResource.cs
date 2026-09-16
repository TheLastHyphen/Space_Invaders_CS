using Godot;
using System;

public partial class InvaderResource : Node
{
	public static InvaderResource Instance { get; private set; }

	public PackedScene InvaderSquid = ResourceLoader.Load<PackedScene>("res://Scenes/InvaderSquid/InvaderSquid.tscn");
	public PackedScene InvaderCrab = ResourceLoader.Load<PackedScene>("res://Scenes/InvaderCrab/InvaderCrab.tscn");
	public PackedScene InvaderOctopus = ResourceLoader.Load<PackedScene>("res://Scenes/InvaderOctopus/InvaderOctopus.tscn");
	
	public override void _Ready()
	{
		Instance = this;
	}
}
