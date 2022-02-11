﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAimShootAnims {

    event EventHandler<CharacterAim_Base.OnShootEventArgs> OnShoot;

    void SetAimTarget(Vector3 targetPosition);
    void ShootTarget(Vector3 targetPosition, Action onShootComplete);

}
