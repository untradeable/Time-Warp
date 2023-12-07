using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager_Menu : MonoBehaviour
{
    //public delegate IEnumerator WaveTimerEvent();
    //public event WaveTimerEvent WaveTimer;

    // ====================================================

    [Header("Buttons")]
    public TMP_Text   pauseButtonTMP;
    public TMP_Text   speedFeedback;    
    public TMP_Text   speedButtonTMP;
    public TMP_Text   volumeSliderTMP;

    public List<TabStats> tabsStats = new List<TabStats>();

    [Header("Timer")]
    public GameObject timerObject;
    public TMP_Text   timerTMP;

    private float lateTime;
    private float timerCount;
    private bool callTimer;

    [Header("Tower Tools Bar")]
    public GameObject   toolsMenu; 

    public TMP_Text     towerSellPriceTMP;
    public TMP_Text     towerUpgradePriceTMP;

    [Space]

    public Image[]      toolsButtonsHighlight;
    public TMP_Text[]   towerStatusTMP;

    [Header("Game Alerts")]
    public GameObject   damageAlert;
    public GameObject   blueprintAlert;
    public GameObject[] alerts;

    [Header("Game Screens")]
    public GameObject winScreen;
    public GameObject loseScreen;

    public GameObject specialWaveEffect;
    public GameObject specialWavePanel;

    public Image    specialIcon;
    public TMP_Text specialEffectDesc;

    public Sprite[] specialSprites;
    public string[] specialStrings;

    // ====================================================

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) && GetComponent<Manager_Level>().GetFaseID < 4)
        {
            PauseButton();
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            GameSpeedButton();
        }
    }

    private void FixedUpdate()
    {
        TimerPopUP();
    }
    
    // ====================================================
    // Special Waves

    public void ChangeSpecialIcon(int id)
    {
        if(specialWaveEffect != null)
        {
            specialWaveEffect.SetActive(true);
        }

        specialWavePanel.SetActive(true);
        
        specialIcon.sprite = specialSprites[id];
        specialEffectDesc.text = specialStrings[id];
    }

    public void DisableSpecialWaveEffect()
    {
        if(specialWaveEffect == true)
        {
            specialWaveEffect.SetActive(false);
        }

        if(specialWavePanel == true)
        {
            specialWavePanel.SetActive(false);
        }
    }

    // ====================================================
    // Game Buttons

    public void StopMusic(bool _self)
    {
        AudioListener.pause = _self;
    }

    public void PauseButton()
    {
        if(Time.timeScale == 0f)
        {
            pauseButtonTMP.text = "II";

            ChangeTime(lateTime);
            AudioListener.pause = false;

            return;
        }

        lateTime = Time.timeScale;
        pauseButtonTMP.text = ">";

        ChangeTime(0f);
        AudioListener.pause = true;
    }

    public void GameSpeedButton()
    {
        AudioListener.pause = false;

        pauseButtonTMP.text = "II";

        if(Time.timeScale <= 1f)
        {
            Time.timeScale = 3f;

            speedFeedback.text = "Velocidade Atual 2x";
            speedButtonTMP.text = "2X";     
            
            return;
        }
        else if(Time.timeScale != 8f)
        {
            Time.timeScale = 8f;

            speedFeedback.text = "Velocidade Atual 3x";
            speedButtonTMP.text = "3X";

            return;
        }

        Time.timeScale = 1f;
        
        speedFeedback.text = "Velocidade Atual 1x";
        speedButtonTMP.text = "1X";
    }

    public void ChangeTime(float _time)
    {
        Time.timeScale = _time;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    // ==================================================== 
    // Utility

    public void ActiveObject(GameObject _object)
    {
        _object.SetActive(true);
    }

    public void ActiveBySelf(GameObject _object)
    {
        _object.SetActive(!_object.activeSelf);
    }

    public void DesactiveObject(GameObject _object)
    {
        _object.SetActive(false);
    }

    public void VolumeSlider(float volume)
    {
        volumeSliderTMP.text = (volume * 100).ToString("0") + "%";
        AudioListener.volume = volume;
    }

    public void HighlightOn(Image _image)
    {
        _image.color = new Color32(0,146,255, 255);
    }

    public void HighlightOff(Image _image)
    {
        _image.color = new Color32(255,255,255, 255);
    }

    public void DisableHighlights()
    {
        foreach(Image img in toolsButtonsHighlight)
        {
            img.color = new Color32(255, 255, 255, 255);
        }
    }

    // ==================================================== 
    // Alerts

    private void TimerPopUP()
    {
        if(!callTimer)
        {
            return;
        }

        timerTMP.text = string.Format("Próxima wave em: {00:00:00} segundos", timerCount);
        timerCount -= Time.deltaTime;
    }

    public IEnumerator OnWaveEvent(float time)
    {
        ActiveObject(timerObject);
        callTimer = true;
        timerCount = time;

        yield return new WaitForSeconds(time / 1f);

        DesactiveObject(timerObject);
        callTimer = false;
    }

    public IEnumerator OnDamageEvent()
    {
        ActiveObject(damageAlert);

        yield return new WaitForSeconds(1f);

        DesactiveObject(damageAlert);
    }

    public void OnBlueprintOn()
    {
        blueprintAlert.SetActive(true);
    }

    public void OnBlueprintOff()
    {
        blueprintAlert.SetActive(false);
    }

    public IEnumerator CallAlert(string alertText, float time)
    {
        GameObject usingAlert = null;
        
        foreach(GameObject alert in alerts)
        {
            if(alert.activeSelf == false)
            {
                usingAlert = alert;

                usingAlert.SetActive(true);
                usingAlert.transform.GetChild(1).GetComponent<TMP_Text>().text = alertText;

                break;
            }
        }

        yield return new WaitForSeconds(time);
        
        try
        {
            usingAlert.SetActive(false);
        }
        catch{}
    }

    public void LockedUpgradeAlert()
    {
        StartCoroutine(CallAlert("Desbloqueie essa torre na loja de melhorias!", 5f));
    }

    // ==================================================== 
    // Screens and Scene

    public void WinScreen()
    {
        ChangeTime(0f);

        AudioListener.pause = true;
        winScreen.SetActive(true);
    }

    public void LoseScreen()
    {
        ChangeTime(0f);

        AudioListener.pause = true;
        loseScreen.SetActive(true);
    }

    public void SetFullScreen(bool self)
    {
        Screen.fullScreen = self;
    }

    public void OnOpenToolsMenu(Component_Tower _tower)
    {
        Manager_Market market = GetComponent<Manager_Market>();
        
        toolsMenu.SetActive(true);

        int sellPrice = market.SellPrice();

        towerSellPriceTMP.text    = $"Vender por {sellPrice - market.SellPrice() * 20 / 100}";

        for(int i = 0; i < towerStatusTMP.Length; i++)
        {
            towerStatusTMP[i].text = _tower.GetTowerStat(i);    
        }

        try
        {
            towerUpgradePriceTMP.text = $"Melhorar por {market.UpgradePrice()}";
        }
        catch
        {
            towerUpgradePriceTMP.text = $"Nível Maximo!";
        }
    }

    public void OnPointerEnterTowerTab(int id)
    {
        Manager_Market market = GetComponent<Manager_Market>();

        Component_Tower tower = market.GetTowerData(id).prefabs[0].GetComponent<Component_Tower>();

        for(int i = 0; i < 3; i++)
        {
            tabsStats[id].stat[i].text = tower.GetTowerStat(i);    
        }
    }

    public void OnCloseToolseMenu()
    {
        toolsMenu.SetActive(false);
    }

    public void SceneLoading(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SceneLoadingASync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}

[System.Serializable]
public class TabStats
{
    public TMP_Text[] stat;
}