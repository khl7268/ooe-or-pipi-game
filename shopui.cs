using UnityEngine;
using UnityEngine.UI;

public class Shopui : MonoBehaviour
{
    [Header("UI References")]
    public Text moneyText;
    public Text unitCountText;
    public Button pippiBuyButton;
    public Button bibiBuyButton;
    public Button startBattleButton;
    
    [Header("Shop s")]
    public Canvas shopCanvas;
    public Image backgroundImage;
    
    void Start()
    {
        // 배경색을 흰색으로 설정
        if (backgroundImage != null)
            backgroundImage.color = Color.white;
        
        pippiBuyButton.onClick.AddListener(BuyPippi);
        bibiBuyButton.onClick.AddListener(BuyBibi);
        startBattleButton.onClick.AddListener(StartBattle);
        
        UpdateUI();
    }
    
    void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.Shopping)
            UpdateUI();
    }
    
    void BuyPippi()
    {
        if (GameManager.instance.TryBuyPippiUnit())
        {
            Debug.Log("삐삐 부대 구매!");
            SpawnPippiUnit();
            UpdateUI();
        }
        else
        {
            Debug.Log("돈이 부족하거나 유닛 수가 초과되었습니다!");
        }
    }
    
    void BuyBibi()
    {
        if (GameManager.instance.TryBuyBibiUnit())
        {
            Debug.Log("비비 부대 구매!");
            SpawnBibiUnit();
            UpdateUI();
        }
        else
        {
            Debug.Log("돈이 부족하거나 유닛 수가 초과되었습니다!");
        }
    }
    
    void SpawnPippiUnit()
    {
        // 아래쪽 화면에 배치 (플레이어팀)
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 0, -10f + Random.Range(-2f, 2f));
        GameObject unit = Resources.Load<GameObject>("Units/Pippi");
        
        if (unit != null)
        {
            GameObject newUnit = Instantiate(unit, spawnPos, Quaternion.identity);
            newUnit.tag = "Player";
            PippiController controller = newUnit.GetComponent<PippiController>();
            if (controller != null)
                controller.isPlayerTeam = true;
        }
    }
    
    void SpawnBibiUnit()
    {
        // 아래쪽 화면에 배치 (플레이어팀)
        Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 0, -10f + Random.Range(-2f, 2f));
        GameObject unit = Resources.Load<GameObject>("Units/Bibi");
        
        if (unit != null)
        {
            GameObject newUnit = Instantiate(unit, spawnPos, Quaternion.identity);
            newUnit.tag = "Player";
            BibiController controller = newUnit.GetComponent<BibiController>();
            if (controller != null)
                controller.isPlayerTeam = true;
        }
    }
    
    void StartBattle()
    {
        Debug.Log("전투 시작!");
        GameManager.instance.StartBattle();
        
        // 적 부대 소환 (비비팀 - 위쪽)
        SpawnEnemyUnits();
        
        // 쇼핑 UI 숨기기
        if (shopCanvas != null)
            shopCanvas.gameObject.SetActive(false);
    }
    
    void SpawnEnemyUnits()
    {
        // 적 부대 10명 정도 소환 (위쪽 화면)
        int enemyCount = Random.Range(5, 10);
        
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), 0, 10f + Random.Range(-2f, 2f));
            GameObject unit = Resources.Load<GameObject>("Units/Bibi");
            
            if (unit != null)
            {
                GameObject newUnit = Instantiate(unit, spawnPos, Quaternion.identity);
                newUnit.tag = "Enemy";
                BibiController controller = newUnit.GetComponent<BibiController>();
                if (controller != null)
                {
                    controller.isPlayerTeam = false;
                    controller.targetTeamIsPlayer = true;
                }
            }
        }
    }
    
    void UpdateUI()
    {
        moneyText.text = "돈: " + GameManager.instance.money.ToString("F0");
        unitCountText.text = "유닛: " + GameManager.instance.currentUnitCount + "/" + GameManager.instance.maxUnits;
        
        pippiBuyButton.interactable = GameManager.instance.money >= GameManager.instance.pippiBuyCost 
            && GameManager.instance.currentUnitCount < GameManager.instance.maxUnits;
        
        bibiBuyButton.interactable = GameManager.instance.money >= GameManager.instance.bibiBuyCost 
            && GameManager.instance.currentUnitCount < GameManager.instance.maxUnits;
    }
}
