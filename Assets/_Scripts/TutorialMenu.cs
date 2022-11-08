using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenu : MonoBehaviour
{
    int curPage = 0;
    [SerializeField] GameObject[] pages = new GameObject[3];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in pages) obj.SetActive(false);
        pages[curPage].SetActive(true);
    }

    public void Next()
    {
        curPage++;
        if(curPage == pages.Length) curPage = 0;
    }   
    
    public void Prev()
    {
        curPage--;
        if (curPage < 0) curPage = pages.Length - 1;
    }

    public void Close()
    {
        curPage = 0;
        gameObject.SetActive(false);
    }
}
