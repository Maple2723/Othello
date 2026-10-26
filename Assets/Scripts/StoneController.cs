using UnityEngine;

public class StoneController : MonoBehaviour
{
    [SerializeField] private Animator stoneAnime;
    void Start()
    {
         stoneAnime.SetTrigger("PutStone");

    }
}
