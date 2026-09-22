using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopUI;  // อ้างอิงหน้าร้านค้า
    public GameObject pressFText; // อ้างอิง UI "กด F"

    private bool isPlayerNearby = false;

    void Start()
    {
        pressFText.SetActive(false); // ซ่อนปุ่ม "กด F"
        //shopUI.SetActive(false); // ซ่อนร้านค้า
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            OpenShop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            pressFText.SetActive(true); // แสดง "กด F"
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            pressFText.SetActive(false); // ซ่อน "กด F"
            CloseShop();
        }
    }

    void OpenShop()
    {
        shopUI.SetActive(true);
    }

    void CloseShop()
    {
        shopUI.SetActive(false);
    }
}
