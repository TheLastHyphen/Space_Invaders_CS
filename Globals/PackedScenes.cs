using Godot;
using System;
using System.Collections.Generic;

public partial class PackedScenes : Node
{
	public static PackedScenes Instance { get; private set; }

	public PackedScene level = ResourceLoader.Load<PackedScene>("res://Scenes/Level/Level.tscn");
	public PackedScene InvaderSquid = ResourceLoader.Load<PackedScene>("res://Scenes/InvaderSquid/InvaderSquid.tscn");
	public PackedScene InvaderCrab = ResourceLoader.Load<PackedScene>("res://Scenes/InvaderCrab/InvaderCrab.tscn");
	public PackedScene InvaderOctopus = ResourceLoader.Load<PackedScene>("res://Scenes/InvaderOctopus/InvaderOctopus.tscn");
	public PackedScene Player = ResourceLoader.Load<PackedScene>("res://Scenes/Player/Player.tscn");
	public PackedScene Game = ResourceLoader.Load<PackedScene>("res://Scenes/Game/Game.tscn");
	public PackedScene MainUI = ResourceLoader.Load<PackedScene>("res://Scenes/MainUI/MainUi.tscn");
	public PackedScene PlayerMissile = ResourceLoader.Load<PackedScene>("res://Scenes/PlayerMissile/PlayerMissile.tscn");

	public PackedScene[] InvaderBombs =
	[
		ResourceLoader.Load<PackedScene>("res://Scenes/InvaderBombType1/InvaderBombType1.tscn"),
		ResourceLoader.Load<PackedScene>("res://Scenes/InvaderBombType2/InvaderBombType2.tscn")
	];
	
	public override void _Ready()
	{
		Instance = this;
	}
}
