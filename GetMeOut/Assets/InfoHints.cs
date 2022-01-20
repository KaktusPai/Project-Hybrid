using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoHints : MonoBehaviour
{
    public GameObject infoPanel;
    public Image infoImage;
    public Sprite[] infoSpritesList;
    public int index;
    public bool panelActive = false;
    void Start()
    {
        infoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TogglePanel() 
    {
        panelActive = !panelActive;
        infoPanel.SetActive(panelActive);
    }
    public void AddOrRemoveI(bool add) {
        if(add && index < 5) 
        {
            index += 1;
        } else if (!add & index > 0)
        {
            index -= 1;
        }
        infoImage.sprite = infoSpritesList[index];
    }
}
