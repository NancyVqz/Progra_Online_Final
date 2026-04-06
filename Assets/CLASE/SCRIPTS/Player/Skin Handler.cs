using System;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviour
{
    public Color[] material;
    public SkinnedMeshRenderer[] partesDelCuerpo;


    //private void Start()
    //{
    //    //cuando consiga la informacion del jugador, quiero cambiar la skin del player
    //   PlayFabManager._PlayfabManager.GetData(new Action[] { ApplySkin });
    //}

    //public void ApplySkin()
    //{
    //    foreach(var currentSkin in playerRenderers) 
    //    {
    //        if(currentSkin.skinnedMeshRenderer == null) // Si mi skinnedMeshRenderer tiene algo, lo cambio
    //        {
    //            //Tengo mi indice de que skin tengo puesta
    //            if(currentSkin.skinIndex != int.Parse(PlayFabManager._PlayfabManager.playerData[currentSkin.name].Value))
    //            {
    //                //mi skin actual
    //                currentSkin.skinnedMeshRenderer =
    //                    skins[currentSkin.name].skinnedMeshRenderer[int.Parse(PlayFabManager._PlayfabManager.playerData[currentSkin.name].Value)];
    //            }
    //        }
    //        else if (currentSkin.renderer != null) // Si no, reviso si el renderer tiene algo dentro y lo cambio
    //        {
    //            if (currentSkin.skinIndex != int.Parse(PlayFabManager._PlayfabManager.playerData[currentSkin.name].Value))
    //            {
    //                //skin.renderer =
    //            }
    //        }
    //    }
    //}

    [Serializable]
    public struct PlayerRenderers
    {
        public string name;
        public SkinnedMeshRenderer skinnedMeshRenderer; //renderer para lo que tiene rigg
        public Renderer renderer; //para el normal que no tiene rig
        public int skinIndex;
    }

    [Serializable]
    public struct Skins
    {
        public string name;
        public SkinnedMeshRenderer[] skinnedMeshRenderer; //renderer para lo que tiene rigg
        public Material[] material;
        public int skinIndex;
    }
}
