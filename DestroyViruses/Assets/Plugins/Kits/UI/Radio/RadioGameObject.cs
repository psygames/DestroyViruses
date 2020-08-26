using UnityEngine;

public class RadioGameObject : RadioItem
{
    [SerializeField]
    private GameObject[] gameObjects = new GameObject[2];

    public override void Radio(int index)
    {
        base.Radio(index);
        if (gameObjects == null)
            return;

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (!gameObjects[i])
                continue;
            if (i != index && gameObjects[i].activeSelf && (gameObjects?[index] != gameObjects[i]))
                gameObjects[i].SetActive(false);
            else if (i == index && !gameObjects[i].activeSelf)
                gameObjects[i].SetActive(true);
        }
    }

    public override int GetOptionCount()
    {
        if (gameObjects == null)
            return 0;
        return gameObjects.Length;
    }
}
