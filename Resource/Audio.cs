using Godot;

public partial class Audio
{
	private readonly AudioStreamWav[] _moveSounds = { ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_1.wav"),
											ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_2.wav"),
											ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_3.wav"),
											ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/Invader_Move_4.wav"),									
										};
	
	// defines an indexer
	public AudioStreamWav this[int index] => _moveSounds[index];
}