using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Initialize(StoneManager.StoneType type)
    {
        Play(type == StoneManager.StoneType.Black ? "PutBlack" : "PutWhite");
    }

    public void FlipStone(int currentStoneData)
    {
        Play(currentStoneData == (int)StoneManager.StoneType.Black ? "FlipToWhite" : "FlipToBlack");
    }

    private void Play(string stateName)
    {
        if (animator == null)
        {
            Debug.LogError("Stone Animator is missing. Run Tools/Othello/Create Stone Animations.", this);
            return;
        }
        animator.Play(stateName, 0, 0f);
        animator.Update(0f);
    }
}
