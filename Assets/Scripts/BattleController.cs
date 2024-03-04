using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Thirdweb;
using Thirdweb.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    public UnityEvent OnScreenOpened;
    public UnityEvent OnOpponentFounded;
    public UnityEvent OnResetScreen;
    [SerializeField] private Debugger popupMSg;
    [SerializeField] private BattleSequenceElement firstBattleElement;
    [SerializeField] private BattleSequenceElement secondBattleElement;
    [SerializeField] private BattleSequenceElement thirdBattleElement;
    [SerializeField] private BattleSequenceElement firstSelectElement;
    [SerializeField] private BattleSequenceElement secondSelectElement;
    [SerializeField] private BattleSequenceElement thirdSelectElement;
    [SerializeField] private TextMeshProUGUI opponentName;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI CP;
    [SerializeField] private TextMeshProUGUI BP;
    [SerializeField] private TextMeshProUGUI CPPlayer;
    [SerializeField] private TextMeshProUGUI BPPlayer;
    [SerializeField] private Button AttackBtn;
    [SerializeField] private GameObject WonScreen;
    [SerializeField] private GameObject LooseScreen;
    private BattleSequenceElement selectedSequence;
    private BattleSequenceElement selectedSelector;

    enum BattleStatus { COMPLETED, INITIATED, STARTED }
    enum BattleDifficulty { NORMAL, EASY, HARD }
    enum BattleAction { TOUGH, SMART, DEVIOUS }
    private const string AdressLifeHackatonPlayers = "0x117dF9cbe08014ACE84c48Cca129aEe2ea0B1f29";
    private const string AdressLifeHackatonBattles = "0x387CA71440614f43ffb1D43f6d5508297aF91dcB";
    string abiBattle = "[{  \"inputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"target\",      \"type\": \"address\"    }  ],  \"name\": \"AddressEmptyCode\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1967InvalidImplementation\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"ERC1967NonPayable\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"FailedInnerCall\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidInitialization\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotInitializing\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableInvalidOwner\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableUnauthorizedAccount\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"UUPSUnauthorizedCallContext\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"slot\",      \"type\": \"bytes32\"    }  ],  \"name\": \"UUPSUnsupportedProxiableUUID\",  \"type\": \"error\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"bool\",      \"name\": \"battleWon\",      \"type\": \"bool\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"ratingChange\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"rewardAmount\",      \"type\": \"uint256\"    }  ],  \"name\": \"BattleCompleted\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"uint64\",      \"name\": \"version\",      \"type\": \"uint64\"    }  ],  \"name\": \"Initialized\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"previousOwner\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnershipTransferred\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"Upgraded\",  \"type\": \"event\"},{  \"inputs\": [],  \"name\": \"PLAYER_ACTIONS_COUNT\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"UPGRADE_INTERFACE_VERSION\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"battles\",  \"outputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleStatus\",      \"name\": \"status\",      \"type\": \"uint8\"    },    {      \"internalType\": \"enum LifeHackatonBattles.BattleDifficulty\",      \"name\": \"difficulty\",      \"type\": \"uint8\"    },    {      \"internalType\": \"uint256\",      \"name\": \"randomSourceBlockNumber\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"playerCoolness\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"enemyCoolness\",      \"type\": \"uint256\"    },    {      \"internalType\": \"address\",      \"name\": \"enemy\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"bigRatingChange\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"completeBattle\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getBattleRandom\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getEnemyActionQueue\",  \"outputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleAction[]\",      \"name\": \"\",      \"type\": \"uint8[]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getEnemyForBattle\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    },    {      \"internalType\": \"enum LifeHackatonBattles.BattleDifficulty\",      \"name\": \"\",      \"type\": \"uint8\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"random\",      \"type\": \"uint256\"    }  ],  \"name\": \"getRandomEnemyAction\",  \"outputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleAction\",      \"name\": \"\",      \"type\": \"uint8\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"enemyLeague\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"enemyLeaguePlayerCount\",      \"type\": \"uint256\"    },    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getRandomEnemySafe\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"playersContract_\",      \"type\": \"address\"    },    {      \"internalType\": \"address\",      \"name\": \"rewardToken_\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"rewardAmount_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"smallRatingChange_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"normalRatingChange_\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"bigRatingChange_\",      \"type\": \"uint256\"    }  ],  \"name\": \"initialize\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"initiateBattle\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"isBattleExpired\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"isBattleWon\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"normalRatingChange\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"owner\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"playersContract\",  \"outputs\": [    {      \"internalType\": \"contract LifeHackatonPlayers\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"proxiableUUID\",  \"outputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"\",      \"type\": \"bytes32\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"renounceOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"rewardAmount\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"rewardToken\",  \"outputs\": [    {      \"internalType\": \"contract IERC20Mintable\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"smallRatingChange\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"enum LifeHackatonBattles.BattleAction[3]\",      \"name\": \"actions\",      \"type\": \"uint8[3]\"    }  ],  \"name\": \"startBattle\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"transferOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newImplementation\",      \"type\": \"address\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"upgradeToAndCall\",  \"outputs\": [],  \"stateMutability\": \"payable\",  \"type\": \"function\"}\r\n]";
    
    string abiPlayer = "[{  \"inputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"constructor\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"target\",      \"type\": \"address\"    }  ],  \"name\": \"AddressEmptyCode\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"ERC1967InvalidImplementation\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"ERC1967NonPayable\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"EmptyName\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"FailedInnerCall\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidEarbudsTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidInitialization\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidLaptopTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidNftItemsAddress\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidPhoneTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"InvalidPowerbankTokenType\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotAnAuthorizedOperator\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotEnoughEnergy\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotInitializing\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"NotTheOwnerOfTheToken\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"owner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableInvalidOwner\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"account\",      \"type\": \"address\"    }  ],  \"name\": \"OwnableUnauthorizedAccount\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"PlayerAlreadyRegistered\",  \"type\": \"error\"},{  \"inputs\": [],  \"name\": \"UUPSUnauthorizedCallContext\",  \"type\": \"error\"},{  \"inputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"slot\",      \"type\": \"bytes32\"    }  ],  \"name\": \"UUPSUnsupportedProxiableUUID\",  \"type\": \"error\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"address\",      \"name\": \"authorizedOperator\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"bool\",      \"name\": \"authorize\",      \"type\": \"bool\"    }  ],  \"name\": \"AuthorizedOperatorSet\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"uint64\",      \"name\": \"version\",      \"type\": \"uint64\"    }  ],  \"name\": \"Initialized\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": false,      \"internalType\": \"address\",      \"name\": \"nftItems\",      \"type\": \"address\"    }  ],  \"name\": \"NftItemsSet\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"previousOwner\",      \"type\": \"address\"    },    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"OwnershipTransferred\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    }  ],  \"name\": \"PlayerLeagueUpdate\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"phoneTokenId\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"earbudsTokenId\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"powerbankTokenId\",      \"type\": \"uint256\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"laptopTokenId\",      \"type\": \"uint256\"    }  ],  \"name\": \"PlayerObjectsUpdate\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"indexed\": false,      \"internalType\": \"uint256\",      \"name\": \"rating\",      \"type\": \"uint256\"    }  ],  \"name\": \"PlayerRatingUpdate\",  \"type\": \"event\"},{  \"anonymous\": false,  \"inputs\": [    {      \"indexed\": true,      \"internalType\": \"address\",      \"name\": \"implementation\",      \"type\": \"address\"    }  ],  \"name\": \"Upgraded\",  \"type\": \"event\"},{  \"inputs\": [],  \"name\": \"EARBUDS_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"LAPTOP_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"MAX_LEAGUE\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"PHONE_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"POWERBANK_SUBTYPE\",  \"outputs\": [    {      \"internalType\": \"uint64\",      \"name\": \"\",      \"type\": \"uint64\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"UPGRADE_INTERFACE_VERSION\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"authorizedOperators\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"baseEnergy\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"consumeEnergy\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"by\",      \"type\": \"uint256\"    }  ],  \"name\": \"decreasePlayerRating\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"energyConsumed\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"index\",      \"type\": \"uint256\"    }  ],  \"name\": \"getLeaguePlayerByIndex\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    }  ],  \"name\": \"getLeaguePlayers\",  \"outputs\": [    {      \"internalType\": \"address[]\",      \"name\": \"\",      \"type\": \"address[]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"league\",      \"type\": \"uint256\"    }  ],  \"name\": \"getLeaguePlayersCount\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"getNextDayTimestamp\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerCoolness\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerLeague\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getPlayerSelectedObjects\",  \"outputs\": [    {      \"internalType\": \"uint256[4]\",      \"name\": \"\",      \"type\": \"uint256[4]\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getRemainingEnergy\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"getTotalEnergy\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"hasReceivedReward\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    },    {      \"internalType\": \"uint256\",      \"name\": \"by\",      \"type\": \"uint256\"    }  ],  \"name\": \"increasePlayerRating\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"nftItemsAddress\",      \"type\": \"address\"    }  ],  \"name\": \"initialize\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"player\",      \"type\": \"address\"    }  ],  \"name\": \"isPlayerRegistered\",  \"outputs\": [    {      \"internalType\": \"bool\",      \"name\": \"\",      \"type\": \"bool\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"lastEnergyConsumedDay\",  \"outputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"\",      \"type\": \"uint256\"    }  ],  \"name\": \"leagues\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"name\",      \"type\": \"string\"    },    {      \"internalType\": \"uint256\",      \"name\": \"minRating\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"maxRating\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"rewardLootBoxAmount\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"nftItems\",  \"outputs\": [    {      \"internalType\": \"contract LifeHackatonItems\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"owner\",  \"outputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"\",      \"type\": \"address\"    }  ],  \"name\": \"playerInfo\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"name\",      \"type\": \"string\"    },    {      \"internalType\": \"uint256\",      \"name\": \"currentLeague\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"currentRating\",      \"type\": \"uint256\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"proxiableUUID\",  \"outputs\": [    {      \"internalType\": \"bytes32\",      \"name\": \"\",      \"type\": \"bytes32\"    }  ],  \"stateMutability\": \"view\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"string\",      \"name\": \"name\",      \"type\": \"string\"    }  ],  \"name\": \"register\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"renounceOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"authorizedOperator_\",      \"type\": \"address\"    },    {      \"internalType\": \"bool\",      \"name\": \"authorize\",      \"type\": \"bool\"    }  ],  \"name\": \"setAuthorizedOperator\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"nftItemsAddress\",      \"type\": \"address\"    }  ],  \"name\": \"setNftItems\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newOwner\",      \"type\": \"address\"    }  ],  \"name\": \"transferOwnership\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"uint256\",      \"name\": \"phoneTokenId\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"earbudsTokenId\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"powerbankTokenId\",      \"type\": \"uint256\"    },    {      \"internalType\": \"uint256\",      \"name\": \"laptopTokenId\",      \"type\": \"uint256\"    }  ],  \"name\": \"updatePlayerSelectedObjects\",  \"outputs\": [],  \"stateMutability\": \"nonpayable\",  \"type\": \"function\"},{  \"inputs\": [    {      \"internalType\": \"address\",      \"name\": \"newImplementation\",      \"type\": \"address\"    },    {      \"internalType\": \"bytes\",      \"name\": \"data\",      \"type\": \"bytes\"    }  ],  \"name\": \"upgradeToAndCall\",  \"outputs\": [],  \"stateMutability\": \"payable\",  \"type\": \"function\"},{  \"inputs\": [],  \"name\": \"version\",  \"outputs\": [    {      \"internalType\": \"string\",      \"name\": \"\",      \"type\": \"string\"    }  ],  \"stateMutability\": \"pure\",  \"type\": \"function\"}\r\n]";

    private string _playerAdress;
    private string _enemyAdress;
    private string _battleDiff;
   
    public void ScreenOpened()
    {
        LoadPlayer();
    }
    public void ResetScreen()
    {
        LoadPlayer();
        OnResetScreen.Invoke();
    }
    public async void LoadPlayer()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        var res = await contract.Read<System.Object[]>("playerInfo", _playerAdress);
        playerName.text = res[0].ToString();
        BPPlayer.text = res[2].ToString();
        var resT = await contract.Read<BigInteger>("getPlayerCoolness", _playerAdress);
        CPPlayer.text = resT.ToString();
    }
    public async void LoadEnemy()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonPlayers, abiPlayer);
        var res = await contract.Read<System.Object[]>("playerInfo", _enemyAdress);
        opponentName.text = res[0].ToString();
        BP.text = res[2].ToString();
        var resT = await contract.Read<BigInteger>("getPlayerCoolness", _enemyAdress);
        CP.text = resT.ToString();
        OnOpponentFounded.Invoke();
    }
    public void SetSequenceSelected(BattleSequenceElement element)
    {
        OffSelectSeq();
        selectedSequence =element;
        selectedSequence.SelectElement();
        int img = selectedSequence.crntImage;
        OffSelect();
        switch (img)
        {
            case 0:
                firstSelectElement.SelectElement(); break;
                case 1:
                secondSelectElement.SelectElement(); break;
                case 2:
                thirdSelectElement.SelectElement(); break;
        }
    }
    public void SetSequenceAction(BattleSequenceElement element)
    {
        OffSelect();
        if (selectedSequence!=null) { selectedSequence.SetSelectedImg(element.crntImage); }
        element.SelectElement();
        CheckSeq();
    }
    private void CheckSeq()
    {
        int cnt = 0;
        if(firstBattleElement.crntImage>=0) cnt++;
        if (secondBattleElement.crntImage >= 0) cnt++;
        if (thirdBattleElement.crntImage >= 0) cnt++;
        if(cnt==3)AttackBtn.interactable = true;
        else AttackBtn.interactable = false;
    }
    public void OffSelectSeq()
    {
        
        firstBattleElement.UnSelect();
        secondBattleElement.UnSelect();
        thirdBattleElement.UnSelect();
        
    }
    public void ResetSeq()
    {
        OffSelectSeq();
        selectedSequence = null;
        firstBattleElement.OffImages();
        secondBattleElement.OffImages();
        thirdBattleElement.OffImages();
        CheckSeq();
    }
    public void OffSelect()
    {
        firstSelectElement.UnSelect();
        secondSelectElement.UnSelect();
        thirdSelectElement.UnSelect();
    }
    public async void InitBattle()
    {
        popupMSg.Log("Battle status", "Finding an opponent...");
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonBattles, abiBattle);
        var res = await contract.Write("initiateBattle");
        Debug.Log("BATTLE INITED "+res);
       
        await Task.Delay(5000);
        GetEnemy();
    }

    public async void GetEnemy()
    {       

        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonBattles, abiBattle);
        var res = await contract.Read<System.Object[]>("getEnemyForBattle",_playerAdress);
        _enemyAdress = res[0].ToString();
        _battleDiff = res[1].ToString();
        Debug.Log("ENEMY INFO " + _enemyAdress+" diff "+_battleDiff);
        LoadEnemy();
        popupMSg.Hide();
        
    }

    public async void StartBattle()
    {
      
        int[] actions = new int[3];
        
        actions[0] = firstBattleElement.crntImage;
        actions[1] = secondBattleElement.crntImage;
        actions[2] = thirdBattleElement.crntImage;
        popupMSg.Log("Battle status", "The battle is in progress...");
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonBattles, abiBattle);
        var res = await contract.Write("startBattle", actions);
        Debug.Log("BATTLE Started " + res);
       
        await Task.Delay(5000);
        popupMSg.Log("Battle status", "Waiting for results...");
        CompleteBattle();
    }
    public async void CompleteBattle()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(AdressLifeHackatonBattles, abiBattle);
        var res = await contract.Write("completeBattle");
        string dataValue="";
        JObject responseJson = JObject.Parse(res.receipt.ToString());
        Debug.Log("JSON "+responseJson.ToString());
        for(int i = 0; i < 10; i++)
        {
            dataValue = responseJson["logs"][i]["topics"].ToString();
            if (dataValue.Contains("0xce12559a20e014c77d822237a383835a775af50857453c2792b60e9bbd696092"))
            {
                dataValue = dataValue = responseJson["logs"][i]["data"].ToString();
                break;
            }
            else
            {
                dataValue = "";
            }
        }
        
        string status = dataValue.Substring(40, 30);
        bool containsOne = status.Contains("1");
        WonScreen.transform.parent.gameObject.SetActive(true);
        popupMSg.Hide();
        if (!containsOne)
        {
            LooseScreen.SetActive(true);
            WonScreen.SetActive(false);
        }
        if(containsOne)
        {
            LooseScreen.SetActive(false);
            WonScreen.SetActive(true);
        }
        Debug.Log("BATTLE COMPLETED " + dataValue);
    }
    
    public void InitPlayerAdress()
    {
        _playerAdress = PlayerPrefs.GetString("Adress");
    }
}
