using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisibleSwitch : MonoBehaviour
{
    public string ValueToWatch = "";
    private bool RunOnce = false;

    private Text TheText;
    private Image TheImage;
    private Button TheButton;
    private MeshRenderer TheMeshRenderer;

    private LevelTracker Tracker;

    void Start()
    {
        Tracker = FindObjectOfType<LevelTracker>();

        TheText = GetComponent<Text>();
        TheImage = GetComponent<Image>();
        TheButton = GetComponent<Button>();
        TheMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        bool visible = (bool)Tracker.GetType().GetField(ValueToWatch).GetValue(Tracker);

        if (visible)
        {
            if (!RunOnce) return;
            if (TheText != null) TheText.enabled = true;
            else if (TheImage != null)
            {
                if (TheButton != null)
                {
                    TheImage.enabled = true;
                    TheButton.enabled = true;
                }
                else TheImage.enabled = true;
            }
            else if (TheMeshRenderer != null) TheMeshRenderer.enabled = true;
            RunOnce = false;
        }
        else
        {
            if (RunOnce) return;
            if (TheText != null) TheText.enabled = false;
            else if (TheImage != null)
            {
                if (TheButton != null)
                {
                    TheImage.enabled = false;
                    TheButton.enabled = false;
                }
                else TheImage.enabled = false;
            }
            else if (TheMeshRenderer != null) TheMeshRenderer.enabled = false;
            RunOnce = true;
        }
    }
}
