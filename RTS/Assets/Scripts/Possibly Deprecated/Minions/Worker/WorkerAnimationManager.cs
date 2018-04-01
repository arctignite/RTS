using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnimationManager : MonoBehaviour {

    public Animator anim;

    #region animator state variables
    [Header("Left Hand Equipment")]
    [SerializeField] private bool bag;
    [SerializeField] private bool wood;

    [Space]

    [Header("Right Hand Equipment")]
    public GameObject axeObject;
    public GameObject pickObject;
    public GameObject hammerObject;

    [SerializeField] private bool axe;
    [SerializeField] private bool pick;
    [SerializeField] private bool hammer;      

    [Space]

    [Header("Actions")]
    [SerializeField] private bool attacking;
    [SerializeField] private bool walking;
    [SerializeField] private bool idle;

    #endregion

    #region Left hand classes
    public void NoLoad()
    {
        bag = false;
        wood = false;

        anim.SetBool("bag", false);
        anim.SetBool("wood", false);
    }

    public void EquipBag()
    {
        bag = true;
        wood = false;

        anim.SetBool("bag", true);
        anim.SetBool("wood", false);
    }

    public void EquipWood()
    {
        bag = false;
        wood = true;

        anim.SetBool("bag", false);
        anim.SetBool("wood", true);
    }
    #endregion

    #region right hand classes
    public void NoItem()
    {
        axe = false;        
        pick = false;
        hammer = false;

        axeObject.SetActive(false);
        pickObject.SetActive(false);
        hammerObject.SetActive(false);
    }

    public void EquipAxe()
    {
        axe = true;       
        pick = false;       
        hammer = false;

        axeObject.SetActive(true);
        pickObject.SetActive(false);
        hammerObject.SetActive(false);
    }

    public void EquipPick()
    {
        axe = false;      
        pick = true;       
        hammer = false;

        axeObject.SetActive(false);
        pickObject.SetActive(true);
        hammerObject.SetActive(false);
    }

    public void EquipHammer()
    {
        axe = false;        
        pick = false;
        hammer = true;

        axeObject.SetActive(false);
        pickObject.SetActive(false);
        hammerObject.SetActive(true);
    }

    #endregion

    #region action classes
    public void Idle()
    {
        idle = true;
        attacking = false;
        walking = false;

        anim.SetBool("attacking", false);
        anim.SetBool("walking", false);
    }

    public void Attacking()
    {
        idle = false;
        attacking = true;
        walking = false;

        anim.SetBool("attacking", true);
        anim.SetBool("walking", false);
    }

    public void Walking()
    {
        idle = true;
        attacking = false;
        walking = true;

        anim.SetBool("attacking", false);
        anim.SetBool("walking", true);
    }
    #endregion

    private void Start()
    {
        Idle();
    }
}
