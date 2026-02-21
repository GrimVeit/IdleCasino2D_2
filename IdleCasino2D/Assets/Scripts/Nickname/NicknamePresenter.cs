using System;

public class NicknamePresenter
{
    private readonly NicknameModel nicknameModel;
    private readonly NicknameView nicknameView;

    public NicknamePresenter(NicknameModel nicknameModel, NicknameView nicknameView)
    {
        this.nicknameModel = nicknameModel;
        this.nicknameView = nicknameView;
    }

    public void Initialize()
    {
        ActivateEvents();

        nicknameModel.Initialize();
        nicknameView.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        nicknameView.Dispose();
        nicknameModel.Dispose();
    }

    private void ActivateEvents()
    {
        nicknameView.OnChangeNickname += nicknameModel.ChangeNickname;

        nicknameModel.OnChooseNickname += nicknameView.ChangeNickname;
        nicknameModel.OnEnterRegisterLoginError += nicknameView.DisplayDescription;

        nicknameModel.OnCorrectNickname += nicknameView.ActivateButton;
        nicknameModel.OnIncorrectNickname += nicknameView.DeactivateButton;
    }

    private void DeactivateEvents()
    {
        nicknameView.OnChangeNickname -= nicknameModel.ChangeNickname;

        nicknameModel.OnChooseNickname -= nicknameView.ChangeNickname;
        nicknameModel.OnEnterRegisterLoginError -= nicknameView.DisplayDescription;

        nicknameModel.OnCorrectNickname -= nicknameView.ActivateButton;
        nicknameModel.OnIncorrectNickname -= nicknameView.DeactivateButton;
    }

    #region Input

    public event Action<string> OnChooseNickname
    {
        add { nicknameModel.OnChooseNickname += value; }
        remove { nicknameModel.OnChooseNickname -= value; }
    }

    public event Action OnCorrectNickname
    {
        add { nicknameModel.OnCorrectNickname += value; }
        remove { nicknameModel.OnCorrectNickname -= value; }
    }

    public event Action OnIncorrectNickname
    {
        add { nicknameModel.OnIncorrectNickname += value; }
        remove { nicknameModel.OnIncorrectNickname -= value; }
    }

    #endregion

    #region Output

    public string Nickname => nicknameModel.Nickname;

    #endregion
}
