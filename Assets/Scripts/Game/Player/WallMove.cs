using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    // Start is called before the first frame update
    public string wallTag = "Wall";
    public bool CanMoveRight {  get; private set;   } = true;
    public bool CanMoveLeft {   get; private set;   } = true;

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag(wallTag)){
            
            if(other.transform.position.x > transform.position.x){

                CanMoveRight = false;
            }

            else if(other.transform.position.x < transform.position.x){

                CanMoveLeft = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag(wallTag)){

            if(other.transform.position.x > transform.position.x){
                
                CanMoveRight = true;
            }

            else if(other.transform.position.x < transform.position.x){

                CanMoveLeft = true;
            }
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
