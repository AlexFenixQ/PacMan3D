using UnityEngine;

public static class SaveSystem
{
    const string HS_KEY = "HIGH_SCORE";

    // Сохраняем только если счёт больше текущего рекорда
    public static void SaveHighScore(int score)
    {
        int hs = LoadHighScore();
        if (score > hs)
        {
            PlayerPrefs.SetInt(HS_KEY, score);
            PlayerPrefs.Save();
        }
    }

    // Загружаем сохранённый рекорд, если нет — вернётся 0
    public static int LoadHighScore() => PlayerPrefs.GetInt(HS_KEY, 0);
}