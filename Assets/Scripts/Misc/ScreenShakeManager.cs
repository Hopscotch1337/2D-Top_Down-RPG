using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;

public class ScreenShakeManager : Singelton<ScreenShakeManager>
{
    private CinemachineImpulseSource impulseSource;
    protected override void Awake()
    {
        base.Awake();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void ScreenShake()
    {
        impulseSource.GenerateImpulse();
    }
}
