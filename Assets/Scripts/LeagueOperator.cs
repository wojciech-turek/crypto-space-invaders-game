using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeagueOperator : MonoBehaviour
{
    //set chain
    string chain = "polygon";

    // set network
    string network = "mainnet";

    // smart contract address
    private string
        contractAddress = "0xEef42cF1d00F5440173F36855D633eC1140cee4c";

    private string sponsorName = "";

    private string sponsorAmount = "";

    [SerializeField]
    GameObject TxOverlay;

    [SerializeField]
    GameObject SponsorOverlay;

    [SerializeField]
    TextMeshProUGUI txPendingText;

    [SerializeField]
    TextMeshProUGUI leagueSponsorText;

    [SerializeField]
    TextMeshProUGUI leagueTimer;

    [SerializeField]
    TextMeshProUGUI leaguePrize;

    [SerializeField]
    Button sponsorButton;

    [SerializeField]
    TextMeshProUGUI stringLengthCounter;

    // get resolve league button
    [SerializeField]
    Button resolveLeagueButton;

    [SerializeField]
    Button showSponsorOverlayButton;

    [HideInInspector]
    public float availableCredits = 0;

    // set contract ABI
    private readonly string
        contractABI =
            "[{\"inputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"ApprovalForAll\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"indexed\":false,\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"TransferBatch\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"TransferSingle\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":false,\"internalType\":\"string\",\"name\":\"value\",\"type\":\"string\"},{\"indexed\":true,\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"URI\",\"type\":\"event\"},{\"inputs\":[],\"name\":\"CREDITS\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address[]\",\"name\":\"accounts\",\"type\":\"address[]\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"}],\"name\":\"balanceOfBatch\",\"outputs\":[{\"internalType\":\"uint256[]\",\"name\":\"\",\"type\":\"uint256[]\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"burn\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"values\",\"type\":\"uint256[]\"}],\"name\":\"burnBatch\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_id\",\"type\":\"uint256\"}],\"name\":\"burnCredit\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"_account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"_id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"_amount\",\"type\":\"uint256\"}],\"name\":\"buyCredit\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"creditPrice\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"exists\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"highestScore\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"highestScorer\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"}],\"name\":\"isApprovedForAll\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"leagueDuration\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"leagueNumber\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"leagueReward\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"leagueSponsor\",\"outputs\":[{\"internalType\":\"bytes32\",\"name\":\"\",\"type\":\"bytes32\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"leagueStart\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"mintBatch\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"internalType\":\"address\",\"name\":\"\",\"type\":\"address\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"renounceOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256[]\",\"name\":\"ids\",\"type\":\"uint256[]\"},{\"internalType\":\"uint256[]\",\"name\":\"amounts\",\"type\":\"uint256[]\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeBatchTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"amount\",\"type\":\"uint256\"},{\"internalType\":\"bytes\",\"name\":\"data\",\"type\":\"bytes\"}],\"name\":\"safeTransferFrom\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"operator\",\"type\":\"address\"},{\"internalType\":\"bool\",\"name\":\"approved\",\"type\":\"bool\"}],\"name\":\"setApprovalForAll\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"newPrice\",\"type\":\"uint256\"}],\"name\":\"setCreditPrice\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_score\",\"type\":\"uint256\"},{\"internalType\":\"address\",\"name\":\"_scorer\",\"type\":\"address\"}],\"name\":\"setHighestScore\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"_duration\",\"type\":\"uint256\"}],\"name\":\"setLeagueDuration\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes32\",\"name\":\"_sponsorName\",\"type\":\"bytes32\"}],\"name\":\"sponsorLeague\",\"outputs\":[],\"stateMutability\":\"payable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"startFirstLeague\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"bytes4\",\"name\":\"interfaceId\",\"type\":\"bytes4\"}],\"name\":\"supportsInterface\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"id\",\"type\":\"uint256\"}],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"uri\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"withdrawAccumulated\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"wrapUpLeagueAndStartNew\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";

    private void Start()
    {
        // Fetch the league information
        GetLeagueSponsor();
        InvokeRepeating("GetLeagueStart", 0f, 1f);
        GetLeagueReward();
    }

    public async void GetLeagueStart()
    {
        string method = "leagueStart";
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

            // end time is 2 days after start time seconds
            int endTime = Int32.Parse(response) + 172800;

            // time now
            int timeNow = (int) DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // convert the difference to hours, minutes and seconds
            int timeLeft = endTime - timeNow;

            int hours = timeLeft / 3600;
            int minutes = (timeLeft % 3600) / 60;
            int seconds = (timeLeft % 3600) % 60;

            if (timeLeft <= 0)
            {
                resolveLeagueButton.interactable = true;
                leagueTimer.text =
                    "League has ended, click below to resolve it and start a new one";
            }
            else
            {
                string hoursLeft = hours < 10 ? "0" + hours : hours.ToString();
                string minutesLeft =
                    minutes < 10 ? "0" + minutes : minutes.ToString();
                string secondsLeft =
                    seconds < 10 ? "0" + seconds : seconds.ToString();

                // display the time remaining
                leagueTimer.text =
                    "League Ends In: " +
                    hoursLeft +
                    ":" +
                    minutesLeft +
                    ":" +
                    secondsLeft;
            }
        }
        catch (Exception e)
        {
            Debug.LogException (e);
        }
    }

    public static byte[] FromHexString(string hexString)
    {
        hexString = hexString.Substring(2);
        byte[] hexChars = new byte[hexString.Length / 2];

        for (int i = 0; i < hexChars.Length; i++)
        {
            hexChars[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return hexChars.ToArray();
    }

    public async void GetLeagueSponsor()
    {
        string method = "leagueSponsor";
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

            byte[] sponsor = FromHexString(response);
            string sponsorText = System.Text.Encoding.UTF8.GetString(sponsor);

            showSponsorOverlayButton.gameObject.SetActive(true);

            if (
                response !=
                "0x0000000000000000000000000000000000000000000000000000000000000000"
            )
            {
                showSponsorOverlayButton.gameObject.SetActive(false);
                leagueSponsorText.text = sponsorText;
            }
            Debug.Log("League Sponsor: " + sponsorText);
        }
        catch (Exception e)
        {
            Debug.LogException (e);
        }
    }

    public void ReadSponsorName(string sponsor)
    {
        sponsorName = sponsor;
        stringLengthCounter.text = sponsor.Length.ToString() + "/20";
        CheckInputs();
    }

    public void ReadSponsorAmount(string amount)
    {
        sponsorAmount = amount.ToString();
        CheckInputs();
    }

    public void CheckInputs()
    {
        if (
            sponsorName != "" &&
            sponsorAmount != "" &&
            Int32.Parse(sponsorAmount) >= 10
        )
        {
            sponsorButton.interactable = true;
        }
        else
        {
            sponsorButton.interactable = false;
        }
    }

    public void ShowSponsorModal()
    {
        SponsorOverlay.SetActive(true);
    }

    public void HideSponsorModal()
    {
        SponsorOverlay.SetActive(false);
    }

    public async void SponsorLeague()
    {
        string method = "sponsorLeague";
        string gasPrice = await EVM.GasPrice(chain, network);
        Debug.Log (sponsorAmount);
        BigInteger weiAmount =
            BigInteger.Parse("1000000000000000000") *
            BigInteger.Parse(sponsorAmount);

        Debug.Log (weiAmount);

        // convert sponsor name to bytes
        byte[] sponsorNameBytes =
            System.Text.Encoding.UTF8.GetBytes(sponsorName);

        // convert bytes to hex string
        string sponsorNameHex =
            BitConverter.ToString(sponsorNameBytes).Replace("-", string.Empty);
        string args = "[\"" + "0x" + sponsorNameHex + "\"]";

        try
        {
            string response =
                await Web3GL
                    .SendContract(method,
                    contractABI,
                    contractAddress,
                    args,
                    weiAmount.ToString(),
                    "",
                    gasPrice);
            bool txSuccess = await AwaitTransactionSuccessful(response);
            if (txSuccess)
            {
                Debug.Log("Transaction successful");
                GetLeagueSponsor();
                GetLeagueReward();
                await Task.Delay(1000);

                // close modal
                HideSponsorModal();
            }
            else
            {
                Debug.Log("Transaction failed");
            }
        }
        catch (Exception e)
        {
            Debug.LogException (e);
        }
    }

    public async void GetLeagueReward()
    {
        string method = "leagueReward";
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
            Debug.Log("leagueReward " + response);
            float wei = float.Parse(response);
            float eth = wei / 1000000000000000000;
            Debug.Log("Balance: " + eth);
            string parsedBalance =
                Convert.ToDecimal(eth).ToString("0.00 MATIC");
            leaguePrize.text = "Current prize: " + parsedBalance;
        }
        catch (Exception e)
        {
            Debug.LogException (e);
        }
    }

    public async void WrapUpLeague()
    {
        string method = "wrapUpLeagueAndStartNew";
        string gasPrice = await EVM.GasPrice(chain, network);
        string args = "[]";
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
                Debug.Log("Transaction successful");
                GetLeagueStart();
                GetLeagueReward();
                GetLeagueSponsor();
            }
            else
            {
                Debug.Log("Transaction failed");
            }
        }
        catch (Exception e)
        {
            Debug.LogException (e);
        }
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
}
