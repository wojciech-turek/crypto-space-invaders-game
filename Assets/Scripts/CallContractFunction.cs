using System;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CallContractFunction : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nativeBalance;

    [SerializeField]
    TextMeshProUGUI creditsBalance;

    [SerializeField]
    TextMeshProUGUI connectedWallet;

    [SerializeField]
    TextMeshProUGUI creditsToBuy;

    [SerializeField]
    Button startButton;

    [SerializeField]
    GameObject TxOverlay;

    [SerializeField]
    TextMeshProUGUI txPendingText;

    //set chain
    string chain = "polygon";

    // set network
    string network = "mainnet";

    private string isApprovedForAll = "false";

    int creditsTokenId = 0;

    // smart contract address
    private string
        contractAddress = "0x7Fe20316b9BD46cF9Df085AF95cb5f6dF36A5c9b";

    [HideInInspector]
    public float availableCredits = 0;

    // set contract ABI
    private readonly string
        contractABI =
            "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"GOLD\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"burn\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"burnBatch\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"burnCredit\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"}],\"name\":\"buyCredit\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"creditPrice\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"exists\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"mintBatch\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"string\",\"name\":\"newuri\",\"type\":\"string\"}],\"name\":\"setURI\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withdrawAccumulated\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    private void Start()
    {
        GetNativeBalance();
        GetCredits1155();
    }

    private string TruncateAddress(string value)
    {
        return value.Substring(0, 3) +
        "..." +
        value.Substring(value.Length - 3);
    }

    public async void GetNativeBalance()
    {
        string account = PlayerPrefs.GetString("Account");

        connectedWallet.text = "Wallet " + TruncateAddress(account);
        string balance = await EVM.BalanceOf(chain, network, account);
        float wei = float.Parse(balance);
        float eth = wei / 1000000000000000000;
        Debug.Log("Balance: " + eth);
        string parsedBalance = Convert.ToDecimal(eth).ToString("0.00 MATIC");
        nativeBalance.text = "Balance: " + parsedBalance;
    }

    public async Task<BigInteger> GetCreditPrice()
    {
        string method = "creditPrice";
        try
        {
            string response =
                await EVM
                    .Call(chain,
                    network,
                    contractAddress,
                    contractABI,
                    method,
                    "[]");
            BigInteger weiPrice = BigInteger.Parse(response);
            return weiPrice;
        }
        catch (Exception e)
        {
            Debug.LogException (e);
            return 0;
        }
    }

    public async void BuyCredits()
    {
        string account = PlayerPrefs.GetString("Account");
        BigInteger weiPrice = await GetCreditPrice();
        string method = "buyCredit";
        BigInteger weiMultiplied =
            weiPrice * BigInteger.Parse(creditsToBuy.text);

        string args =
            JsonConvert
                .SerializeObject(new object[] {
                    account,
                    creditsTokenId,
                    int.Parse(creditsToBuy.text)
                });

        string gasPrice = await EVM.GasPrice(chain, network);
        try
        {
            string response =
                await Web3GL
                    .SendContract(method,
                    contractABI,
                    contractAddress,
                    args,
                    weiMultiplied.ToString(),
                    "",
                    gasPrice);

            bool txSuccess = await AwaitTransactionSuccessful(response);
            if (txSuccess)
            {
                await Task.Delay(1000);
                GetCredits1155();
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public async void GetCredits1155()
    {
        string account = PlayerPrefs.GetString("Account");
        BigInteger result =
            await ERC1155
                .BalanceOf(chain, network, contractAddress, account, "0");
        Debug.Log("ERC1155 " + result);
        float wei = float.Parse(result.ToString());
        float eth = wei / 1000000000000000000;
        string parsedBalance = Convert.ToDecimal(eth).ToString("0.00");
        creditsBalance.text = "Credits: " + parsedBalance;

        if (float.Parse(parsedBalance) >= 1)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public async Task<bool> CheckIsApproved()
    {
        string account = PlayerPrefs.GetString("Account");
        try
        {
            isApprovedForAll =
                await EVM
                    .Call(chain,
                    network,
                    contractAddress,
                    contractABI,
                    "isApprovedForAll",
                    "[\"" + account + "\",\"" + contractAddress + "\"]");
            Debug.Log("Approved result: " + isApprovedForAll);
            if (isApprovedForAll == "false")
            {
                return false;
            }
            else if (isApprovedForAll == "true")
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
            return false;
        }
        return false;
    }

    public async Task<string> ApproveAll()
    {
        string response =
            await ERC1155
                .SetApprovalForAll(ERC1155.BroadcastMethod.WebGL,
                chain,
                network,
                contractAddress,
                contractAddress,
                true);
        return response;
    }

    public async Task<bool> AwaitTransactionSuccessful(string txHash)
    {
        TxOverlay.SetActive(true);
        txPendingText.text = "Transaction pending...";
        Debug.Log("Awaiting transaction: " + txHash);
        string txConfirmed = await EVM.TxStatus(chain, network, txHash);
        if (txConfirmed == "success")
        {
            Debug.Log("Transaction successful");
            txPendingText.text = "Transaction successful!";
            await Task.Delay(1000);
            TxOverlay.SetActive(false);
            return true;
        }

        if (txConfirmed == "fail")
        {
            Debug.Log("Transaction failed");
            txPendingText.text = "Transaction failed!";
            await Task.Delay(1000);
            TxOverlay.SetActive(false);
            return false;
        }
        else
        {
            Debug.Log("Transaction pending");
            await Task.Delay(2000);

            // wait for a second and try again
            return await AwaitTransactionSuccessful(txHash);
        }
    }

    public async void BurnCredit()
    {
        bool isApproved = await CheckIsApproved();

        if (!isApproved)
        {
            string result = await ApproveAll();
            bool txSuccess = await AwaitTransactionSuccessful(result);
            if (!txSuccess)
            {
                return;
            }
        }

        // send StartGame web request
        try
        {
            await WebRequests.StartGame();
        }
        catch
        {
            Debug.Log("StartGame failed");
            return;
        }

        string method = "burnCredit";

        string args = JsonConvert.SerializeObject(new object[] { "0" });

        string gasPrice = await EVM.GasPrice(chain, network);
        try
        {
            string response =
                await Web3GL
                    .SendContract(method,
                    contractABI,
                    contractAddress,
                    args,
                    "0",
                    "",
                    gasPrice);
            bool txSuccess = await AwaitTransactionSuccessful(response);
            if (txSuccess)
            {
                SceneManager.LoadScene("Game");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}
