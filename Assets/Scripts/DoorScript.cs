using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpened;
    private void Start()
    {
        _isOpened = false;
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        _isOpened = !_isOpened;
        _animator.SetBool("IsOpen", _isOpened);
    }
}
