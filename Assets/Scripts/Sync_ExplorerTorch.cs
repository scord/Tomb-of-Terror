﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Sync_ExplorerTorch : NetworkBehaviour {

  [SerializeField] private GameObject m_Torch;
  [SerializeField] private ExplorerController m_Controller;

  private TorchManager torchManagerScript;

  [SyncVar (hook = "UpdateTorchStatus")] private bool isActive;


  void Start() {
    if ( !isLocalPlayer ) m_Torch.SetActive(true);
    torchManagerScript = new TorchManager(m_Torch);
    if ( isLocalPlayer ) {
      torchManagerScript = m_Controller.GetTorchManager();
      CmdChangeActive(true);
    }
    torchManagerScript.Trigger(isActive);
    //SetPlayStop();
  }

  void Update() {
    torchManagerScript.SetLight();
    //SetLight();
    if ( !isLocalPlayer ) {
      return;
    }
    if ( isLocalPlayer ) {
      if ( isActive != m_Controller.GetTorchState()) {
        CmdChangeActive(!isActive);
      }
      //if (Input.GetButtonDown("Fire1")) {
      //  CmdChangeActive(!isActive);
      //}
    } 
  }

  [Command]
  void CmdChangeActive(bool newValue) {
    isActive = newValue;
  }

  [Client]
  void UpdateTorchStatus(bool newValue) {
    isActive = newValue;
    torchManagerScript.Trigger(isActive);
    //SetPlayStop();
  }

}
