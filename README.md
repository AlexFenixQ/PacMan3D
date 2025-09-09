# Pacman Clone (Unity)

## 🎮 Описание проекта
Это 3D-клон классической игры Pacman на Unity. Игрок управляет персонажем, собирает пеллеты и бонусы, избегает врагов и стремится набрать как можно больше очков.

**Особенности:**
- 3D-игровое поле с пеллетами и бонусами
- Враги с разными стратегиями движения:
  - **PatrolEnemy** — патрулирует по заданным точкам
  - **DirectChaseEnemy** — преследует игрока
  - **AmbushEnemy** — находится в засаде
- Телепорт по стенам
- Панели UI: старт, игра, пауза, победа, проигрыш
- Система звуков и эффектов (сбор пеллет, powerup, смерть)
- Сохранение highscore через PlayerPrefs

---

## 🏗 Архитектура проекта

**Основные компоненты:**

| Класс | Назначение |
|-------|------------|
| `GameManager` | Управляет состоянием игры (Start, Playing, Pause, Win, Lose), счётом, пеллетами и проверкой условий победы/проигрыша |
| `PlayerController` | Управление движением игрока и столкновения с объектами |
| `Pellet` / `Powerup` | Объекты для сбора игроком, увеличения очков или активации временных эффектов |
| `PatrolEnemy` / `DirectChaseEnemy` / `AmbushEnemy` | Враги с патрульным и преследующим поведением, используют `NavMeshAgent` |
| `UIManager` | Обновление UI: текущий счёт, оставшиеся пеллеты, панели состояния игры |
| `AudioManager` | Централизованное воспроизведение звуков |
| `SaveSystem` | Хранение highscore через `PlayerPrefs` |

**Принцип работы:**
- Игрок взаимодействует с пеллетами, бонусами и врагами через `OnTriggerEnter`.
- `GameManager` обновляет счёт и состояние игры.
- UI обновляется через `UpdateUI()` и `UIManager`.
- Система звуков активируется при действиях игрока.
- Highscore сохраняется при проигрыше или победе.

**Дополнительно:**
- Пауза реализована через `Time.timeScale = 0` и панель `pausePanel`.
- Телепорт стен: игрок появляется строго по центру противоположной стены.
- Враги используют навигацию по `NavMesh`.

---

## ⚙️ Инструкция по сборке и запуску

### Настройка Unity
1. Открыть проект в Unity (рекомендуемая версия 2022.3.x или выше).
2. Проверить сцены в **Build Settings → Scenes In Build**.

### Платформа Android
1. В `File → Build Settings` выбрать платформу **Android**, нажать `Switch Platform`.
2. В `Player Settings`:
   - Настроить `Bundle Identifier` (например, `com.yourname.pacmanclone`)
   - Версию игры
   - Minimum API Level (Android 7.0 / API 24 или выше)
   - Target API Level — `Automatic (highest installed)`

### Сборка APK
1. Настроить Keystore (Publishing Settings), если нужен подписанный APK.
2. Нажать `Build` → выбрать папку для APK (например, `Builds/Android`).
3. Для прямого запуска на устройстве: подключить Android через USB и выбрать `Build And Run`.

### Установка на устройство
- Передать APK на телефон и установить вручную (включить `Install unknown apps`).
- Либо через USB и `Build And Run`.

### Запуск игры
- Стартовая панель показывает highscore и текущий счёт.
- Управлять персонажем, собирать пеллеты и бонусы.
- Избегать врагов. При столкновении с врагом – проигрыш.

---

## 🔊 Звуки и эффекты

- **Сбор пеллет:** `AudioManager.Instance.PlayPellet()`
- **Powerup:** `AudioManager.Instance.PlayPowerup()`
- **Смерть игрока:** `AudioManager.Instance.PlayDeath()`

Вызовы должны происходить в соответствующих местах в `PlayerController`, `GameManager` или скриптах объектов.

---

## 💾 Система сохранений

Highscore сохраняется через `PlayerPrefs`:

```csharp
public static class SaveSystem
{
    const string HS_KEY = "HIGH_SCORE";
    public static void SaveHighScore(int score)
    {
        int hs = LoadHighScore();
        if (score > hs) PlayerPrefs.SetInt(HS_KEY, score);
        PlayerPrefs.Save();
    }
    public static int LoadHighScore() => PlayerPrefs.GetInt(HS_KEY, 0);
}
