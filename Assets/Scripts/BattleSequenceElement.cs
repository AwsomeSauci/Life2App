using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceElement : MonoBehaviour
{
    [SerializeField] private BattleController controller;
    [SerializeField] private GameObject[] images;
    [SerializeField] private GameObject selectImage;
    public int crntImage;
    [SerializeField] private bool IsSequenceElement;

    public void OnObjectClick()
    {
        if (IsSequenceElement) controller.SetSequenceSelected(this);
        else controller.SetSequenceAction(this);
    }

    public void UnSelect()
    {
        selectImage.SetActive(false);
    }

    public void OffImages()
    {
        foreach (var image in images)
        {
            image.SetActive(false);
            image.transform.parent.gameObject.SetActive(false);
        }
        crntImage = -1;
    }

    public void SelectElement()
    {
        selectImage.SetActive(true);
    }

    public void SetSelectedImg(int image)
    {
        
        OffImages();
        crntImage = image;
        images[image].transform.parent.gameObject.SetActive(true);
        images[image].SetActive(true);
    }
}
