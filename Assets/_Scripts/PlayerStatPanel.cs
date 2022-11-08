using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanel : MonoBehaviour
{
    [SerializeField] Text Name;
    [SerializeField] Text KillCount;
    [SerializeField] Text Killer;


    public void SetText(string a, string b, string c)
    {
        Name.text = a;
        KillCount.text = b;
        if(c == "none") Killer.text = "-";
        else Killer.text = c;
    }
}
