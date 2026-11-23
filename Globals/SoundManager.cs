using Godot;

public enum SoundEffect
{
	PlayerMissile = 0,
	PlayerDie,
	InvaderKilled,
	InvaderMove
}

public partial class SoundManager : Node
{
	public static SoundManager Instance { get; private set; }
	private AudioStreamPlayer2D _playerAudioPlayer = new();
	private AudioStreamPlayer2D _invaderAudioPlayer = new();
	private AudioStreamPlayer2D _invaderMoveAudioPlayer = new();
	private AudioStreamWav _playerMissileSound;
	private AudioStreamWav _invaderKilledSound;
	private readonly Audio _invaderMoveSounds = new();
	private int _moveSoundIndex = 0;

	public override void _Ready()
	{
		Instance = this;
		_playerMissileSound = ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/PlayerFire_alt.wav");
		_invaderKilledSound = ResourceLoader.Load<AudioStreamWav>("res://Assets/Audio/InvaderPop.wav");
		AddChild(_playerAudioPlayer);
		AddChild(_invaderAudioPlayer);
		AddChild(_invaderMoveAudioPlayer);
	}

	public void PlayStream(SoundEffect soundEffect)
	{
		if(States.Instance.IsSoundsMute) return;
		
		switch(soundEffect)
		{
			case SoundEffect.PlayerMissile:
				_playerAudioPlayer.Stream = _playerMissileSound;
				_playerAudioPlayer.Play();
				break;
			case SoundEffect.InvaderKilled:
				_invaderAudioPlayer.Stream = _invaderKilledSound;
				_invaderAudioPlayer.Play();
				break;
			case SoundEffect.InvaderMove:
				if(_moveSoundIndex > 3) _moveSoundIndex = 0;
				_invaderMoveAudioPlayer.VolumeLinear = 1.0f;
				_invaderMoveAudioPlayer.Stream = _invaderMoveSounds[_moveSoundIndex++];
				_invaderMoveAudioPlayer.Play();
				break;
		}
	}

	public void StopStream(SoundEffect soundEffect)
	{
		if(States.Instance.IsSoundsMute) return;
		
		switch(soundEffect)
		{
			case SoundEffect.PlayerMissile:
				_playerAudioPlayer.Stop();
				break;
			case SoundEffect.InvaderKilled:
				_invaderAudioPlayer.Stop();
				break;
			case SoundEffect.InvaderMove:
				_invaderMoveAudioPlayer.Stop();
				break;
		}
	}
}
