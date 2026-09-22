using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public void SelectCharacter(string characterName)
    {
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        PlayerPrefs.Save(); // บันทึกข้อมูล
        Debug.Log("เลือกตัวละคร: " + characterName);
        
        // โหลดฉากเกม
        SceneManager.LoadScene("Map1_Main"); 
    }
}
