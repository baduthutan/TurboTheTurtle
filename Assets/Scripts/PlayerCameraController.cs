using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Rigidbody _rb;

    [Header("Combat Reference")]
    [SerializeField] private Transform _combatCenter;

    [Header("Camera Settings")]
    [SerializeField] private GameObject _basicCamera;
    [SerializeField] private GameObject _combatCamera;
    [SerializeField] private GameObject _topdownCamera;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    private bool _isDead = false;

    public enum CameraMode
    {
        Basic,
        Combat,
        Topdown
    }
    private CameraMode _currentCameraMode = CameraMode.Basic;
    private int _cameraModeIndex = 0;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeCameraMode();
        }
        UpdatePlayerOrientation();
    }
    private void UpdatePlayerOrientation()
    {
        // if (_currentCameraMode == CameraMode.Combat && !_isDead)
        // {
        //     Vector3 combatDirection = _combatCenter.position - new Vector3(transform.position.x, _combatCenter.position.y, transform.position.z);
        //     combatDirection.y = 0f; // Only affect xz plane
        //     _orientation.forward = combatDirection.normalized;

        //     // Only affect x axis rotation of _playerBody
        //     Vector3 playerBodyForward = combatDirection.normalized;
        //     playerBodyForward.y = 0f;
        //     if (playerBodyForward != Vector3.zero)
        //     {
        //         float angle = Mathf.Atan2(playerBodyForward.x, playerBodyForward.z) * Mathf.Rad2Deg;
        //         Vector3 currentEuler = _playerBody.eulerAngles;
        //         _playerBody.eulerAngles = new Vector3(currentEuler.x, angle, currentEuler.z);
        //     }
        // }
        // else
        // {
            Vector3 direction = _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
            _orientation.forward = direction.normalized;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 movementDirection = _orientation.forward * verticalInput + _orientation.right * horizontalInput;
            if (movementDirection != Vector3.zero && !_isDead)
            {
                Vector3 targetForward = movementDirection.normalized;
                targetForward.y = 0f;
                float angle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;
                Vector3 currentEuler = _playerBody.eulerAngles;
                float newY = Mathf.LerpAngle(currentEuler.y, angle, Time.deltaTime * _speed);
                _playerBody.eulerAngles = new Vector3(currentEuler.x, newY, currentEuler.z);
            }
        // }
    }
    private void ChangeCameraMode()
    {
        switch (_cameraModeIndex)
        {
            case 0:
                SetCameraMode(CameraMode.Basic);
                _cameraModeIndex = 1;
                break;
            case 1:
                SetCameraMode(CameraMode.Combat);
                _cameraModeIndex = 2;
                break;
            case 2:
                SetCameraMode(CameraMode.Topdown);
                _cameraModeIndex = 0;
                break;
        }
    }
    public void SetCameraMode(CameraMode mode)
    {
        _basicCamera.SetActive(false);
        _combatCamera.SetActive(false);
        _topdownCamera.SetActive(false);
        _currentCameraMode = mode;
        switch (mode)
        {
            case CameraMode.Basic:
                _basicCamera.SetActive(true);
                break;
            case CameraMode.Combat:
                _combatCamera.SetActive(true);
                break;
            case CameraMode.Topdown:
                _topdownCamera.SetActive(true);
                break;
            default:
                _basicCamera.SetActive(true);
                break;
        }
    }
    public void SetIsDead(bool isDead)
    {
        _isDead = isDead;
    }
}
