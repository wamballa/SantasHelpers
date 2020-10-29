using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentCollision : MonoBehaviour
{

  Present parent;

  // Start is called before the first frame update
  void Start()
  {
    parent = gameObject.GetComponent<Present>();
  }

  // Update is called once per frame
  void Update()
  {

  }
  // Machine Collision
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "EndOfBelt")
    {
      CakeHitEndOfBelt();
    }

    // if (collision.tag == "Machine" && isAlive)
    // {
    //     CheckIfOvenReady(collision);
    // }
    // if (collision.tag == "Player" && isAlive)
    // {
    //     if (collision.GetComponent<PlayerR>().GetIsReadyToPack())
    //     {
    //         PutOnTruck();
    //     }
    //     else
    //     {
    //         Debug.Log("PRESENT: Collision with Player");
    //         MoveUp();
    //     }
    // }
    // if (collision.tag == "Mouse" && isAlive)
    // {
    //     //CakeDead();
    //     DropCakeForMouseToEat();
    // }
  }
  void CakeHitEndOfBelt()
  {
    Debug.Log("Hit End Of Belt");
    // parent.isAlive = false;
    // parent.rb.velocity = Vector2.zero;
    // parent.rb.gravityScale = -parent.gravity;
  }

}
