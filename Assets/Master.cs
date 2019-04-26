using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Master : MonoBehaviour
{

    [SerializeField] Gameplay _gameplay;

    Coroutine _game;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _game = StartCoroutine(Game());
    }

    IEnumerator Game()
    {
        while(true)
        {
            MenuReturn menuChoice = MenuReturn.Null;
            yield return Menu((m) => menuChoice = m);

            switch (menuChoice)
            {
                case MenuReturn.StartGame:
                    yield return _gameplay.RunGame();
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

    [Header("Menu UI")]
    [SerializeField] Animation _menuAnimation;
    [SerializeField] AnimationClip _menuOpenAnimation;
    [SerializeField] AnimationClip _menuCloseAnimation;
    [SerializeField] Button _gameButton;
    [SerializeField] Button _creditButton; 
    [SerializeField] Button _tutoButton;

    enum MenuReturn { Null, StartGame, Credit, Tuto }
    IEnumerator Menu(Action<MenuReturn> @return)
    {
        _menuAnimation.gameObject.SetActive(true);
        yield return _menuAnimation.PlayAndWait(_menuOpenAnimation.name);

        MenuReturn choice = MenuReturn.Null;
        _gameButton.onClick.AddListener(() => choice = MenuReturn.StartGame);
        _creditButton.onClick.AddListener(() => choice = MenuReturn.Credit);
        _tutoButton.onClick.AddListener(() => choice = MenuReturn.Tuto);

        yield return new WaitWhile(()=> choice == MenuReturn.Null);

        _gameButton.onClick.RemoveAllListeners();
        _creditButton.onClick.RemoveAllListeners();
        _tutoButton.onClick.RemoveAllListeners();
        @return.Invoke(choice);

        yield return _menuAnimation.PlayAndWait(_menuCloseAnimation.name);
        _menuAnimation.gameObject.SetActive(false);
        yield break;
    }


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

        yield return _creditAnimation.PlayAndWait(_creditCloseAnimation.name);
        _creditAnimation.gameObject.SetActive(false);
        yield break;
    }

    [Header("Tuto")]
    [SerializeField] Animation _tutoAnimation;
    [SerializeField] AnimationClip _tutoOpenAnimation;
    [SerializeField] AnimationClip _tutoCloseAnimation;
    [SerializeField] Button _nextButton;
    IEnumerator Tuto()
    {
        _tutoAnimation.gameObject.SetActive(true);
        yield return _tutoAnimation.PlayAndWait(_tutoOpenAnimation.name);

        bool done = false;
        _nextButton.onClick.AddListener(() => done = true);
        yield return new WaitWhile(() => !done);
        _nextButton.onClick.RemoveAllListeners();

        yield return _tutoAnimation.PlayAndWait(_tutoCloseAnimation.name);
        _tutoAnimation.gameObject.SetActive(false);
        yield break;
    }

}
