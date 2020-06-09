using Unity.Entities;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    private bool gameOverInvoked;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
        set
        {
            if(_instance)
            {
                Destroy(_instance.gameObject);
                _instance = value;

            }
        }
    }

    public bool GameOver { get; set; }

    private void Update()
    {
        if(GameOver && !gameOverInvoked)
        {
            gameOverInvoked = true;
            UnloadMainMenuLoadOperation();
        }
    }

    private void UnloadMainMenuLoadOperation()
    {
        ApplicationManager.Instance.UnloadScene("Game", LoadNewGameScene);
    }

    private void LoadNewGameScene()
    {
        ApplicationManager.Instance.LoadScene("MainMenu", MenuSceneLoaded);
    }

    private void MenuSceneLoaded()
    {
        ApplicationManager.Instance.SwitchLoader(false);
    }

    public void TriggerGameOver()
    {
        ApplicationManager.Instance.SwitchLoader(true, DisposeECS);
    }

    public void DisposeECS()
    {
        foreach(var world in World.All)
        {
            var entities = world.EntityManager.GetAllEntities(Unity.Collections.Allocator.Temp);
            foreach(var entity in entities)
            {
                world.EntityManager.DestroyEntity(entity);
            }
        }
        GameOver = true;
    }
}
