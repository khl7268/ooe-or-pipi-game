using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shopui : MonoBehaviour
{
    public TMP_Text moneyText;
    public TMP_Text unitCountText;

    public Button pippiBuyButton;
    public Button bibiBuyButton;
    public Button startBattleButton;

    public Canvas shopCanvas;
    public Image backgroundImage;

    private GameObject selectedUnitPrefab;

    void Start()
    {
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

        // 🔥 배치
        if (Input.GetMouseButtonDown(0) && selectedUnitPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SpawnUnit(hit.point);
                selectedUnitPrefab = null;
            }
        }
    }

    void BuyPippi()
    {
        if (GameManager.instance.TryBuyPippiUnit())
        {
            selectedUnitPrefab = Resources.Load<GameObject>("Units/Pippi");

            if (selectedUnitPrefab == null)
                Debug.LogError("Pippi 프리팹 못 찾음");
        }
    }

    public void BuyBibi()
    {
        if (GameManager.instance.TryBuyBibiUnit())
        {
            selectedUnitPrefab = Resources.Load<GameObject>("Units/Bibi");

            if (selectedUnitPrefab == null)
                Debug.LogError("Bibi 프리팹 못 찾음");
        }
    }

    public void SpawnUnit(Vector3 pos)
    {
        GameObject unit = Instantiate(selectedUnitPrefab, pos, Quaternion.identity);
        unit.tag = "Player";

        GameManager.instance.currentUnitCount++; // 👈 여기서 증가

        var p = unit.GetComponent<PippiController>();
        if (p != null) p.isPlayerTeam = true;

        var b = unit.GetComponent<BibiController>();
        if (b != null) b.isPlayerTeam = true;
    }

    public void StartBattle()
    {
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

            if (prefab == null)
            {
                Debug.LogError("Enemy 프리팹 못 찾음");
                continue;
            }

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