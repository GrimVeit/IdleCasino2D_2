using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private ViewContainer viewContainer_World;
    [SerializeField] private ShopCasinoEntityDatasSO shopCasinoEntityDatas;
    [SerializeField] private ShopCasinoPersonalDatasSO shopCasinoPersonalDatas;

    [Header("NODES STAFF")]
    [SerializeField] private List<Node> nodesPokerStaff = new();
    [SerializeField] private List<Node> nodesBarStaff = new();
    [SerializeField] private Node nodeSongstress;
    [SerializeField] private Node nodeRouletteStaff_1;
    [SerializeField] private Node nodeRouletteStaff_2;
    [SerializeField] private List<Node> nodesHostess;
    [SerializeField] private List<Node> nodesManager;

    [Header("NODES VISITOR")]
    [SerializeField] private List<Node> nodesEntranceQueue;
    [SerializeField] private List<Node> nodesSlot;
    [SerializeField] private List<Node> nodesWheel;
    [SerializeField] private List<Node> nodesRoulette_1;
    [SerializeField] private List<Node> nodesRoulette_2;
    [SerializeField] private List<Node> nodesPoker;
    [SerializeField] private List<Node> nodesExit;
    [SerializeField] private List<Node> nodesBar;
    [SerializeField] private List<Node> nodesMusic;
    [SerializeField] private Sounds sounds;
    [SerializeField] private UIGameRoot menuRootPrefab;

    private UIGameRoot sceneRoot;
    private ViewContainer viewContainer;

    private BankPresenter bankPresenter;
    private BankTransactionVisualPresenter bankTransactionVisualPresenter;

    private ParticleEffectPresenter particleEffectPresenter;
    private SoundPresenter soundPresenter;

    private CasinoProfitStorePresenter casinoProfitStorePresenter;
    private CasinoProfitPresenter casinoProfitPresenter;

    private CasinoEntityStorePresenter casinoEntityStorePresenter;
    private StaffSpawnerPresenter staffSpawnerPresenter;
    private SpawnerVisitorPresenter spawnerVisitorPresenter;
    private VisitorCounterTrafficPresenter visitorCounterTrafficPresenter;
    private VisitorPathTrafficPresenter visitorPathTrafficPresenter;

    private ProfitOfflinePresenter profitOfflinePresenter;

    private ClickDispatcherPresenter clickDispatcherPresenter;
    private TouchCameraPresenter touchCameraPresenter;
    private MapOrderPresenter mapOrderPresenter;

    private CoinSystemPresenter coinSystemPresenter;
    private CasinoEntityFinancePresenter casinoEntityFinancePresenter;

    private CasinoEntityClickInteractionPresenter casinoEntityClickInteractionPresenter;
    private ShopCasinoSpotPresenter shopCasinoSpotPresenter;

    private ShopCasinoPersonalPresenter shopCasinoPersonalPresenter;
    private FilterShopCasinoStaffPresenter filterShopCasinoStaffPresenter;

    private HostessEntityPresenter hostessEntityPresenter;

    private GameProgressPresenter gameProgressPresenter;
    private AdministratorPresenter administratorPresenter;

    private StateMachine_Game stateMachine;

    private readonly List<ICasinoEntityInfo> casinoEntities = new();

    public void Run(UIRootView uIRootView)
    {
        sceneRoot = menuRootPrefab;

        uIRootView.AttachSceneUI(sceneRoot.gameObject, Camera.main);

        viewContainer = sceneRoot.GetComponent<ViewContainer>();
        viewContainer.Initialize();
        viewContainer_World.Initialize();

        soundPresenter = new SoundPresenter
                    (new SoundModel(sounds.sounds, PlayerPrefsKeys.IS_MUTE_SOUNDS, PlayerPrefsKeys.KEY_VOLUME_SOUND, PlayerPrefsKeys.KEY_VOLUME_MUSIC),
                    viewContainer.GetView<SoundView>());

        bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());
        bankTransactionVisualPresenter = new BankTransactionVisualPresenter(new BankTransactionVisualModel(bankPresenter), viewContainer.GetView<BankTransactionVisualView>());

        casinoProfitStorePresenter = new CasinoProfitStorePresenter(new CasinoProfitStoreModel());
        casinoProfitPresenter = new CasinoProfitPresenter(new CasinoProfitModel(casinoProfitStorePresenter, casinoProfitStorePresenter, casinoProfitStorePresenter, bankPresenter, soundPresenter), viewContainer.GetView<CasinoProfitView>());

        particleEffectPresenter = new ParticleEffectPresenter
            (new ParticleEffectModel(),
            viewContainer.GetView<ParticleEffectView>());

        CreateCasinoEntities();

        spawnerVisitorPresenter = new SpawnerVisitorPresenter(new SpawnerVisitorModel(soundPresenter), viewContainer.GetView<SpawnerVisitorView>());

        visitorPathTrafficPresenter = new VisitorPathTrafficPresenter(new VisitorPathTrafficModel(casinoEntities, spawnerVisitorPresenter, spawnerVisitorPresenter));
        hostessEntityPresenter = new HostessEntityPresenter(new HostessEntityModel(casinoEntities, visitorPathTrafficPresenter, nodesHostess), viewContainer.GetView<HostessEntityView>());
        casinoEntities.Add(hostessEntityPresenter);

        profitOfflinePresenter = new ProfitOfflinePresenter(new ProfitOfflineModel(casinoEntities, casinoProfitStorePresenter, bankPresenter), viewContainer.GetView<ProfitOfflineView>());
        casinoEntityStorePresenter = new CasinoEntityStorePresenter(new CasinoEntityStoreModel(casinoEntities));
        staffSpawnerPresenter = new StaffSpawnerPresenter(new StaffSpawnerModel(casinoEntities), viewContainer.GetView<StaffSpawnerView>());
        visitorCounterTrafficPresenter = new VisitorCounterTrafficPresenter(new VisitorCounterTrafficModel(spawnerVisitorPresenter, spawnerVisitorPresenter, casinoEntities));

        clickDispatcherPresenter = new ClickDispatcherPresenter(new ClickDispatcherModel());
        touchCameraPresenter = new TouchCameraPresenter(viewContainer.GetView<TouchCameraView>());
        mapOrderPresenter = new MapOrderPresenter(new MapOrderModel(spawnerVisitorPresenter, staffSpawnerPresenter), viewContainer.GetView<MapOrderView>());

        coinSystemPresenter = new CoinSystemPresenter(new CoinSystemModel(bankPresenter), viewContainer.GetView<CoinSystemView>());
        casinoEntityFinancePresenter = new CasinoEntityFinancePresenter(new CasinoEntityFinanceModel(casinoEntities, coinSystemPresenter));

        casinoEntityClickInteractionPresenter = new CasinoEntityClickInteractionPresenter(new CasinoEntityClickInteractionModel(casinoEntities));
        shopCasinoSpotPresenter = new ShopCasinoSpotPresenter(new ShopCasinoSpotModel(casinoEntityClickInteractionPresenter, bankPresenter, shopCasinoEntityDatas, soundPresenter), viewContainer.GetView<ShopCasinoSpotView>());

        shopCasinoPersonalPresenter = new ShopCasinoPersonalPresenter(new ShopCasinoPersonalModel(shopCasinoPersonalDatas, soundPresenter), viewContainer.GetView<ShopCasinoPersonalView>());
        filterShopCasinoStaffPresenter = new FilterShopCasinoStaffPresenter(new FilterShopCasinoStaffModel(casinoEntities, bankPresenter, staffSpawnerPresenter, shopCasinoPersonalPresenter, soundPresenter), viewContainer.GetView<FilterShopCasinoStaffView>());

        gameProgressPresenter = new GameProgressPresenter(new GameProgressModel(bankPresenter), viewContainer.GetView<GameProgressView>());
        administratorPresenter = new AdministratorPresenter(viewContainer.GetView<AdministratorView>());

        sceneRoot.SetSoundProvider(soundPresenter);
        sceneRoot.Activate();

        ActivateEvents();

        soundPresenter.Initialize();
        particleEffectPresenter.Initialize();
        sceneRoot.Initialize();
        bankPresenter.Initialize();
        bankTransactionVisualPresenter.Initialize();
        casinoProfitStorePresenter.Initialize();
        casinoProfitPresenter.Initialize();

        casinoEntityStorePresenter.Initialize();
        staffSpawnerPresenter.Initialize();
        spawnerVisitorPresenter.Initialize();
        visitorCounterTrafficPresenter.Initialize();
        visitorPathTrafficPresenter.Initialize();
        hostessEntityPresenter.Initialize();
        gameProgressPresenter.Initialize();
        administratorPresenter.Initialize();
        profitOfflinePresenter.Initialize();

        stateMachine = new StateMachine_Game
            (sceneRoot, 
            visitorCounterTrafficPresenter, 
            touchCameraPresenter, 
            clickDispatcherPresenter,
            shopCasinoSpotPresenter,
            shopCasinoSpotPresenter,
            shopCasinoPersonalPresenter,
            filterShopCasinoStaffPresenter,
            filterShopCasinoStaffPresenter,
            hostessEntityPresenter,
            hostessEntityPresenter,
            casinoProfitPresenter,
            administratorPresenter,
            profitOfflinePresenter,
            profitOfflinePresenter,
            soundPresenter);

        touchCameraPresenter.Initialize();
        mapOrderPresenter.Initialize();
        coinSystemPresenter.Initialize();
        casinoEntityFinancePresenter.Initialize();

        casinoEntityClickInteractionPresenter.Initialize();
        shopCasinoSpotPresenter.Initialize();

        shopCasinoPersonalPresenter.Initialize();
        filterShopCasinoStaffPresenter.Initialize();
        stateMachine.Initialize();
    }

    private void CreateCasinoEntities()
    {
        var entityEnter = new EntranceQueueEntityPresenter(new EntranceQueueEntityModel(nodesEntranceQueue));
        entityEnter.Initialize();
        casinoEntities.Add(entityEnter);

        var entityExit = new ExitEntityPresenter(new ExitEntityModel(nodesExit));
        entityExit.Initialize();
        casinoEntities.Add(entityExit);

        var entityBar = new BarEntityPresenter(new BarEntityModel(casinoProfitStorePresenter, nodesBar, nodesBarStaff));
        entityBar.Initialize();
        casinoEntities.Add(entityBar);

        var musicZone = new MusicEntityPresenter(new MusicEntityModel(casinoProfitStorePresenter, nodesMusic, nodeSongstress));
        musicZone.Initialize();
        casinoEntities.Add(musicZone);

        var management = new ManagementEntityPresenter(new ManagementEntityModel(nodesManager));
        management.Initialize();
        casinoEntities.Add(management);

        for (int i = 0; i < 6; i++)
        {
            var spot = new SlotSpotPresenter(new SlotSpotModel(), viewContainer_World.GetView<SlotSpotView>($"Slot_{i + 1}"));
            spot.Initialize();

            var entity = new SlotMachineEntityPresenter(new SlotMachineEntityModel(casinoProfitStorePresenter, spot, nodesSlot[i]));
            entity.Initialize();

            casinoEntities.Add(entity);

            if(i == 0)
                entity.Open();
        }

        for (int i = 0; i < 6; i++)
        {
            var spot = new WheelSpotPresenter(new WheelSpotModel(), viewContainer_World.GetView<WheelSpotView>($"Wheel_{i + 1}"));
            spot.Initialize();

            var entity = new WheelEntityPresenter(new WheelEntityModel(casinoProfitStorePresenter, spot, nodesWheel[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }

        for (int i = 0; i < 4; i++)
        {
            var spot = new PokerSpotPresenter(new PokerSpotModel(), viewContainer_World.GetView<PokerSpotView>($"Poker_{i + 1}"));
            spot.Initialize();

            var entity = new PokerEntityPresenter(new PokerEntityModel(casinoProfitStorePresenter, spot, nodesPoker[i], nodesPokerStaff[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }

        var spotRoulette_1 = new RouletteSpotPresenter(new RouletteSpotModel(), viewContainer_World.GetView<RouletteSpotView>($"Roulette_1"));
        spotRoulette_1.Initialize();
        var entityRoulette_1 = new RouletteEntityPresenter(new RouletteEntityModel(casinoProfitStorePresenter, spotRoulette_1, nodesRoulette_1[1], nodeRouletteStaff_1));
        entityRoulette_1.Initialize();
        casinoEntities.Add(entityRoulette_1);

        var spotRoulette_2 = new RouletteSpotPresenter(new RouletteSpotModel(), viewContainer_World.GetView<RouletteSpotView>($"Roulette_2"));
        spotRoulette_2.Initialize();
        var entityRoulette_2 = new RouletteEntityPresenter(new RouletteEntityModel(casinoProfitStorePresenter, spotRoulette_2, nodesRoulette_2[1], nodeRouletteStaff_2));
        entityRoulette_2.Initialize();
        casinoEntities.Add(entityRoulette_2);

        Debug.Log("5");
    }

    private void ActivateEvents()
    {
        ActivateTransitions();
    }

    private void DeactivateEvents()
    {
        DeactivateTransitions();
    }

    private void ActivateTransitions()
    {

    }

    private void DeactivateTransitions()
    {

    }

    private void Deactivate()
    {
        clickDispatcherPresenter?.Deactivate();
        sceneRoot.Deactivate();
        soundPresenter?.Dispose();
    }

    private void Dispose()
    {
        casinoEntityStorePresenter?.Dispose();
        staffSpawnerPresenter?.Dispose();

        DeactivateEvents();

        soundPresenter?.Dispose();
        sceneRoot?.Dispose();
        particleEffectPresenter?.Dispose();
        bankPresenter?.Dispose();
        bankTransactionVisualPresenter?.Dispose();

        casinoProfitStorePresenter?.Dispose();
        casinoProfitPresenter?.Dispose();
        profitOfflinePresenter?.Dispose();

        //touchCameraPresenter?.Dispose();
        
        stateMachine?.Dispose();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    private void OnApplicationQuit()
    {
        Dispose();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            casinoProfitStorePresenter?.Dispose();
            casinoEntityStorePresenter?.Dispose();
            profitOfflinePresenter?.Save();
            staffSpawnerPresenter?.Save();
            bankPresenter?.Save();
        }
    }

    void OnApplicationFocus(bool focusStatus)
    {
        if (!focusStatus)
        {
            casinoProfitStorePresenter?.Dispose();
            casinoEntityStorePresenter?.Dispose();
            profitOfflinePresenter?.Save();
            staffSpawnerPresenter?.Save();
            bankPresenter?.Save();
        }
    }

    #region Output


    public event Action OnClickToMenu;
    public event Action OnClickToGame;

    private void HandleClickToMenu()
    {
        Deactivate();

        OnClickToMenu?.Invoke();
    }

    private void HandleClickToGame()
    {
        Deactivate();

        OnClickToGame?.Invoke();
    }

    #endregion
}
