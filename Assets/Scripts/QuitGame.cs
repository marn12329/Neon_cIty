using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void ExitGame()
    {
        // ใช้สำหรับตอน Build เป็น .exe หรือเกมจริง
        Application.Quit();

        // ใช้ดูผลใน Editor (จะไม่ปิดจริง แต่เห็นใน Console)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
