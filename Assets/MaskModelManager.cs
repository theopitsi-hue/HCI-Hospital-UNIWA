using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskModelManager : MonoBehaviour
{
    public BlackboardKey inUse;
    public GameObject stand;
    public GameObject stand_cbl;
    public GameObject face;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inUse.TryGetValue(out var b))
        {
            if ((bool)b.GetValue())
            {
                stand.SetActive(false);
                face.SetActive(true);
                stand_cbl.SetActive(false);
            }
            else
            {
                stand.SetActive(true);
                face.SetActive(false);
                stand_cbl.SetActive(true);

            }
        }
    }
}
