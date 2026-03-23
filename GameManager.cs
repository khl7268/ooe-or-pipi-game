using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    
    public float money = 500f;
    public float pippiBuyCost = 100f;
    public float bibiBuyCost = 150f;
    
    [Header("Unit Limits")]
    public int maxUnits = 30;
    public int currentUnitCount = 0;
    
    
    public enum GameState { Shopping, Battle, GameOver }
    public GameState currentState = GameState.Shopping;
    
    public bool isPlayerTeam = true; // 플레이어 팀 표시
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public bool TryBuyPippiUnit()
{
    if (money >= pippiBuyCost && currentUnitCount < maxUnits)
    {
        money -= pippiBuyCost;
        return true; // ❗ count 증가 제거
    }
    return false;
}

public bool TryBuyBibiUnit()
{
    if (money >= bibiBuyCost && currentUnitCount < maxUnits)
    {
        money -= bibiBuyCost;
        return true; // ❗ count 증가 제거
    }
    return false;
}
    
    public void StartBattle()
    {
        currentState = GameState.Battle;
        Time.timeScale = 1f;
    }
    
    public void EndGame(bool playerWon)
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        
        if (playerWon)
            Debug.Log("플레이어 승리!");
        else
            Debug.Log("플레이어 패배...");
    }
}
