using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void IniciarNavegador()
    {
        SceneManager.LoadScene("Navegador");
    }
}