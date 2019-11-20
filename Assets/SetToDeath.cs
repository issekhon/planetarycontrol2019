using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetToDeath : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerRef;
    private PlayerController myControl;

    void Start()
    {
        myControl = playerRef.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerRef == null)
        {
            transform.GetComponent<Image>().color = new Color32(253, 0, 25, 100);
        }
    }
}
