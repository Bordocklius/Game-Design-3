using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SpellInputHandler _inputHandler;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private CameraFollow _cameraFollow;

    [SerializeField] private GameObject _enemy;

    [SerializeField] private float _channelDelay;
    [SerializeField] private float _delayTime;

    void Start()
    {
        StartCoroutine(TutorialCoroutine());
    }

    private IEnumerator TutorialCoroutine()
    {
        yield return new WaitForSeconds(2f);

        _inputHandler.OnSpellSlot1(null);
        yield return new WaitForSeconds(_channelDelay);
        _inputHandler.OnAttack(null);

        yield return new WaitForSeconds(_channelDelay);
        _inputHandler.OnSpellSlot2(null);
        yield return new WaitForSeconds(_channelDelay);
        _inputHandler.OnAttack(null);

        _playerMovement.enabled = true;
        _playerInput.enabled = true;

        _enemySpawner.EnableSpawning = true;
        _cameraFollow.enabled = true;
        _audioSource.Play();
    }
}
