using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadComponent : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerCameraController _cameraController;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            // Trigger the death animation
            if (_animator != null)
            {
                _animator.SetTrigger("Dead");
            }
            else
            {
                Debug.LogWarning("Animator not assigned on PlayerDeadComponent.");
            }
            if (_playerMovement != null)
            {
                _playerMovement.enabled = false;
            }
            else
            {
                Debug.LogWarning("PlayerMovement not assigned on PlayerDeadComponent.");
            }
            if (_cameraController != null)
            {
                _cameraController.SetIsDead(true);
            }
            else
            {
                Debug.LogWarning("PlayerCameraController not assigned on PlayerDeadComponent.");
            }
        }
    }
}
