using System;
using System.Collections;
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

    [Header("NODES VISITOR")]
    [SerializeField] private List<Node> nodesEntranceQueue;
    [SerializeField] private List<Node> nodesSlot;
    [SerializeField] private List<Node> nodesWheel;
    [SerializeField] private List<Node> nodesPoker;
    [SerializeField] private List<Node> nodesExit;
    [SerializeField] private List<Node> nodesBar;
    [SerializeField] private List<Node> nodesMusic;
    [SerializeField] private Sounds sounds;
    [SerializeField] private UIGameRoot menuRootPrefab;

    private UIGameRoot sceneRoot;
    private ViewContainer viewContainer;

    private BankPresenter bankPresenter;
    private ParticleEffectPresenter particleEffectPresenter;
    private SoundPresenter soundPresenter;

    private StaffSpawnerPresenter staffSpawnerPresenter;
    private SpawnerVisitorPresenter spawnerVisitorPresenter;
    private VisitorCounterTrafficPresenter visitorCounterTrafficPresenter;
    private VisitorPathTrafficPresenter visitorPathTrafficPresenter;

    private ClickDispatcherPresenter clickDispatcherPresenter;
    private TouchCameraPresenter touchCameraPresenter;
    private MapOrderPresenter mapOrderPresenter;

    private CoinSystemPresenter coinSystemPresenter;
    private CasinoEntityFinancePresenter casinoEntityFinancePresenter;

    private CasinoEntityClickInteractionPresenter casinoEntityClickInteractionPresenter;
    private ShopCasinoSpotPresenter shopCasinoSpotPresenter;

    private ShopCasinoPersonalPresenter shopCasinoPersonalPresenter;
    private FilterShopCasinoStaffPresenter filterShopCasinoStaffPresenter;

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

        particleEffectPresenter = new ParticleEffectPresenter
            (new ParticleEffectModel(),
            viewContainer.GetView<ParticleEffectView>());

        bankPresenter = new BankPresenter(new BankModel(), viewContainer.GetView<BankView>());

        CreateCasinoEntities();

        staffSpawnerPresenter = new StaffSpawnerPresenter(new StaffSpawnerModel(), viewContainer.GetView<StaffSpawnerView>());
        spawnerVisitorPresenter = new SpawnerVisitorPresenter(new SpawnerVisitorModel(), viewContainer.GetView<SpawnerVisitorView>());
        visitorCounterTrafficPresenter = new VisitorCounterTrafficPresenter(new VisitorCounterTrafficModel(spawnerVisitorPresenter, spawnerVisitorPresenter, casinoEntities));
        visitorPathTrafficPresenter = new VisitorPathTrafficPresenter(new VisitorPathTrafficModel(casinoEntities, spawnerVisitorPresenter, spawnerVisitorPresenter));

        clickDispatcherPresenter = new ClickDispatcherPresenter(new ClickDispatcherModel());
        touchCameraPresenter = new TouchCameraPresenter(viewContainer.GetView<TouchCameraView>());
        mapOrderPresenter = new MapOrderPresenter(new MapOrderModel(spawnerVisitorPresenter, staffSpawnerPresenter), viewContainer.GetView<MapOrderView>());

        coinSystemPresenter = new CoinSystemPresenter(new CoinSystemModel(bankPresenter), viewContainer.GetView<CoinSystemView>());
        casinoEntityFinancePresenter = new CasinoEntityFinancePresenter(new CasinoEntityFinanceModel(casinoEntities, coinSystemPresenter));

        casinoEntityClickInteractionPresenter = new CasinoEntityClickInteractionPresenter(new CasinoEntityClickInteractionModel(casinoEntities));
        shopCasinoSpotPresenter = new ShopCasinoSpotPresenter(new ShopCasinoSpotModel(casinoEntityClickInteractionPresenter, bankPresenter, shopCasinoEntityDatas), viewContainer.GetView<ShopCasinoSpotView>());

        shopCasinoPersonalPresenter = new ShopCasinoPersonalPresenter(new ShopCasinoPersonalModel(shopCasinoPersonalDatas, bankPresenter), viewContainer.GetView<ShopCasinoPersonalView>());
        filterShopCasinoStaffPresenter = new FilterShopCasinoStaffPresenter(new FilterShopCasinoStaffModel(casinoEntities, bankPresenter, staffSpawnerPresenter, shopCasinoPersonalPresenter), viewContainer.GetView<FilterShopCasinoStaffView>());

        sceneRoot.SetSoundProvider(soundPresenter);
        sceneRoot.Activate();

        ActivateEvents();

        soundPresenter.Initialize();
        particleEffectPresenter.Initialize();
        sceneRoot.Initialize();
        bankPresenter.Initialize();

        staffSpawnerPresenter.Initialize();
        spawnerVisitorPresenter.Initialize();
        visitorCounterTrafficPresenter.Initialize();
        visitorPathTrafficPresenter.Initialize();

        stateMachine = new StateMachine_Game
            (sceneRoot, 
            visitorCounterTrafficPresenter, 
            touchCameraPresenter, 
            clickDispatcherPresenter,
            shopCasinoSpotPresenter,
            shopCasinoSpotPresenter,
            shopCasinoPersonalPresenter,
            filterShopCasinoStaffPresenter,
            filterShopCasinoStaffPresenter);

        stateMachine.Initialize();
        touchCameraPresenter.Initialize();
        mapOrderPresenter.Initialize();
        coinSystemPresenter.Initialize();
        casinoEntityFinancePresenter.Initialize();

        casinoEntityClickInteractionPresenter.Initialize();
        shopCasinoSpotPresenter.Initialize();

        shopCasinoPersonalPresenter.Initialize();
        filterShopCasinoStaffPresenter.Initialize();
    }

    private void CreateCasinoEntities()
    {
        var entityEnter = new EntranceQueueEntityPresenter(new EntranceQueueEntityModel(nodesEntranceQueue));
        entityEnter.Initialize();
        casinoEntities.Add(entityEnter);

        var entityExit = new ExitEntityPresenter(new ExitEntityModel(nodesExit));
        entityExit.Initialize();
        casinoEntities.Add(entityExit);

        var entityBar = new BarEntityPresenter(new BarEntityModel(nodesBar, nodesBarStaff));
        entityBar.Initialize();
        casinoEntities.Add(entityBar);

        var musicZone = new MusicEntityPresenter(new MusicEntityModel(nodesMusic, nodeSongstress));
        musicZone.Initialize();
        casinoEntities.Add(musicZone);

        for (int i = 0; i < 6; i++)
        {
            var spot = new SlotSpotPresenter(new SlotSpotModel(), viewContainer_World.GetView<SlotSpotView>($"Slot_{i + 1}"));

            Debug.Log("2.1");
            spot.Initialize();

            Debug.Log("2.2");

            var entity = new SlotMachineEntityPresenter(new SlotMachineEntityModel(spot, nodesSlot[i]));
            entity.Initialize();

            Debug.Log("2.3");

            casinoEntities.Add(entity);
        }

        for (int i = 0; i < 6; i++)
        {
            var spot = new WheelSpotPresenter(new WheelSpotModel(), viewContainer_World.GetView<WheelSpotView>($"Wheel_{i + 1}"));
            spot.Initialize();

            var entity = new WheelEntityPresenter(new WheelEntityModel(spot, nodesWheel[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }

        for (int i = 0; i < 4; i++)
        {
            var spot = new PokerSpotPresenter(new PokerSpotModel(), viewContainer_World.GetView<PokerSpotView>($"Poker_{i + 1}"));
            spot.Initialize();

            var entity = new PokerEntityPresenter(new PokerEntityModel(spot, nodesPoker[i], nodesPokerStaff[i]));
            entity.Initialize();

            casinoEntities.Add(entity);
        }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            staffSpawnerPresenter.SetStaff(casinoEntities[15] as ICasinoEntityStaff, StaffType.Croupier);
        }
    }

    private void Deactivate()
    {
        clickDispatcherPresenter?.Deactivate();
        sceneRoot.Deactivate();
        soundPresenter?.Dispose();
    }

    private void Dispose()
    {
        DeactivateEvents();

        soundPresenter?.Dispose();
        sceneRoot?.Dispose();
        particleEffectPresenter?.Dispose();
        bankPresenter?.Dispose();

        //touchCameraPresenter?.Dispose();
        
        stateMachine?.Dispose();
    }

    private void OnDestroy()
    {
        Dispose();
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
