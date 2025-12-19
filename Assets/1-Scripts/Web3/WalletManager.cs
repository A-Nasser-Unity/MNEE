using UnityEngine;
using UnityEngine.UI;
using Thirdweb.Unity;

public class WalletManager : MonoBehaviour
{
    public Button connectButton;
    public TMPro.TextMeshProUGUI walletAddressText;

    private void Start()
    {
        connectButton.onClick.AddListener(ConnectWallet);
    }

    private async void ConnectWallet()
    {
        try
        {
            var walletOptions = new WalletOptions(
                provider: WalletProvider.ReownWallet,
                chainId: 1 // Ethereum mainnet
            );

            var wallet = await ThirdwebManager.Instance.ConnectWallet(walletOptions);
            var address = await wallet.GetAddress();

            walletAddressText.text = "Connected: " + address.Substring(0, 6) + "..." + address.Substring(address.Length - 4);
            Debug.Log("Wallet connected: " + address);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Connection failed: " + e.Message);
        }
    }
}