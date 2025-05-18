using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManagement : Singelton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }

    public void SetTransitionName(string name)
    {
        SceneTransitionName = name;
    }

}
