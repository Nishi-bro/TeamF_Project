using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    public float speed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical"); Z 移動

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.05f);

        transform.Translate(movement * speed); 
    }
    
}


