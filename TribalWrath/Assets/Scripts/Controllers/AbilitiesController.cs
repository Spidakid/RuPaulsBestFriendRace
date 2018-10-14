﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesController : MonoBehaviour {
    [Tooltip("The Type of Light you would like to use for the Owl ability.")]
    public Light lightObject;

    private AbilityState ability = AbilityState.Normal;
    [Tooltip("Go to the Unity Documentation for more details")]
    public ForceMode forceMode = ForceMode.Force;
    public ForceMode highJumpForceMode = ForceMode.Force;
    Jumping jump;
    LightSwitch light;

    [Tooltip("Amount of Force Required to Jump")]
    public float jumpForce = 12;
    [Tooltip("Maximum Height to jump before descending")]
    public float jumpHeight = 5;
    [Tooltip("Amount of Force Required to HIGH Jump")]
    public float highJumpForce = 12;
    [Tooltip("Maximum Height to HIGH Jump before descending")]
    public float highJumpHeight = 8;
    // Use this for initialization
    void Start()
    {
        jump = new Jumping();
        jump.rigidbody = GetComponent<Rigidbody>();
        jump.savedJumpHeight = jumpHeight;
        jump.savedHighJumpHeight = jump.highJumpHeight = highJumpHeight;
        
        light = new LightSwitch(lightObject);
        light.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        InputSwitchAbilities();
        Debug.Log("Jump Height: " + jump.jumpHeight);
        Debug.Log("HighJump Height: " + jump.highJumpHeight);
        Debug.Log("Ability: " + ability);
    }
    private void LateUpdate()
    {
        light.LateUpdate();
    }
    private void UpdateJumpInput()
    {
        if (KeyboardInputUtil.KeyWasPressed(KeyCode.Space) && !jump.isJumping)
        {
            jump.Direction = Vector3.up;
            Debug.Log("Jumping!");
            jump.isJumping = true;
        }
        jump.Direction.Normalize();
    }
    private void InputSwitchAbilities()
    {
        if (KeyboardInputUtil.KeyWasPressed(KeyCode.Alpha1))
        {
            ability = AbilityState.HighJump;
            light.state = LightState.LightOff;
        }
        //else if (KeyboardInputUtil.KeyWasPressed(KeyCode.Alpha2))
        //{
            //light.state = LightState.LightOff;
        //}
        else if (KeyboardInputUtil.KeyWasPressed(KeyCode.Alpha3))
        {
            ability = AbilityState.OwlSight;
        }
        AbilitySwitch();
    }
    private void AbilitySwitch()
    {
        switch (ability)
        {
            case AbilityState.Normal:
                light.state = LightState.LightOff;
                RegularJump(jumpForce, forceMode);
                break;
            case AbilityState.HighJump:
                
                RegularJump(highJumpForce, highJumpForceMode);
                break;
            case AbilityState.AcidSpit:
                //TODO: Enter code for projectile ability below

                RegularJump(jumpForce, forceMode);
                break;
            case AbilityState.OwlSight:
                light.state = LightState.LightOn;
                RegularJump(jumpForce, forceMode);
                break;
            default:
                ability = AbilityState.Normal;
                break;
        }
        jump.MaximumjumpHeightCheck(this.gameObject,ability);
    }
    private void RegularJump(float _jumpforce, ForceMode _forceMode)
    {
        UpdateJumpInput();

        jump.jumpPower = _jumpforce;
        jump.UpdateJumpMovement(_forceMode);
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            jump.isJumping = false;
            Debug.Log("Not Jumping");
            jump.newJumpHeight(this.gameObject,ability);
        }
    }
}
