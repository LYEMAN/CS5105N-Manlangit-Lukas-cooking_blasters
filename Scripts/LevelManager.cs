
using Godot;

public partial class LevelManager : Node
{
	[Export] private PackedScene nextLevel;
	[Export] private NodePath spawnerPath;

	private EnemySpawner spawner;
	private bool spawningFinished = false;
	private bool levelComplete = false;

	public override void _Ready()
	{
		spawner = GetNodeOrNull<EnemySpawner>(spawnerPath);

		if (spawner == null)
		{
			GD.PrintErr("EnemySpawner could not be found.");
			return;
		}

		spawner.SpawningFinished += OnSpawningFinished;

		GD.Print("LevelManager connected to EnemySpawner.");
	}

	private void OnSpawningFinished()
	{
		spawningFinished = true;

		GD.Print("Spawner finished all waves.");
	}

	public override void _Process(double delta)
	{
		if (levelComplete)
			return;

		if (!spawningFinished)
			return;

		int enemyCount = GetTree().GetNodesInGroup("Enemies").Count;

		GD.Print("Enemies remaining: ", enemyCount);

		if (enemyCount <= 0)
		{
			levelComplete = true;

			GD.Print("All enemies defeated!");
			LoadNextLevel();
		}
	}

	private void LoadNextLevel()
	{
		if (nextLevel == null)
		{
			GD.PrintErr("Next level has not been assigned.");
			return;
		}

		GD.Print("Loading next level...");

		GetTree().ChangeSceneToPacked(nextLevel);
	}
}
