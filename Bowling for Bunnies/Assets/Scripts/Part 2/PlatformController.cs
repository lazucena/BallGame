using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    private string[] platformLayout = {
        "* *************",
        "* *   *       *",
        "* *   *   *   *",
        "* *   *   *   *",
        "* *********   *",
        "*     *   *   *",
        "*******   *****",
        "      *        ",
        "****  *        ",
        "*  *  *********",
        "*  *       *  *",
        "*  *****   *  *",
        "*  *   *   *  *",
        "*  *   *   *  *",
        "** *   *   *  *",
        " * *   *   *  *",
        " * *** *****  *",
        " *   *        *",
        " *   *  *******",
        " *   ****      " };
    public GameObject tile;

    // Use this for initialization
    void Start () {
        int row = 0;

        foreach (string value in platformLayout)
        {
            for (int col = 0; col < value.Length; col++)
            {
                
                if (value[col] == '*')
                {
                    GameObject newTile = Instantiate(tile, new Vector3(10 * col, 0, 10 * row), Quaternion.identity, transform);
                }
            }
            row++;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public string[] GetLayout() {
        return platformLayout;
    }
}
