using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{

    [Header("Camera Settings")]
    [SerializeField] private GameObject _basicCamera;
    [SerializeField] private GameObject _combatCamera;
    [SerializeField] private GameObject _topdownCamera;

    [Header("Animator Settings")]
    [SerializeField] private Animator _animator;

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
        // if(_isHiding) return;
        // Vector3 direction = _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
        // _orientation.forward = direction.normalized;
        float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
        // Vector3 movementDirection = _orientation.forward * verticalInput + _orientation.right * horizontalInput;
        // if (movementDirection != Vector3.zero && !_isDead)
        // {
        //     Vector3 targetForward = movementDirection.normalized;
        //     targetForward.y = 0f;
        //     float angle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;
        //     Vector3 currentEuler = _playerBody.eulerAngles;
        //     float newY = Mathf.LerpAngle(currentEuler.y, angle, Time.deltaTime * _speed);
        //     _playerBody.eulerAngles = new Vector3(currentEuler.x, newY, currentEuler.z);
        // }
        float offset = _animator.GetFloat("Offset");
        if (horizontalInput < 0)
            offset = 0.5f;
        else if (horizontalInput > 0)
            offset = 0f;
        _animator.SetFloat("Offset", offset);
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
}
