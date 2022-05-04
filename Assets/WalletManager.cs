using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using System.IO;
 using System.Runtime.Serialization.Formatters.Binary;
using NBitcoin;
using Nethereum.Web3;
using Nethereum.Web3.Accounts; 
using Nethereum.Util; 
using Nethereum.Hex.HexConvertors.Extensions; 
using Nethereum.HdWallet;
using Nethereum.KeyStore.Crypto;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.KeyStore.Model;
using Nethereum.KeyStore;
using Nethereum.Hex.HexTypes;
using Nethereum.RLP;
using Nethereum.Util;
using Nethereum.Signer;
using ZXing.QrCode;
using UnityEngine.UI;
using System;
using TMPro;
using ZXing;

public class WalletManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI walletAddress;
    [SerializeField] private TextMeshProUGUI mnemoPhrase;

    [SerializeField] private TextMeshProUGUI amountOfDragons;
    [SerializeField] private TMP_InputField importWalletPassword;
    [SerializeField] private TMP_InputField importWalletMnemonic;

    [SerializeField] private TextMeshProUGUI createdWalletMnemonicPhrase;

    [SerializeField] private RawImage qrCode;

    private Texture2D _encodedTexture;

    private KeyStoreScryptService keyStoreService;
    private ScryptParams scryptParams;

    private Wallet userWallet;

    public async void CreateWallet(TMP_InputField passwordInput) {
        Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
        
        Debug.Log("mnemonic phrase: " + mnemo.ToString());
        string password = passwordInput.text;
        mnemoPhrase.text = mnemo.ToString();
        userWallet = new Wallet(mnemo.ToString(), password);
        var account2 = userWallet.GetAccount(0);
        var web3 = new Web3(account2, Manager.infuraMumbaiEndpointUrl);
        var balance = await web3.Eth.GetBalance.SendRequestAsync(account2.Address);
        Debug.Log("created account: " + account2.Address + " with balance: " + balance);
        walletAddress.text = account2.Address;
        Manager.PlayerAddress = account2.Address;
        Manager.PlayerPK = account2.PrivateKey;
        
        createdWalletMnemonicPhrase.text = mnemo.ToString();
        // save account to keystore
        SaveAccountToKeyStore(passwordInput.text, account2.PrivateKey, account2.Address);
        // TODO shouldn't save here
        PlayerPrefs.SetString("playerPk", account2.PrivateKey);
        PlayerPrefs.SetString("playerAddress", account2.Address);
        PlayerPrefs.SetString("playerWords", mnemo.ToString());

        updateAmountOfDragons();
    }

    public void ImportWallet() {
        var password = importWalletPassword.text;
        var backupSeed = importWalletMnemonic.text;
        userWallet = new Wallet(backupSeed, password);
        var recoveredAccount = userWallet.GetAccount(0);

        Debug.Log("recovered account: " + recoveredAccount.Address);
        walletAddress.text = recoveredAccount.Address;
        Manager.PlayerAddress = recoveredAccount.Address;
        Manager.PlayerPK = recoveredAccount.PrivateKey;
        updateAmountOfDragons();

        // save account to keystore
        SaveAccountToKeyStore(password, recoveredAccount.PrivateKey, recoveredAccount.Address);
        // TODO shouldn't save here
        PlayerPrefs.SetString("playerPk", recoveredAccount.PrivateKey);
        PlayerPrefs.SetString("playerAddress", recoveredAccount.Address);
        PlayerPrefs.SetString("playerWords", backupSeed);
    }

    public void Login(TMP_InputField passwordField) {
        try {
            LoadAccountFromKeyStore(passwordField.text);
            // if no account show panel "create wallet"
        } catch (DecryptionException e) {
            Debug.LogError(e); 
        }
    }

    public void LoadAccountFromKeyStore(string password) {
        string destination = Application.persistentDataPath + "/save.dat";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenRead(destination);
         else
         {
             Debug.LogError("File not found");
             return;
         }
 
         BinaryFormatter bf = new BinaryFormatter();
         KeystoreData data = (KeystoreData) bf.Deserialize(file);
         file.Close();
        var keyStoreEncryptedJson = data.contentAsJson;
         // @"{""crypto"":{""cipher"":""aes-128-ctr"",""ciphertext"":""6b305a969ace3a8862373512bfcd084cca2dfe1ac2b9b23603a87b08ee45dec2"",""cipherparams"":{""iv"":""f90057eb50b5d90f17c3691ffebb9744""},""kdf"":""scrypt"",""mac"":""1b245be69d2995508eb8b0667e9e178dc06ea2776a437def0dca16652b9d0933"",""kdfparams"":{""n"":262144,""r"":1,""p"":8,""dklen"":32,""salt"":""55226bb014afd92ff45a790c08e5db135b788a773cb1918c44fda23415b6c64f""}},""id"":""996b13be-67cc-44a5-b253-bbcc8f02507c"",""address"":""0x1f646F69022b2aA84bBF1Ecfd41E2a7f9eFC568C"",""version"":3}";
        var account = Nethereum.Web3.Accounts.Account.LoadFromKeyStore(keyStoreEncryptedJson, password);
        Debug.Log("loaded account from keystore: " + account.Address);
        Manager.PlayerAddress = account.Address;
        Manager.PlayerPK = account.PrivateKey;
    }

    public void SaveAccountToKeyStore(string password, string privateKey, string address) {
        var account = new Account(privateKey);
        var ecKey = new EthECKey(privateKey);

        var keyStore = keyStoreService.EncryptAndGenerateKeyStore(password, ecKey.GetPrivateKeyAsBytes(), address, scryptParams);
        var json = keyStoreService.SerializeKeyStoreToJson(keyStore);

        Debug.Log("encrypted account: " + json.ToString());
         string destination = Application.persistentDataPath + "/save.dat";
         FileStream file;
 
         if(File.Exists(destination)) file = File.OpenWrite(destination);
         else file = File.Create(destination);
 
         KeystoreData data = new KeystoreData("save", json.ToString());
         BinaryFormatter bf = new BinaryFormatter();
         bf.Serialize(file, data);
         file.Close();
    }

    public void BackupMnemonic() {
        if (userWallet != null) {
            mnemoPhrase.text =string.Join(" ", userWallet.Words);
            Debug.Log("mnemonic phrase: " + string.Join(" ", userWallet.Words));
        } else {
            if (!String.IsNullOrEmpty(Manager.PlayerPK)) {
                // TODO should be stored here
                mnemoPhrase.text = PlayerPrefs.GetString("playerWords");
                Debug.Log("mnemonic phrase: " + mnemoPhrase.text);
            }
            Debug.Log("no account");
        }
    }

    void Start() {
        _encodedTexture = new Texture2D(256,256);
        walletAddress.text = Manager.PlayerAddress;
        Color32[] qrCodeEncoded = Encode(Manager.PlayerAddress, 256,256);
        _encodedTexture.SetPixels32(qrCodeEncoded);
        _encodedTexture.Apply();
        qrCode.texture = _encodedTexture;
        updateAmountOfDragons();
    }

    private Color32[] Encode(string text, int width, int height) {
        BarcodeWriter writer = new BarcodeWriter 
        {
           Format = BarcodeFormat.QR_CODE,
           Options = new QrCodeEncodingOptions{
               Height = height,
               Width = width
           } 
        };
        return writer.Write(text);
    }

    void Awake() {
        keyStoreService = new Nethereum.KeyStore.KeyStoreScryptService();
        scryptParams = new ScryptParams {Dklen = 32, N = 262144, R = 1, P = 8};
    }

    public void updateAmountOfDragons() {
        amountOfDragons.text = Manager.playerAmountOfDragons.ToString();
    }

    public void resetAccount() {
        PlayerPrefs.DeleteAll();   
        Manager.PlayerPK = "";
        Manager.PlayerAddress = "";
        Application.Quit();
    } 

    public void copyTextToClipboard(TextMeshProUGUI UiText) {
        TextEditor te = new TextEditor();
        te.text = UiText.text;
        te.SelectAll();
        te.Copy();
    }

}
