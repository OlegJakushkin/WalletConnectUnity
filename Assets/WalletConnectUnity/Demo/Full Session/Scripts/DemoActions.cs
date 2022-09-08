using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Unity;
using WalletConnectUnity.Demo.Scripts;

public class DemoActions : WalletConnectActions
{
    public Text resultText;
    public Text accountText;

    private int count;

    // Start is called before the first frame update
    void OnEnable()
    {
        WalletConnect.ActiveSession.OnSessionDisconnect += ActiveSessionOnDisconnect;
    }

    private void ActiveSessionOnDisconnect(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
        foreach (var platformToggle in transform.parent.GetComponentsInChildren<PlatformToggle>(true))
        {
            platformToggle.MakeActive();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (WalletConnect.ActiveSession.Accounts == null)
            return;
        
        accountText.text = "\nConnected to Chain " + WalletConnect.ActiveSession.ChainId + ":\n" + WalletConnect.ActiveSession.Accounts[0];
    }
    
    public async void OnClickSwitchChain(){
        List<string> list = new List<string>();
            list.Add("https://rpc.ankr.com/eth_rinkeby");
            list.Add("https://rinkeby.infura.io/v3/9aa3d95b3bc440fa88ea12eaa4456161");
        var chainData = new EthChainData(){
                chainId = "0x4",
                chainName = "Rinkeby",
                rpcUrls = list.ToArray(),
                nativeCurrency = new NativeCurrency(){
                    name = "ETH",
                    symbol = "ETH",
                    decimals = 18
                }
            };
            var results = await WalletAddEthChain(chainData);
    }

    public async void OnClickPersonalSign()
    {
        var results = await PersonalSign("This is a test!");

        resultText.text = results;
        resultText.gameObject.SetActive(true);
    }
    
    public async void OnClickSendTransaction()
    {
        var address = WalletConnect.ActiveSession.Accounts[0];
        var transaction = new TransactionData()
        {
            data = "0x",
            from = address,
            to = address,
            gas = "21000",
            value = "0",
            chainId = 4,
        };

        var results = await SendTransaction(transaction);

        resultText.text = results;
        resultText.gameObject.SetActive(true);
    }
    
    public async void OnClickSignTransaction()
    {
        var address = WalletConnect.ActiveSession.Accounts[0];
        var transaction = new TransactionData()
        {
            data = "0x",
            from = address,
            to = address,
            gas = "21000",
            value = "0",
            chainId = 4,
            nonce = "0",
            gasPrice = "50000000000"
        };

        var results = await SignTransaction(transaction);

        resultText.text = results;
        resultText.gameObject.SetActive(true);
    }
    
    public async void OnClickSignTypedData()
    {
        var address = WalletConnect.ActiveSession.Accounts[0];

        var results = await SignTypedData(DemoSignTypedData.ExampleData, DemoSignTypedData.Eip712Domain);

        resultText.text = results;
        resultText.gameObject.SetActive(true);
    }

    public async void OnClickDisconnectAndConnect()
    {
        CloseSession(true);
    }
}
