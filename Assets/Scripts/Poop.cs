using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Poop : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameManager.instance.Score();
            animator.SetTrigger("poop");
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // // 똥이 Player와 충돌시 게임 오버 처리 
            GameManager.instance.GameOver();
            animator.SetTrigger("poop"); 
        }
        else
        {
            return;
        }
    }
}
