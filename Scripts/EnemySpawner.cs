
using Godot;

public partial class EnemySpawner : Area2D
{
	[Export] private float interval = 1.0f;
	[Export] private int enemiesPerWave = 5;
	[Export] private int waves = 3;
	[Export] private float waveInterval = 5.0f;
	[Export] private PackedScene enemyScene;
	[Export] private PackedScene bossScene;

	[Signal]
	public delegate void SpawningFinishedEventHandler();

	private CollisionShape2D _spawnArea;
	private RandomNumberGenerator _random = new RandomNumberGenerator();

	private int _currentWave = 0;

	public override void _Ready()
	{
		_spawnArea = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

		if (_spawnArea == null)
		{
			GD.PrintErr("Could not find CollisionShape2D.");
			return;
		}

		if (enemyScene == null)
		{
			GD.PrintErr("Enemy scene has not been assigned.");
			return;
		}

		_random.Randomize();

		StartSpawning();
	}

	private async void StartSpawning()
	{
		while (_currentWave < waves)
		{
			_currentWave++;

			GD.Print("Starting wave ", _currentWave);

			for (int i = 0; i < enemiesPerWave; i++)
			{
				SpawnEnemy();

				await ToSignal(
					GetTree().CreateTimer(interval),
					SceneTreeTimer.SignalName.Timeout
				);
			}

			GD.Print("Wave ", _currentWave, " finished.");

			if (_currentWave < waves)
			{
				await ToSignal(
					GetTree().CreateTimer(waveInterval),
					SceneTreeTimer.SignalName.Timeout
				);
			}
		}

		GD.Print("All waves finished spawning.");

		EmitSignal(SignalName.SpawningFinished);
	}

	private void SpawnEnemy()
	{
		if (enemyScene == null)
		{
			GD.PrintErr("Enemy scene has not been assigned.");
			return;
		}

		Node2D enemy = enemyScene.Instantiate<Node2D>();

		enemy.AddToGroup("Enemies");

		GetTree().CurrentScene.AddChild(enemy);

		enemy.GlobalPosition = GetRandomPosition();

		GD.Print("Enemy Spawned: ", enemy.GetPath());
	}

	private Vector2 GetRandomPosition()
	{
		RectangleShape2D rectangle =
			_spawnArea.Shape as RectangleShape2D;

		if (rectangle == null)
		{
			GD.PrintErr(
                "CollisionShape2D must use a RectangleShape2D."
			);

			return GlobalPosition;
		}

		Vector2 size = rectangle.Size;

		float x = _random.RandfRange(
			-size.X / 2.0f,
			size.X / 2.0f
		);

		float y = _random.RandfRange(
			-size.Y / 2.0f,
			size.Y / 2.0f
		);

		return _spawnArea.GlobalPosition + new Vector2(x, y);
	}
}
