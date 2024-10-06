using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPointer : MonoBehaviour
{
    public string option = "Rematch";
    public bool confirm = false;
    // Start is called before the first frame update
    public string getOption()
    {
        return option;
    }

    // Update is called once per frame
    void Update()
    {   
        while (confirm == false)
        {
            if (Input.GetKeyDown("l") && option != "Rematch"){
            option = "Rematch";
            transform.position = new Vector3(transform.position.x, -90, transform.position.z);
            }
            else if (Input.GetKeyDown("k") && option != "Return"){
                option = "Return";
                transform.position = new Vector3(transform.position.x, -230, transform.position.z);
            }
            else if (Input.GetKeyDown("space")){
                confirm = true;
            }
        }
    }
}
