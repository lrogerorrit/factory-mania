using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultsHandler : MonoBehaviour
{
    static public ResultsHandler instance;

    [SerializeField] private GameObject results;
    [SerializeField] private Text xpResults;
    [SerializeField] private Text itemsResults;
    [SerializeField] private Image slider;
    [SerializeField] private Vector2 minSliderPos;
    [SerializeField] private Vector2 maxSliderPos;
    [SerializeField] private List<GameObject> players;

    private ItemDirectory itemDirectory;

    bool isVisible = false;
    bool loading = false;
    float sliderVal = 0.0f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        itemDirectory = ItemDirectory.instance;
        isVisible = false;
        results.SetActive(false);
    }
    
    bool isInSlider(Vector3 pos)
    {
        return pos.x > minSliderPos.x && pos.x < maxSliderPos.x && pos.z > minSliderPos.y && pos.z < maxSliderPos.y;
    }

    void updateSlider()
    {
        slider.fillAmount = Mathf.Clamp(sliderVal,0.0f,1.0f);
        if (sliderVal == 1.0f)
        {
            loading = true;   
            SceneManager.LoadScene("Menu");
        }
    }

    public void showResults(int xp, Dictionary<int, int> items)
    {
        foreach (GameObject player in players)
        {
            player.transform.Find("Cube").localPosition = new Vector3(0, 50, 0);
        }

        xpResults.text = string.Format("<b>XP:</b> {0}", xp.ToString());
        foreach (KeyValuePair<int, int> item in items)
            itemsResults.text += string.Format("{0} ({1})\n", itemDirectory.getObjectName(item.Key), item.Value.ToString());
        results.SetActive(true);
        isVisible = true;
        sliderVal = 0.0f;
        updateSlider();
        
    }

    public void hideResults()
    {
        isVisible = false;
        results.SetActive(false);
        sliderVal = 0.0f;
        updateSlider();
    }

    void Update()
    {
        if (isVisible)
        {
            if (loading) return;
            updateSlider();
            foreach (GameObject player in players)
            {
                if (isInSlider(player.transform.position))
                {
                    sliderVal += Time.deltaTime;
                    return;
                }
            }
            sliderVal -= Time.deltaTime;
            sliderVal = Mathf.Clamp(sliderVal, 0.0f, 1.0f);

        }
    }

}
