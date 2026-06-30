using System;

public class FirebaseAuthenticationPresenter
{
    private readonly FirebaseAuthenticationModel _model;
    private readonly FirebaseAuthenticationView _view;

    public FirebaseAuthenticationPresenter(FirebaseAuthenticationModel model, FirebaseAuthenticationView view)
    {
        _model = model;
        _view = view;
    }

    public void Initialize()
    {
        ActivateEvents();

        _model.Initialize();
        _view?.Initialize();
    }

    public void Dispose()
    {
        DeactivateEvents();

        _view?.Dispose();
    }

    private void ActivateEvents()
    {
        _model.OnSignUpMessage_Action += _view.SetDescription;
    }

    private void DeactivateEvents()
    {
        _model.OnSignUpMessage_Action -= _view.SetDescription;
    }

    #region Input

    public bool IsAuthorization()
    {
        return _model.IsAuthorization();
    }

    public void DeleteAccount()
    {
        _model.DeleteAccount();
    }

    public void SignOut()
    {
        _model.SignOut();
    }

    public void SignUp()
    {
        _model.SignUp();
    }

    public void SetNickname(string nickname)
    {
        _model.SetNickname(nickname);
    }

    public event Action<string> OnChangeCurrentUser
    {
        add { _model.OnChangeUser += value; }
        remove { _model.OnChangeUser -= value; }
    }



    public event Action OnSignIn
    {
        add { _model.OnSignIn_Action += value; }
        remove { _model.OnSignIn_Action -= value; }
    }



    public event Action OnSignUp
    {
        add { _model.OnSignUp_Action += value; }
        remove { _model.OnSignUp_Action -= value; }
    }

    public event Action OnSignUpError
    {
        add { _model.OnSignUpError_Action += value; }
        remove { _model.OnSignUpError_Action -= value; }
    }


    public event Action OnSignOut
    {
        add { _model.OnSignOut_Action += value; }
        remove { _model.OnSignOut_Action -= value; }
    }

    public event Action OnDeleteAccount
    {
        add { _model.OnDeleteAccount_Action += value; }
        remove { _model.OnDeleteAccount_Action -= value; }
    }

    #endregion
}
