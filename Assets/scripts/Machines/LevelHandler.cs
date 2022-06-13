using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler instance;
    public float timeRemaining = 80;

    [SerializeField] private GameObject instructions;
    [SerializeField] private Image instr_slider;

    [SerializeField] private List<Text> timeDisplays;
    [SerializeField] private List<Text> xpDisplays;

    private Dictionary<int, int> handedInItems = new Dictionary<int, int>();

    private SoundHandler soundHandler;
    private OrderHandler orderHandler;
    private ResultsHandler resultsHandler;

    private bool audioFast = false;
    private bool lvlEnded = false;
    [HideInInspector] public bool instructionsVisible = false;
    private float instr_sliderValue = 1.0f;
    private int xp = 0;

    [SerializeField] private float instructionsVisibleTime = 10.0f;

    void Awake()
    {

        instance = this;
    }

    void startLevel()
    {
        
        orderHandler.setIsActive(true);
        orderHandler.enableTimer(true);
        soundHandler.playAudio("musicNormal");
        orderHandler.forceSpawnItem();
        
    }

    void updateInstructions()
    {
        if (instructionsVisible)
        {
            instr_sliderValue -= Time.deltaTime / instructionsVisibleTime ;
            instr_slider.fillAmount = instr_sliderValue;
            if (instr_sliderValue <= 0)
            {
                instructionsVisible = false;
                instructions.SetActive(false);
                startLevel();
            }
        }
    }

    bool checkInstructions()
    {
        if (instructions && instr_slider)
        {
            instructionsVisible = true;
            instr_sliderValue = 1.0f;
            instructions.SetActive(true);
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        orderHandler = OrderHandler.instance;
        soundHandler = SoundHandler.instance;
        resultsHandler = ResultsHandler.instance;
        if (!checkInstructions())
        {
            startLevel();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (instructionsVisible)
        {
            updateInstructions();
        }
        else
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0.0f, timeRemaining);
            updateClocks();
            updateXP();

            if (timeRemaining == 0.0f && !lvlEnded)
                endLevel();

            if (timeRemaining < 60 && !audioFast)
            {
                audioFast = true;
                soundHandler.playAudio("musicFast");
                soundHandler.setAudioPosition("musicFast", soundHandler.getAudioPosition("musicNormal"));
                soundHandler.stopAudio("musicNormal");
            }
        }
    }

    void endLevel()
    {
        lvlEnded = true;
        orderHandler.setIsActive(false);
        orderHandler.enableTimer(false);
        orderHandler.deleteRemainingOrders();
        soundHandler.stopAudio("musicFast");
        resultsHandler.showResults(xp, handedInItems);



    }

    public bool didLevelEnd()
    {
        return lvlEnded;
    }

    void updateClocks()
    {
        int timeRemInt = (int) Mathf.Ceil(timeRemaining);
        string timeString = string.Format("{0}:{1}", Mathf.Floor(timeRemInt / 60).ToString("00"), (timeRemInt % 60).ToString("00"));
        foreach (Text t in timeDisplays)
            t.text = timeString;
    }
    void updateXP()
    {
        foreach (Text t in xpDisplays)
            t.text =string.Format("{0} xp",xp.ToString());
    }

    public void failOrder(int xp)
    {
        this.xp -= xp;
        this.xp = Mathf.Max(0, this.xp);
        soundHandler.playAudio("failOrder");
    }

    public void handInBadOrder()
    {
        Debug.Log("Handing in bad order");
        soundHandler.playAudio("wrong");
    }

    public void handInGoodOrder(int xp,int id)
    {
        Debug.Log("Handing in good order ("+xp+")");
        this.xp += xp;
        soundHandler.playAudio("good");
        if (handedInItems.ContainsKey(id))
            handedInItems[id]++;
        else
            handedInItems.Add(id, 1);

    }
}
