using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeacherScript : MonoBehaviour
{
    #region Singleton
    public static TeacherScript instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    public void ChangeAnimation(string responseAnimation)
    {
        switch (responseAnimation)
        {
            case "Correct":
                anim.SetBool("correctAnswer", true);
                StartCoroutine(ResetAnimation("Correct"));
                break;
            case "Incorrect":
                StartCoroutine(ResetAnimation("Incorrect"));
                anim.SetBool("wrongAnswer", true);
                break;
        }
    }

    private IEnumerator ResetAnimation(string responseAnimation)
    {
        yield return new WaitForSeconds(2.5f);

        switch (responseAnimation)
        {
            case "Correct":
                anim.SetBool("correctAnswer", false);
                break;
            case "Incorrect":
                anim.SetBool("wrongAnswer", false);
                break;
        }
    }
}
