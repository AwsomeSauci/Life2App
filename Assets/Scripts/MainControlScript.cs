using evm.net.Models;
using Nethereum.ABI;
using Newtonsoft.Json.Linq;
using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using Thirdweb;
using Thirdweb.Examples;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Debugger = Thirdweb.Examples.Debugger;
[Serializable]
public class ItemInfo : IComparable<ItemInfo>
{
    public BigInteger itemID;
    public BigInteger count;
   public int CompareTo(ItemInfo other)
    {
        return other.count.CompareTo(this.count);
    }
    public ItemInfo(BigInteger itemID, BigInteger count)
    {
        this.itemID = itemID;
        this.count = count;
    }
}
public class MainControlScript : MonoBehaviour
{
    private const string AdressTestERC20 = "0x7eda19224a5ff381C851F450e124E60Ad62aFc7A";
    private const string AdressLifeHackatonItems = "0xD29A0bDe6E1f648Af5125411F70462243b4c65bF";
    private const string AdressLifeHackatonPlayers = "0x117dF9cbe08014ACE84c48Cca129aEe2ea0B1f29";
    private const string AdressLifeHackatonBattles = "0x387CA71440614f43ffb1D43f6d5508297aF91dcB";
    private const string EQUIPMENT_TYPE =           "4A78BC8049ECDA3D";
    private const string EQUIPMENT_PART_TYPE =      "62EB12BD8F7E363E";
    private const string LOOTBOX_TYPE =             "D9FAE74DDFF89E31";
    private const string PHONE_SUBTYPE =            "B5816CFC55FB3CB5";
    private const string EARBUDS_SUBTYPE =          "7C1FF20149A3FEF4";
    private const string POWERBANK_SUBTYPE =        "64E53703AB552E2E";
    private const string LAPTOP_SUBTYPE =           "57A3F6CA210E2B2A";
    private const string REGULAR_LOOTBOX_SUBTYPE =  "6E477AD27FB26738";
    private const string PREMIUM_LOOTBOX_SUBTYPE =  "EDCC438796E76959";
    string abiERC20 = "[{  \"inputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"spender\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"allowance\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"needed\",      \"type\": \"uint256\"    }  ],  \"name\": \"ERC20InsufficientAllowance\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"sender\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"balance\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"needed\",      \"type\": \"uint256\"    }  ],  \"name\": \"ERC20InsufficientBalance\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"approver\",      \"type\": \"address\"    }  ],  \"name\": \"ERC20InvalidApprover\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"receiver\",      \"type\": \"address\"    }  ],  \"name\": \"ERC20InvalidReceiver\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"sender\",      \"type\": \"address\"    }  ],  \"name\": \"ERC20InvalidSender\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"spender\",      \"type\": \"address\"    }  ],  \"name\": \"ERC20InvalidSpender\",  \"type\": \"error\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"spender\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    }  ],  \"name\": \"Approval\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"from\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    }  ],  \"name\": \"Transfer\",  \"type\": \"event\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"spender\",      \"type\": \"address\"    }  ],  \"name\": \"allowance\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"spender\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    }  ],  \"name\": \"approve\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"balanceOf\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"decimals\",  \"outputs\": [    {      \"internalType\": \"uint8\",      \"name\": \"\",      \"type\": \"uint8\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"amount\",      \"type\": \"uint256\"    }  ],  \"name\": \"mint\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"name\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"amount\",      \"type\": \"uint256\"    }  ],  \"name\": \"selfMint\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"symbol\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"totalSupply\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    }  ],  \"name\": \"transfer\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"from\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    }  ],  \"name\": \"transferFrom\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"}\r\n]";
    string abiItems = "[{  \"inputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"target\",      \"type\": \"address\"    }  ],  \"name\": \"AddressEmptyCode\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"AddressInsufficientBalance\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"sender\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"balance\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"needed\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"tokenId\",      \"type\": \"uint256\"    }  ],  \"name\": \"ERC1155InsufficientBalance\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"approver\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1155InvalidApprover\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"idsLength\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"valuesLength\",      \"type\": \"uint256\"    }  ],  \"name\": \"ERC1155InvalidArrayLength\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1155InvalidOperator\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"receiver\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1155InvalidReceiver\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"sender\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1155InvalidSender\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1155MissingApprovalForAll\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1967InvalidImplementation\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"ERC1967NonPayable\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"FailedInnerCall\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidInitialization\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotInitializing\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableInvalidOwner\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableUnauthorizedAccount\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"token\",      \"type\": \"address\"    }  ],  \"name\": \"SafeERC20FailedOperation\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"UUPSUnauthorizedCallContext\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"slot\",      \"type\": \"bytes32\"    }  ],  \"name\": \"UUPSUnsupportedProxiableUUID\",  \"type\": \"error\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"bool\",      \"name\": \"approved\",      \"type\": \"bool\"    }  ],  \"name\": \"ApprovalForAll\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"uint64\",      \"name\": \"version\",      \"type\": \"uint64\"    }  ],  \"name\": \"Initialized\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"previousOwner\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnershipTransferred\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"from\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256[]\",      \"name\": \"ids\",      \"type\": \"uint256[]\"    },    {      \"indexed\": false,      \"internalType\": \"uint256[]\",      \"name\": \"values\",      \"type\": \"uint256[]\"    }  ],  \"name\": \"TransferBatch\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"from\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"id\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    }  ],  \"name\": \"TransferSingle\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"string\",      \"name\": \"value\",      \"type\": \"string\"    },    {      \"indexed\": true,      \"internalType\": \"uint256\",      \"name\": \"id\",      \"type\": \"uint256\"    }  ],  \"name\": \"URI\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"Upgraded\",  \"type\": \"event\"},{  \"inputs\": [],  \"name\": \"EARBUDS_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"EQUIPMENT_PART_TYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"EQUIPMENT_TYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"LAPTOP_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"LOOTBOX_TYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"MAX_EQUIPMENT_LEVEL\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"NUMBER_OF_ITEMS_TO_MERGE\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"NUMBER_OF_PARTS_TO_MERGE\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"PHONE_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"POWERBANK_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"PREMIUM_LOOTBOX_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"REGULAR_LOOTBOX_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"UPGRADE_INTERFACE_VERSION\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"id\",      \"type\": \"uint256\"    }  ],  \"name\": \"balanceOf\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address[]\",      \"name\": \"accounts\",      \"type\": \"address[]\"    },    {      \"internalType\": \"uint256[]\",      \"name\": \"ids\",      \"type\": \"uint256[]\"    }  ],  \"name\": \"balanceOfBatch\",  \"outputs\": [    {      \"internalType\": \"uint256[]\",      \"name\": \"\",      \"type\": \"uint256[]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"subType\",      \"type\": \"uint64\"    }  ],  \"name\": \"buyPremiumItem\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"buyPremiumLootbox\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"buyRegularLootbox\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"lootbox\",      \"type\": \"uint256\"    }  ],  \"name\": \"claimLootboxReward\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"type_\",      \"type\": \"uint64\"    },    {      \"internalType\": \"uint64\",      \"name\": \"subType\",      \"type\": \"uint64\"    },    {      \"internalType\": \"uint64\",      \"name\": \"level\",      \"type\": \"uint64\"    },    {      \"internalType\": \"uint256\",      \"name\": \"amount\",      \"type\": \"uint256\"    }  ],  \"name\": \"freeMintSelfItems\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getEquipmentCoolness\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getEquipmentLevel\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getEquipmentRewardIncrease\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getItemExtra\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getItemId\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getItemSubtype\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getItemType\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"lootbox\",      \"type\": \"uint256\"    }  ],  \"name\": \"getOpenedLootboxRandom\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerOwnedItems\",  \"outputs\": [    {      \"internalType\": \"uint256[]\",      \"name\": \"\",      \"type\": \"uint256[]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"getPowerbankCapacity\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"amount\",      \"type\": \"uint256\"    }  ],  \"name\": \"giveRegularLootboxes\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"string\",      \"name\": \"uri\",      \"type\": \"string\"    },    {      \"internalType\": \"address\",      \"name\": \"regularToken_\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"premiumToken_\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"regularLootboxPrice_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"premiumLootboxPrice_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"address\",      \"name\": \"paymentReceiver_\",      \"type\": \"address\"    }  ],  \"name\": \"initialize\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    }  ],  \"name\": \"isApprovedForAll\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isEarbuds\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isEquipment\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isEquipmentPart\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isLaptop\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isLootbox\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"lootbox\",      \"type\": \"uint256\"    }  ],  \"name\": \"isOpenedLootboxExpired\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isPhone\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isPowerbank\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isPremiumLootbox\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"item\",      \"type\": \"uint256\"    }  ],  \"name\": \"isRegularLootbox\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"lootboxOpenedBlocks\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"equipment\",      \"type\": \"uint256\"    }  ],  \"name\": \"mergeEquipment\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"part\",      \"type\": \"uint256\"    }  ],  \"name\": \"mergeEquipmentParts\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"nonces\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"lootbox\",      \"type\": \"uint256\"    }  ],  \"name\": \"openLootbox\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"owner\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"paymentReceiver\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"playersContract\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"premiumLootboxPrice\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"premiumToken\",  \"outputs\": [    {      \"internalType\": \"contract IERC20\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"proxiableUUID\",  \"outputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"\",      \"type\": \"bytes32\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"regularLootboxPrice\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"regularToken\",  \"outputs\": [    {      \"internalType\": \"contract IERC20\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"renounceOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"from\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256[]\",      \"name\": \"ids\",      \"type\": \"uint256[]\"    },    {      \"internalType\": \"uint256[]\",      \"name\": \"values\",      \"type\": \"uint256[]\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"safeBatchTransferFrom\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"from\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"to\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"id\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"value\",      \"type\": \"uint256\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"safeTransferFrom\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"operator\",      \"type\": \"address\"    },    {      \"internalType\": \"bool\",      \"name\": \"approved\",      \"type\": \"bool\"    }  ],  \"name\": \"setApprovalForAll\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"playersContract_\",      \"type\": \"address\"    }  ],  \"name\": \"setPlayersContract\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"newPrice\",      \"type\": \"uint256\"    }  ],  \"name\": \"setPremiumLootboxPrice\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"newPrice\",      \"type\": \"uint256\"    }  ],  \"name\": \"setRegularLootboxPrice\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"string\",      \"name\": \"newURI\",      \"type\": \"string\"    }  ],  \"name\": \"setURI\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"bytes4\",      \"name\": \"interfaceId\",      \"type\": \"bytes4\"    }  ],  \"name\": \"supportsInterface\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"transferOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newImplementation\",      \"type\": \"address\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"upgradeToAndCall\",  \"outputs\": [],  \"stateMutability\": \"payable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"uri\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"}\r\n]";
    string abiPlayer = "[{  \"inputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"target\",      \"type\": \"address\"    }  ],  \"name\": \"AddressEmptyCode\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1967InvalidImplementation\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"ERC1967NonPayable\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"EmptyName\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"FailedInnerCall\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidEarbudsTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidInitialization\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidLaptopTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidNftItemsAddress\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidPhoneTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidPowerbankTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotAnAuthorizedOperator\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotEnoughEnergy\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotInitializing\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotTheOwnerOfTheToken\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableInvalidOwner\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableUnauthorizedAccount\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"PlayerAlreadyRegistered\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"UUPSUnauthorizedCallContext\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"slot\",      \"type\": \"bytes32\"    }  ],  \"name\": \"UUPSUnsupportedProxiableUUID\",  \"type\": \"error\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"address\",      \"name\": \"authorizedOperator\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"bool\",      \"name\": \"authorize\",      \"type\": \"bool\"    }  ],  \"name\": \"AuthorizedOperatorSet\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"uint64\",      \"name\": \"version\",      \"type\": \"uint64\"    }  ],  \"name\": \"Initialized\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"address\",      \"name\": \"nftItems\",      \"type\": \"address\"    }  ],  \"name\": \"NftItemsSet\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"previousOwner\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnershipTransferred\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    }  ],  \"name\": \"PlayerLeagueUpdate\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"phoneTokenId\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"earbudsTokenId\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"powerbankTokenId\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"laptopTokenId\",      \"type\": \"uint256\"    }  ],  \"name\": \"PlayerObjectsUpdate\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"rating\",      \"type\": \"uint256\"    }  ],  \"name\": \"PlayerRatingUpdate\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"Upgraded\",  \"type\": \"event\"},{  \"inputs\": [],  \"name\": \"EARBUDS_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"LAPTOP_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"MAX_LEAGUE\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"PHONE_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"POWERBANK_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"UPGRADE_INTERFACE_VERSION\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"authorizedOperators\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"baseEnergy\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"consumeEnergy\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"by\",      \"type\": \"uint256\"    }  ],  \"name\": \"decreasePlayerRating\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"energyConsumed\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"freeRestoreEnergy\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"index\",      \"type\": \"uint256\"    }  ],  \"name\": \"getLeaguePlayerByIndex\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    }  ],  \"name\": \"getLeaguePlayers\",  \"outputs\": [    {      \"internalType\": \"address[]\",      \"name\": \"\",      \"type\": \"address[]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    }  ],  \"name\": \"getLeaguePlayersCount\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"getNextDayTimestamp\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerCoolness\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerLeague\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerSelectedObjects\",  \"outputs\": [    {      \"internalType\": \"uint256[4]\",      \"name\": \"\",      \"type\": \"uint256[4]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getRemainingEnergy\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getTotalEnergy\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"hasReceivedReward\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"by\",      \"type\": \"uint256\"    }  ],  \"name\": \"increasePlayerRating\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"nftItemsAddress\",      \"type\": \"address\"    }  ],  \"name\": \"initialize\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"isPlayerRegistered\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"lastEnergyConsumedDay\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"leagues\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"name\",      \"type\": \"string\"    },    {      \"internalType\": \"uint256\",      \"name\": \"minRating\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"maxRating\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"rewardLootBoxAmount\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"nftItems\",  \"outputs\": [    {      \"internalType\": \"contract LifeHackatonItems\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"owner\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"playerInfo\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"name\",      \"type\": \"string\"    },    {      \"internalType\": \"uint256\",      \"name\": \"currentLeague\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"currentRating\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"proxiableUUID\",  \"outputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"\",      \"type\": \"bytes32\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"string\",      \"name\": \"name\",      \"type\": \"string\"    }  ],  \"name\": \"register\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"renounceOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"authorizedOperator_\",      \"type\": \"address\"    },    {      \"internalType\": \"bool\",      \"name\": \"authorize\",      \"type\": \"bool\"    }  ],  \"name\": \"setAuthorizedOperator\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"nftItemsAddress\",      \"type\": \"address\"    }  ],  \"name\": \"setNftItems\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"transferOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"phoneTokenId\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"earbudsTokenId\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"powerbankTokenId\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"laptopTokenId\",      \"type\": \"uint256\"    }  ],  \"name\": \"updatePlayerSelectedObjects\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newImplementation\",      \"type\": \"address\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"upgradeToAndCall\",  \"outputs\": [],  \"stateMutability\": \"payable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"version\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"}\r\n]";
        string abiBattle = "[{  \"inputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"target\",      \"type\": \"address\"    }  ],  \"name\": \"AddressEmptyCode\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1967InvalidImplementation\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"ERC1967NonPayable\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"FailedInnerCall\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidInitialization\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotInitializing\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableInvalidOwner\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableUnauthorizedAccount\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"UUPSUnauthorizedCallContext\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"slot\",      \"type\": \"bytes32\"    }  ],  \"name\": \"UUPSUnsupportedProxiableUUID\",  \"type\": \"error\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"bool\",      \"name\": \"battleWon\",      \"type\": \"bool\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"ratingChange\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"rewardAmount\",      \"type\": \"uint256\"    }  ],  \"name\": \"BattleCompleted\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"uint64\",      \"name\": \"version\",      \"type\": \"uint64\"    }  ],  \"name\": \"Initialized\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"previousOwner\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnershipTransferred\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"Upgraded\",  \"type\": \"event\"},{  \"inputs\": [],  \"name\": \"PLAYER_ACTIONS_COUNT\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"UPGRADE_INTERFACE_VERSION\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"battles\",  \"outputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleStatus\",      \"name\": \"status\",      \"type\": \"uint8\"    },    {      \"internalType\": \"enum LifeHackatonBattles.BattleDifficulty\",      \"name\": \"difficulty\",      \"type\": \"uint8\"    },    {      \"internalType\": \"uint256\",      \"name\": \"randomSourceBlockNumber\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"playerCoolness\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"enemyCoolness\",      \"type\": \"uint256\"    },    {      \"internalType\": \"address\",      \"name\": \"enemy\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"bigRatingChange\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"completeBattle\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getBattleRandom\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getEnemyActionQueue\",  \"outputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleAction[]\",      \"name\": \"\",      \"type\": \"uint8[]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getEnemyForBattle\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    },    {      \"internalType\": \"enum LifeHackatonBattles.BattleDifficulty\",      \"name\": \"\",      \"type\": \"uint8\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"random\",      \"type\": \"uint256\"    }  ],  \"name\": \"getRandomEnemyAction\",  \"outputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleAction\",      \"name\": \"\",      \"type\": \"uint8\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"enemyLeague\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"enemyLeaguePlayerCount\",      \"type\": \"uint256\"    },    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getRandomEnemySafe\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"playersContract_\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"rewardToken_\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"rewardAmount_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"smallRatingChange_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"normalRatingChange_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"bigRatingChange_\",      \"type\": \"uint256\"    }  ],  \"name\": \"initialize\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"initiateBattle\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"isBattleExpired\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"isBattleWon\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"normalRatingChange\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"owner\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"playersContract\",  \"outputs\": [    {      \"internalType\": \"contract LifeHackatonPlayers\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"proxiableUUID\",  \"outputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"\",      \"type\": \"bytes32\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"renounceOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"rewardAmount\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"rewardToken\",  \"outputs\": [    {      \"internalType\": \"contract IERC20Mintable\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"smallRatingChange\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleAction[3]\",      \"name\": \"actions\",      \"type\": \"uint8[3]\"    }  ],  \"name\": \"startBattle\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"transferOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newImplementation\",      \"type\": \"address\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"upgradeToAndCall\",  \"outputs\": [],  \"stateMutability\": \"payable\",  \"type\": \"function\"}\r\n]";


    #region Fields
   [SerializeField] private TextMeshProUGUI TokenBalanceTxt;
    [SerializeField] private TextMeshProUGUI BalanceTxt;
    [SerializeField] private TextMeshProUGUI CPTxt;
    [SerializeField] private TextMeshProUGUI BPTxt;
    [SerializeField] private TMP_InputField playerNameTxt;
    [SerializeField] private GameObject NameSelectScreen;
    [SerializeField] private GameObject MenuScreen;
    public Debugger debugger;
    [Serializable]
    public class NetworkIcon
    {
        public string chain;
        public Sprite sprite;
    }
    
    [Serializable]
    public class WalletIcon
    {
        public WalletProvider provider;
        public Sprite sprite;
    }

    [System.Serializable]
    public class WalletProviderUIDictionary : SerializableDictionaryBase<WalletProvider, GameObject> { }

    private List<ItemInfo> _userItems=new List<ItemInfo>();
    [Header("Enabled Wallet Providers. Press Play to see changes.")]
    public List<WalletProvider> enabledWalletProviders = new List<WalletProvider> { WalletProvider.LocalWallet, WalletProvider.EmbeddedWallet, WalletProvider.SmartWallet };

    [Header("Events")]
    public UnityEvent onStart;
    public UnityEvent<WalletConnection> onConnectionRequested;
    public UnityEvent<string> onConnected;
    public UnityEvent<Exception> onConnectionError;
    public UnityEvent onDisconnected;
    public UnityEvent onSwitchNetwork;
    public UnityEvent onItemBuyed;
    public UnityEvent onInventoryLoaded;

    [Header("UI")]
    public WalletProviderUIDictionary walletProviderUI;
    public List<Image> walletImages;
    public List<TMP_Text> addressTexts;
    public List<TMP_Text> balanceTexts;

    public List<NetworkIcon> networkIcons;
    public List<WalletIcon> walletIcons;

    private string _address;
    private ChainData _currentChainData;
    private BoxOpenController _openController;
    #endregion
    private void Start()
    {
        _openController = FindFirstObjectByType<BoxOpenController>();
        _address = null;
        _currentChainData = ThirdwebManager.Instance.supportedChains.Find(x => x.identifier == ThirdwebManager.Instance.activeChain);
        foreach (var walletProvider in walletProviderUI)
            walletProvider.Value.SetActive(enabledWalletProviders.Contains(walletProvider.Key));
        onStart.Invoke();
        
    }

    #region Connection
    public void ConnectExternal(string walletProviderStr)
    {
        var wc = new WalletConnection(provider: Enum.Parse<WalletProvider>(walletProviderStr), chainId: BigInteger.Parse(_currentChainData.chainId));
        Connect(wc);
    }

    public async void Disconnect()
    {
        try
        {
            _address = null;
            await ThirdwebManager.Instance.SDK.wallet.Disconnect();
            onDisconnected.Invoke();
        }
        catch (System.Exception e)
        {
            ThirdwebDebug.LogError($"Failed to disconnect: {e}");
        }
    }

    private async void Connect(WalletConnection wc)
    {
        ThirdwebDebug.Log($"Connecting to {wc.provider}...");
       
        onConnectionRequested.Invoke(wc);
        try
        {
            
            _address = await ThirdwebManager.Instance.SDK.wallet.Connect(wc);
            PlayerPrefs.SetString("Adress", _address);
        }
        catch (Exception e)
        {
            _address = null;
            ThirdwebDebug.LogError($"Failed to connect: {e}");
            onConnectionError.Invoke(e);
            return;
        }

        PostConnect(wc);
    }

    private async void PostConnect(WalletConnection wc = null)
    {
        ThirdwebDebug.Log($"Connected to {_address}");

        var addy = _address.ShortenAddress();
        foreach (var addressText in addressTexts)
            addressText.text = addy;

        var bal = await ThirdwebManager.Instance.SDK.wallet.GetBalance();
        var balStr = $"{bal.value.ToEth()} {bal.symbol}";
        foreach (var balanceText in balanceTexts)
            balanceText.text = balStr;

        if (wc != null)
        {
            var currentWalletIcon = walletIcons.Find(x => x.provider == wc.provider)?.sprite ?? walletIcons[0].sprite;
            foreach (var walletImage in walletImages)
                walletImage.sprite = currentWalletIcon;
        }
               
        onConnected.Invoke(_address);
        LoadInventory();
        LoadShopCosts();
        LoadCPBP();
        LoadBalance();
        CheckAllowence();
        GetInventory();
        BoxesChecker();
    }
    BigInteger[] InventoryItems;
    public BigInteger[] GetInventoryItems()
    {
        return InventoryItems;
    }
    public async void LoadInventory()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        InventoryItems = await contract.Read<BigInteger[]>("getPlayerSelectedObjects", _address);
        
    }
    public async void ResetEnergy()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        await contract.Write("freeRestoreEnergy");
    }
    public async void LoadShopCosts()
    {
        /*Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        var cost = await contract.Read<BigInteger>("regularLootboxPrice");
        FindFirstObjectByType<ShopController>().SetPriceSB((float) (cost/ 1000000000000000000));*/
    }
    public async void LoadCPBP()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        var isreg = await contract.Read<bool>("isPlayerRegistered", _address);
        if (isreg)
        {
            MenuScreen.SetActive(true);
            NameSelectScreen.SetActive(false);
            var res = await contract.Read<System.Object[]>("playerInfo", _address);
            PlayerPrefs.SetString("PlayerName", res[0].ToString());
            BPTxt.text = res[2].ToString();
           
            var resT = await contract.Read<BigInteger>("getPlayerCoolness", _address);
            CPTxt.text = resT.ToString();
           
            
        }
        else
        {
            NameSelectScreen.SetActive(true);
            MenuScreen.SetActive(false);
        }
    }
    public async void RegisterPlayer()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        var res = await contract.Write("register", playerNameTxt.text);
        LoadCPBP();
    }
    public async void LoadBalance()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressTestERC20, abiERC20);
        var balance = await contract.Read<BigInteger>("balanceOf", _address);
        balance /= 1000000000000000000;
        TokenBalanceTxt.text = balance.ToString("0.");
        var bal = await ThirdwebManager.Instance.SDK.wallet.GetBalance();
        float balan = float.Parse(bal.value)/1000000000000000000;
        BalanceTxt.text = balan.ToString("0.000");
    }
    public async void CheckAllowence()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressTestERC20, abiERC20);
        var balance = await contract.Read<BigInteger>("balanceOf",_address);
        var allowence = await contract.Read<BigInteger>("allowance", _address,AdressLifeHackatonItems);
        if (allowence < BigInteger.Pow(10, 18))
        {
            await contract.Write("approve", AdressLifeHackatonItems, BigInteger.Pow(10, 18) * 1000000);
        }
    }
    #endregion
    
    #region WriteMethods

    public async void EquipItems()
    {
        List<ItemInfo> itemsH = new List<ItemInfo>();
        List<ItemInfo> itemsS = new List<ItemInfo>();
        List<ItemInfo> itemsL = new List<ItemInfo>();
        List<ItemInfo> itemsP = new List<ItemInfo>();
        string[] hexs = ConvertToHexArray(_rawUserItems);        
        for(int i = 0;i<hexs.Length;i++)
        {
            string type = hexs[i].Substring(0, 16);
            string subtype = hexs[i].Substring(16, 16);
            string lvl = hexs[i].Substring(48, 16);
            int level = Int32.Parse(lvl);
            if(type == EQUIPMENT_TYPE)
            {
                switch (subtype)
                {
                    case PHONE_SUBTYPE:
                        {
                            itemsS.Add(new ItemInfo(_rawUserItems[i], level));
                            break;
                        }
                    case EARBUDS_SUBTYPE:
                        {
                            itemsH.Add(new ItemInfo(_rawUserItems[i], level));
                            break;
                        }
                    case POWERBANK_SUBTYPE:
                        {
                            itemsP.Add(new ItemInfo(_rawUserItems[i], level));
                            break;
                        }
                    case LAPTOP_SUBTYPE:
                        {
                            itemsL.Add(new ItemInfo(_rawUserItems[i], level));
                            break;
                        }
                }
            }
            
        }
        BigInteger smart = 0;
        BigInteger headph = 0;
        BigInteger laptop = 0;
        BigInteger power = 0;
        if (itemsP.Count > 0)
        {
            itemsP.Sort();
            power = itemsP[0].itemID;
        }
        if (itemsH.Count > 0)
        {
            itemsH.Sort();
            headph = itemsH[0].itemID;
        }
        if (itemsS.Count > 0)
        {
            itemsS.Sort();
            smart = itemsS[0].itemID;
        }
        if (itemsL.Count > 0)
        {
            itemsL.Sort();
            laptop = itemsL[0].itemID;
        }
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        var res = await contract.Write("updatePlayerSelectedObjects", smart,headph, power, laptop);
        GetInventory();
    }
    public async void UnEquipItems()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        var res = await contract.Write("updatePlayerSelectedObjects", 0, 0, 0, 0);
        GetInventory();
    }
    public async void SelfMint()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressTestERC20, abiERC20);
        BigInteger count = BigInteger.Pow(10,18)* 10000;
        await contract.Write("selfMint", count);
        CheckAllowence();
    }

    public async void BuyRegularLootBox()
    {
        string adress = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        var result = await contract.Write("buyRegularLootbox");
        GetInventory();
        onItemBuyed.Invoke();
    }
    private async void BoxesChecker()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        string id = "98595196311672320881376512671230966117268107951326164903185855800251651194880";
        BigInteger.TryParse(id, out var lootbox);
        var result = await contract.Read<BigInteger>("lootboxOpenedBlocks", _address, lootbox);
        if(result>0)
        {
            _openController.ShowNotClaimedBox();
        }
    }
    public async void ShowBoxOpenScreen()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        string id = "98595196311672320881376512671230966117268107951326164903185855800251651194880";
        BigInteger.TryParse(id, out var lootbox);
        var result = await contract.Read<BigInteger>("lootboxOpenedBlocks", _address, lootbox);
        if (result > 0)
        {
            _openController.ShowNotClaimedBox();
        }
        else
        {
            _openController.ShowClosedBox();
        }
    }
    public async void OpenRegularLootBox()
    {                
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        string id = "98595196311672320881376512671230966117268107951326164903185855800251651194880";
        BigInteger.TryParse(id, out var lootbox);
        var result = await contract.Write("openLootbox", lootbox);
        Debug.Log(result);

        _openController.ShowNotClaimedBox();
        
    }

    public async void ClaimLootBoxPrize()
    {
        string adress = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        string id = "98595196311672320881376512671230966117268107951326164903185855800251651194880";
        BigInteger.TryParse(id, out var lootbox);
        var result = await contract.Write("claimLootboxReward", lootbox);
        Debug.Log("LOOTBOXREWARD" + result);
        JObject responseJson = JObject.Parse(result.receipt.ToString());
        Debug.Log("JSON " + responseJson.ToString());

        string dataValue = responseJson["logs"][0]["data"].ToString();
        dataValue = dataValue.Substring(2, 64);
        Debug.Log("DATAVALUE = " + dataValue);
        BigInteger resultItem = BigInteger.Parse("0" + dataValue, System.Globalization.NumberStyles.AllowHexSpecifier);
        BigInteger cnt = 0;
        ItemInfo foundItem = _userItems.Find(item => item.itemID == resultItem);
        if(foundItem != null)
        {
            cnt = foundItem.count;
            cnt++;
        }
        else { cnt = 1; }
        _openController.ShowClaimedBox(resultItem, cnt);
        GetInventory();
    }

    #endregion

    #region ReadMethods

    public async void GetInventory()
    {
        LoadInventory();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        var result = await contract.Read<BigInteger[]>("getPlayerOwnedItems", _address);
        string[] arrays = ConvertToHexArray(result);
        _rawUserItems = result;
        List<string> adr=new List<string>();
        for(int i = 0; i < result.Length; i++)
        {
            adr.Add(_address);
        }
        System.Object[] objects = { adr.ToArray(), result };
        var res = await contract.Read<BigInteger[]>("balanceOfBatch", objects);              
        _userItems.Clear();
        for(int i = 0;i < result.Length; i++)
        {
            _userItems.Add(new ItemInfo( result[i], res[i]));
        }
        
        onInventoryLoaded.Invoke();
        
    }
    BigInteger[] _rawUserItems;
    #endregion

    #region Tools

    string[] ConvertToHexArray(BigInteger[] bigIntegerArray)
    {
        string[] hexArray = new string[bigIntegerArray.Length];

        for (int i = 0; i < bigIntegerArray.Length; i++)
        {
            hexArray[i] = bigIntegerArray[i].ToString("X");
        }

        return hexArray;
    }

    public List<ItemInfo> GetItems()
    {
        return _userItems;
    }
    public async void SelfMinItems()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        await contract.Write("freeMintSelfItems", 5366246215194040893, 13078854623890717877, 1, 1);
        await contract.Write("freeMintSelfItems", 5366246215194040893, 8944133472325467892, 1, 1);
        await contract.Write("freeMintSelfItems", 5366246215194040893, 7270277662358449710, 1, 1);
        await contract.Write("freeMintSelfItems", 5366246215194040893, 6315162450501970730, 1, 1);
    }
    public async void BuySmartPhone()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        await contract.Write("buyPremiumItem", 13078854623890717877);
        GetInventory();
        onItemBuyed.Invoke();
    }
    public async void BuyLaptop()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        await contract.Write("buyPremiumItem", 6315162450501970730);
        GetInventory();
        onItemBuyed.Invoke();
    }
    public async void BuyEarbuds()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        await contract.Write("buyPremiumItem", 8944133472325467892);
        GetInventory();
        onItemBuyed.Invoke();
    }
    public async void BuyPowerBank()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        await contract.Write("buyPremiumItem", 7270277662358449710);
        GetInventory();
        onItemBuyed.Invoke();
    }
    public async void GetItemsID()
    {
        string adress = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        Contract cont = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonItems, abiItems);
        var result = await cont.Read<BigInteger[]>("getPlayerOwnedItems", adress);
        List<BigInteger> types = new List<BigInteger>
        {
            await cont.Read<BigInteger>("EQUIPMENT_TYPE"),
            await cont.Read<BigInteger>("EQUIPMENT_PART_TYPE"),
            await cont.Read<BigInteger>("LOOTBOX_TYPE"),
            await cont.Read<BigInteger>("PHONE_SUBTYPE"),
            await cont.Read<BigInteger>("EARBUDS_SUBTYPE"),
            await cont.Read<BigInteger>("POWERBANK_SUBTYPE"),
            await cont.Read<BigInteger>("LAPTOP_SUBTYPE"),
            await cont.Read<BigInteger>("REGULAR_LOOTBOX_SUBTYPE"),
            await cont.Read<BigInteger>("PREMIUM_LOOTBOX_SUBTYPE")
        };
        string[] strings = ConvertToHexArray(types.ToArray());
        Debug.Log("EQUIPMENT_TYPE          " + strings[0] + "\r\n" +
                  "EQUIPMENT_PART_TYPE     " + strings[1] + "\r\n" +
                  "LOOTBOX_TYPE            " + strings[2] + "\r\n" +
                  "PHONE_SUBTYPE           " + strings[3] + "\r\n" +
                  "EARBUDS_SUBTYPE         " + strings[4] + "\r\n" +
                  "POWERBANK_SUBTYPE       " + strings[5] + "\r\n" +
                  "LAPTOP_SUBTYPE          " + strings[6] + "\r\n" +
                  "REGULAR_LOOTBOX_SUBTYPE " + strings[7] + "\r\n" +
                  "PREMIUM_LOOTBOX_SUBTYPE " + strings[8]);
    }
    #endregion
}
