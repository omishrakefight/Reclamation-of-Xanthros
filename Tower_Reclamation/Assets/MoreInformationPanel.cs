using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MoreInformationPanel : MonoBehaviour {

    [SerializeField] Text informationText;
    [SerializeField] RawImage picture;
    [SerializeField] Image pictureX;
    [SerializeField] Text buttonName;
    List<Texture> images = new List<Texture>();
    List<string> texts = new List<string>();
    float outsideTimeScale;
    float pauseTime = 0f;
    private bool createAnother = false;

	// Use this for initialization
	void Start () {
        outsideTimeScale = Time.timeScale;
        Time.timeScale = pauseTime;
    }
	
	// Update is called once per frame
	void Update () {

	}

    /// <summary>
    /// Tries to close the panel.  If the list of images and texts still have items, instead swap to the next and remove them from the list.  Otherwise close.
    /// </summary>
    public void CloseThePanel()
    {
        // revert it to whatever timescale it was before the popup.
        Time.timeScale = outsideTimeScale;

        if (createAnother)
        {
            picture.texture = images[0];
            //pictureX.sprite = images[0];
            informationText.text = texts[0];

            images.RemoveAt(0);
            texts.RemoveAt(0);

            if (images.Count > 0 && texts.Count > 0)
            {
                createAnother = true;
                buttonName.text = "NEXT";
                //TODO set button to continue instead of close here.
            } else
            {
                createAnother = false;
                buttonName.text = "CLOSE";
            }
            return;
        }

        Destroy(this.gameObject);
    }

    public MoreInformationPanel() { }
    public MoreInformationPanel(Texture image, string text)
    {
        picture.texture = image;
        informationText.text = text;
    }
    public MoreInformationPanel(List<Texture> image, List<string> text)
    {
        AddAndSetItems(image, text);
    }

    public void DelayedInitialization(List<Texture> image, List<string> text)
    {
        AddAndSetItems(image, text);
    }

    public void AddAndSetItems(List<Texture> image, List<string> text)
    {
        picture.texture = image[0];
        informationText.text = text[0];

        image.RemoveAt(0);
        text.RemoveAt(0);

        images = image;
        texts = text;

        if (image.Count > 0 && text.Count > 0)
        {
            createAnother = true;
            buttonName.text = "NEXT";
        }
    }

}
