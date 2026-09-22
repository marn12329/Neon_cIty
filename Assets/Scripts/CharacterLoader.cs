using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public GameObject[] characters;

    void Start()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "");
        
        if (!string.IsNullOrEmpty(selectedCharacter))
        {
            SpawnCharacter(selectedCharacter);
        }
        else
        {
            Debug.LogError("ไม่พบตัวละครที่เลือก!");
        }
    }

    void SpawnCharacter(string characterName)
    {
        foreach (GameObject character in characters)
        {
            if (character.name == characterName)
            {
                Instantiate(character, Vector3.zero, Quaternion.identity);
                Debug.Log("สร้างตัวละคร: " + characterName);
                return;
            }
        }
        Debug.LogError("ไม่พบตัวละครที่ชื่อ: " + characterName);
    }
}
