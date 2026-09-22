using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs; // เก็บ Prefabs ทั้งหมด เช่น Archer, Mage
    public string selectedCharacterKey = "SelectedCharacter"; // key สำหรับ PlayerPrefs

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt(selectedCharacterKey, 0); // Default = ตัวแรก
        Transform spawnPoint = GameObject.Find("SpawnPoint").transform;

        if (characterPrefabs.Length > selectedIndex && spawnPoint != null)
        {
            GameObject character = Instantiate(characterPrefabs[selectedIndex], spawnPoint.position, Quaternion.identity);
            character.tag = "Player"; // ตั้ง tag ให้กล้องตาม
        }
        else
        {
            Debug.LogError("SpawnPoint หรือ CharacterPrefab ไม่ถูกต้อง");
        }
    }
}
