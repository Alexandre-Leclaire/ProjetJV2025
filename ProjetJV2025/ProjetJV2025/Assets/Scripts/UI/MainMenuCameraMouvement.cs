using UnityEngine;

public class MainMenuCameraMouvement : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ChangeAnimation(string name)
    {
        animator.SetFloat("speed", 1);
        animator.Play(name);
    }

    public void Reverse()
    {
        animator.SetFloat("speed", -1);
    }
}
