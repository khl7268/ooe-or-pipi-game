using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shopui : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text moneyText;
    public TMP_Text unitCountText;
    public Button pippiBuyButton;
    public Button bibiBuyButton;
    public Button startBattleButton;

    [Header("Shop")]
    public Canvas shopCanvas;
    public Image backgroundImage;

    private GameObject selectedUnitPrefab; // 👈 핵심

    void Start()
    {
        // 흰 배경
        if (backgroundImage != null)
            backgroundImage.color = Color.white;

        pippiBuyButton.onClick.AddListener(BuyPippi);
        bibiBuyButton.onClick.AddListener(BuyBibi);
        startBattleButton.onClick.AddListener(StartBattle);

        UpdateUI();
    }

    void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameState.Shopping)
            return;

        UpdateUI();

        // 👇 배치 시스템
        if (Input.GetMouseButtonDown(0) && selectedUnitPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SpawnUnit(hit.point);
                selectedUnitPrefab = null; // 1회 배치
            }
        }
    }

    // 🔥 삐삐 구매
    void BuyPippi()
    {
        if (GameManager.instance.TryBuyPippiUnit())
        {
            selectedUnitPrefab = Resources.Load<GameObject>("Units/Pippi");
            Debug.Log("삐삐 배치 모드");
        }
        else
        {
            Debug.Log("돈 부족 or 유닛 초과");
        }
    }

    // 🔥 비비 구매
    void BuyBibi()
    {
        if (GameManager.instance.TryBuyBibiUnit())
        {
            selectedUnitPrefab = Resources.Load<GameObject>("Units/Bibi");
            Debug.Log("비비 배치 모드");
        }
        else
        {
            Debug.Log("돈 부족 or 유닛 초과");
        }
    }

    // 🔥 실제 생성
    void SpawnUnit(Vector3 pos)
    {
        GameObject unit = Instantiate(selectedUnitPrefab, pos, Quaternion.identity);
        unit.tag = "Player";

        var p = unit.GetComponent<PippiController>();
        if (p != null) p.isPlayerTeam = true;

        var b = unit.GetComponent<BibiController>();
        if (b != null) b.isPlayerTeam = true;
    }

    void StartBattle()
    {
        Debug.Log("전투 시작!");
        GameManager.instance.StartBattle();

        SpawnEnemyUnits();

        if (shopCanvas != null)
            shopCanvas.gameObject.SetActive(false);
    }

    void SpawnEnemyUnits()
    {
        int count = Random.Range(5, 10);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(6f, 12f));

            GameObject prefab;

            if (Random.value > 0.5f)
                prefab = Resources.Load<GameObject>("Units/Pippi");
            else
                prefab = Resources.Load<GameObject>("Units/Bibi");

            GameObject unit = Instantiate(prefab, pos, Quaternion.identity);
            unit.tag = "Enemy";

            var p = unit.GetComponent<PippiController>();
            var b = unit.GetComponent<BibiController>();

            if (p != null) p.isPlayerTeam = false;
            if (b != null) b.isPlayerTeam = false;
        }
    }

    void UpdateUI()
    {
        moneyText.text = "돈: " + GameManager.instance.money.ToString("F0");
        unitCountText.text = "유닛: " + GameManager.instance.currentUnitCount + "/" + GameManager.instance.maxUnits;
    }
}