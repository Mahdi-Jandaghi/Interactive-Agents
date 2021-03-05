using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 10f;
    void Start() {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        float translation = Input.GetAxis("Vertical") * speed;
        float translation2 = Input.GetAxis("Horizontal") * speed;

        translation *= Time.deltaTime;
        translation2 *= Time.deltaTime;

        transform.Translate(translation2, 0, translation);



        if (Input.GetKey(KeyCode.Alpha2)) {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKey(KeyCode.Alpha1)) {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
