using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Space(10), Header("Spawning Settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField, Min(0.1f)] private float _spawnInterval = 3f;
    [SerializeField, Min(1f)] private float _spawnRadius = 15f;
    [SerializeField, Min(1)] private int _maxEnemies = 10;
    
    [Space(10), Header("Camera Settings")]
    [SerializeField, Min(1f)] private float _cameraViewPadding = 2f;

    private Transform _playerTransform;
    private Camera _mainCamera;
    private float _spawnTimer;
    private int _currentEnemies = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        _mainCamera = Camera.main;
        _spawnTimer = _spawnInterval;
    }

    void Update()
    {
        if (_playerTransform == null || _mainCamera == null)
            return;
        if (_currentEnemies >= _maxEnemies)
            return;

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f)
        {
            SpawnEnemy();
            _spawnTimer = _spawnInterval;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemyPrefabs == null)
        {
            Debug.LogError("Enemy prefab not assigned to EnemySpawner!");
            return;
        }

        Vector3 spawnPosition = GetSpawnPosition();
        Instantiate(PickRandomEnemyPrefab(), spawnPosition, Quaternion.identity);
        _currentEnemies++;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 playerPos = _playerTransform.position;
        Vector3 spawnPos = Vector3.zero;
        bool isOutOfView = false;

        // Keep generating positions until we find one outside camera view
        while (!isOutOfView)
        {
            // Generate a random position around the player within spawn radius
            Vector2 randomCircle = Random.insideUnitCircle.normalized * _spawnRadius;
            spawnPos = playerPos + new Vector3(randomCircle.x, 0f, randomCircle.y);

            // Check if position is outside camera view
            Vector3 screenPos = _mainCamera.WorldToViewportPoint(spawnPos);
            isOutOfView = screenPos.x < -_cameraViewPadding || screenPos.x > 1f + _cameraViewPadding ||
                         screenPos.y < -_cameraViewPadding || screenPos.y > 1f + _cameraViewPadding ||
                         screenPos.z < 0f;
        }

        return spawnPos;
    }

    private GameObject PickRandomEnemyPrefab()
    {
        return _enemyPrefabs[Random.Range(0, _enemyPrefabs.Length - 1)];
    }

    public void EnemyKilled()
    {
        _currentEnemies--;
    }
}
