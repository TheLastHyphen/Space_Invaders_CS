using Godot;

public partial class Audio
{
	public AudioStreamWav[] MoveSounds = { ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_1.wav"),
											ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_2.wav"),
											ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_3.wav"),
											ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_4.wav"),									
										};
}