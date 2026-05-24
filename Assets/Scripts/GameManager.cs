using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panelleri")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;
    }

    // Zafer Kontrol Fonksiyonu
    public void CheckVictory()
    {
        if (gameEnded) return;

        // Haritadaki "Enemy" etiketli tüm objeleri bul
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Eðer hiç düþman kalmadýysa (ve biz hala yaþýyorsak)
        if (enemies.Length <= 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameEnded = true;
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f; // Oyunu durdur
        Debug.Log("Tebrikler! Tüm düþmanlar elendi.");
    }

    public void GameOver()
    {
        if (gameEnded) return;
        gameEnded = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}