using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rbody;
    public Animator anim;

    // Use this for initialization
    void Start()
    {

        rbody.GetComponent<Rigidbody2D>();
        anim.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

		Vector2 movement_vector = new Vector2(CnControls.CnInputManager.GetAxisRaw("Horizontal"), CnControls.CnInputManager.GetAxisRaw("Vertical"));

        if (movement_vector != Vector2.zero)
        {
            anim.SetBool("iswalking", true);
            anim.SetFloat("input_x", movement_vector.x);
            anim.SetFloat("input_y", movement_vector.y);

        }
        else
        {
            anim.SetBool("iswalking", false);
        }

        rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime);
    }
}	
