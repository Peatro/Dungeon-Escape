using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    private Player _player;
    private int _currentSelectedItem;
    private int _currentItemCost;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _player = other.GetComponent<Player>();
            shopPanel.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                UIManager.Instance.OpenShop(_player.GemsCount());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            shopPanel.SetActive(false);
        }
    }

    public void BuyItem()
    {
        if (_player.GemsCount() >= _currentItemCost)
        {
            if (_currentSelectedItem == 2)
            {
                GameManager.Instance.HasKeyToCastle = true;
            }

            _player.Purchase(_currentItemCost);
            Debug.Log("Purchased");
        }
        else
        {
            shopPanel.SetActive(false);
            Debug.Log("You don't have enough gems.");
        }
    }

    public void SelectItem(int item)
    {
        //0 = flame sword
        //1 = boots of flight
        //2 = key to castle
        Debug.Log("SelectItem()" + item);
        _currentSelectedItem = item;

        switch (item)
        {
            case 0: //flame sword
                UIManager.Instance.UpdateShopSelection(185);
                _currentItemCost = 200;
                break;
            case 1: //boots of flight
                UIManager.Instance.UpdateShopSelection(80);
                _currentItemCost = 400;
                break;
            case 2: //key to castle
                UIManager.Instance.UpdateShopSelection(-25);
                _currentItemCost = 100;
                break;
        }
    }
}
