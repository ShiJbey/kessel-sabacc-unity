using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sabacc
{
public class MainMenuController : MonoBehaviour
{
	[SerializeField]
	private HomeScreenController m_HomeScreen;

	[SerializeField]
	private CreditsScreenController m_CreditsScreen;

	[SerializeField]
	private string m_NewGameScenePath;

	private UIComponent m_CurrentScreen;

	private Stack<UIComponent> m_ScreenStack = new();

    private void Start()
    {
		InitializeScreens();
		ShowScreen(m_HomeScreen);
    }

	private IEnumerator Initialize()
	{
		// yield return new WaitUntil
		yield return null;
	}

	private void InitializeScreens()
	{
		m_HomeScreen.Initialize(this);
		m_HomeScreen.Hide();
		m_CreditsScreen.Initialize(this);
		m_CreditsScreen.Hide();
	}

	public void StartNewGame()
	{
		SceneManager.LoadScene(m_NewGameScenePath);
	}

	public void PushScreen(UIComponent screen)
	{
		if (m_CurrentScreen != null)
		{
			m_CurrentScreen.Hide();
			m_ScreenStack.Push(m_CurrentScreen);
		}

		m_CurrentScreen = screen;
		m_CurrentScreen.Show();
	}

	public void PopScreen()
	{
		m_CurrentScreen.Hide();
		if (m_ScreenStack.Count > 0)
		{
			m_CurrentScreen = m_ScreenStack.Pop();
		}
		m_CurrentScreen.Show();
	}

	public void ShowScreen(UIComponent screen)
	{
		m_ScreenStack.Clear();
		m_CurrentScreen = screen;
		screen.Show();
	}
}

}
