using System.Collections.Generic;
using UnityEngine;

public class Welcome : MonoBehaviour
{
#if !UNITY_WEBGL || UNITY_EDITOR
    [SerializeField] private List<string> validGreetings;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        WhisperRecognition.OnPlayerTalked += ReactToPlayer;
    }

    private void ReactToPlayer(string playerWords)
    {
        foreach (string word in validGreetings)
        {
            if(playerWords.Contains(word))
            {
                animator.SetTrigger("Greetings");
            }
        }
    }
#endif
}
