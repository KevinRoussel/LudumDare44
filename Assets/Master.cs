using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] Gameplay _gameplay;
    [SerializeField] CanvasGroup _splashScreen;

    Coroutine _game;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        _game = StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        yield return SplashScreen();
        IEnumerator SplashScreen()
        {
            _splashScreen.gameObject.SetActive(true);
            _splashScreen.alpha = 0;

            CountDown cd = new CountDown(0.5f);
            while(!cd.isDone)
            {
                _splashScreen.alpha = cd.Progress;
                yield return null;
            }
            yield return new WaitForSeconds(3f);

            cd = new CountDown(0.5f);
            while (!cd.isDone)
            {
                _splashScreen.alpha = 1-cd.Progress;
                yield return null;
            }

            _splashScreen.gameObject.SetActive(false);
            yield break;
        }

        while(true)
        {
            MenuReturn menuChoice = MenuReturn.Null;
            yield return Menu((m) => menuChoice = m);

            switch (menuChoice)
            {
                case MenuReturn.StartGame:
                    yield return Tuto();
                    yield return _gameplay.RunGame();
                    yield break;
                    break;

                case MenuReturn.Credit:
                    yield return Credit();
                    break;

                case MenuReturn.Tuto:
                    yield return Tuto();
                    break;

                case MenuReturn.Null:
                default:
                    break;
            }
        }


        yield break;
    }

    #region Menu
    [Header("Menu UI")]
    [SerializeField] Animation _menuAnimation;
    [SerializeField] AnimationClip _menuOpenAnimation;
    [SerializeField] AnimationClip _menuCloseAnimation;
    [SerializeField] Button _gameButton;
    [SerializeField] Button _creditButton; 
    [SerializeField] Button _tutoButton;

    public Action OnMenuStart;

    enum MenuReturn { Null, StartGame, Credit, Tuto }
    IEnumerator Menu(Action<MenuReturn> @return)
    {
        OnMenuStart?.Invoke();
        _menuAnimation.gameObject.SetActive(true);
        _menuAnimation.Play(_menuOpenAnimation.name);

        MenuReturn choice = MenuReturn.Null;
        _gameButton.onClick.AddListener(() => choice = MenuReturn.StartGame);
        _creditButton.onClick.AddListener(() => choice = MenuReturn.Credit);
        _tutoButton.onClick.AddListener(() => choice = MenuReturn.Tuto);

        yield return new WaitWhile(()=> choice == MenuReturn.Null);

        _gameButton.onClick.RemoveAllListeners();
        _creditButton.onClick.RemoveAllListeners();
        _tutoButton.onClick.RemoveAllListeners();
        @return.Invoke(choice);

        //yield return _menuAnimation.PlayAndWait(_menuCloseAnimation.name);
        _menuAnimation.gameObject.SetActive(false);
        yield break;
    }
    #endregion

    #region Credit
    [Header("Credit")]
    [SerializeField] Animation _creditAnimation;
    [SerializeField] AnimationClip _creditOpenAnimation;
    [SerializeField] AnimationClip _creditCloseAnimation;
    [SerializeField] Button _returnButton;
    IEnumerator Credit()
    {
        _creditAnimation.gameObject.SetActive(true);
        yield return _creditAnimation.PlayAndWait(_creditOpenAnimation.name);

        bool done = false;
        _returnButton.onClick.AddListener(() => done=true);
        yield return new WaitWhile(() => !done);
        _returnButton.onClick.RemoveAllListeners();

        _creditAnimation.gameObject.SetActive(false);
        yield break;
    }
    #endregion

    #region Tuto
    [Header("Tuto")]
    [SerializeField] Transform _tutoAnimation;
    [SerializeField] Button _nextButton;

    IEnumerator Tuto()
    {
        _tutoAnimation.gameObject.SetActive(true);

        bool done = false;
        _nextButton.onClick.AddListener(() => done = true);
        yield return new WaitWhile(() => !done);
        _nextButton.onClick.RemoveAllListeners();

        _tutoAnimation.gameObject.SetActive(false);
        yield break;
    }
    #endregion

}
