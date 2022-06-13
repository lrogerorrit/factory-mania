using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{

    public Image slider;

    public List<GameObject> players;

    public Vector2 minPos;
    public Vector2 maxPos;

    public float sliderVal = 0.0f;
    bool loaded = false;
    // Start is called before the first frame update


    void updateSlider()
    {
        slider.fillAmount = Mathf.Clamp(sliderVal, 0.0f, 1.0f);
        if (sliderVal == 1.0f)
        {
            loaded = true;
            SceneManager.LoadScene("DemoLevel");
        }
    }

    bool isInSlider(Vector3 pos)
    {
        return pos.x > minPos.x && pos.x < maxPos.x && pos.z > minPos.y && pos.z < maxPos.y;
    }

    // Update is called once per frame
    void Update()
    {

        updateSlider();
        if (loaded) return;
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
