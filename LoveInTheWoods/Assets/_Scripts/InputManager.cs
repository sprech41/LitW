using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    /// <summary>
    /// Does neat shit like handling universalization of input n' stuff.
    /// 
    /// To use this once an object of this class has been created from Global, use something like:
    /// DoesThePlayerWantToInteract = (Global.inputManager.interact == ButtonStates.Pressed);
    /// </summary>

    //Variable declaration
    float hAxis = 0f;
    float vAxis = 0f;
    
    enum ButtonStates
    {
        Up,
        Down,
        Pressed,
        Held,
        Released        
    };

    //Initializing button states
    ButtonStates interact = ButtonStates.Up;
    ButtonStates cancel = ButtonStates.Up;
    ButtonStates menu = ButtonStates.Up;
    ButtonStates jump = ButtonStates.Up;

    //For storing previous frame button state
    ButtonStates prevInteract = ButtonStates.Up;
    ButtonStates prevCancel = ButtonStates.Up;
    ButtonStates prevMenu = ButtonStates.Up;
    ButtonStates prevJump = ButtonStates.Up;

    void Start () {
        //TODO: actually create one of these objects from the Global object, so it doesn't get deleted between scene transitions.
	}

    void Update()
    {
        //Nothing super fancy here yet. Every frame, check for all our accepted user inputs, and set local flags based on that input.
        //The reason we do this in one central location is so that it's only done once per frame, 
        //and any changes to input only need to be applied in one file

        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        //Check if buttons are down right now
        if (Input.GetButton("Interact")) {
            interact = ButtonStates.Down;
        }else{
            interact = ButtonStates.Up;
        }

        if (Input.GetButton("Cancel")){
            cancel = ButtonStates.Down;
        }else{
            cancel = ButtonStates.Up;
        }

        if (Input.GetButton("Menu"))
        {
            menu = ButtonStates.Down;
        }else{
            menu = ButtonStates.Up;
        }

        if (Input.GetButton("Jump"))
        {
            jump = ButtonStates.Down;
        }else{
            jump = ButtonStates.Up;
        }

        //If buttons weren't previously down, but are now, we want to report a Pressed state
        //If they were down, but aren't now, report a Released state
        //If they were down, and still are, report a Held state
            //There is no state for remaining Up for multiple frames. 
            //This will be the default case acted upon when any checks for Pressed, Released, or Held fall through.
        
        //INTERACT
        if (interact == ButtonStates.Down){
            if (prevInteract == ButtonStates.Pressed || prevInteract == ButtonStates.Held){
                interact = ButtonStates.Held;
            }else{
                interact = ButtonStates.Pressed;
            }
        }else{
            if (prevInteract == ButtonStates.Pressed || prevInteract == ButtonStates.Held)
            {
                interact = ButtonStates.Released;
            }
        }

        //CANCEL
        if (cancel == ButtonStates.Down)
        {
            if (prevCancel == ButtonStates.Pressed || prevCancel == ButtonStates.Held)
            {
                cancel = ButtonStates.Held;
            }
            else
            {
                cancel = ButtonStates.Pressed;
            }
        }
        else
        {
            if (prevCancel == ButtonStates.Pressed || prevCancel == ButtonStates.Held)
            {
                cancel = ButtonStates.Released;
            }
        }

        //MENU
        if (menu == ButtonStates.Down)
        {
            if (prevMenu == ButtonStates.Pressed || prevMenu == ButtonStates.Held)
            {
                menu = ButtonStates.Held;
            }
            else
            {
                menu = ButtonStates.Pressed;
            }
        }
        else
        {
            if (prevMenu == ButtonStates.Pressed || prevMenu == ButtonStates.Held)
            {
                prevMenu = ButtonStates.Released;
            }
        }

        //JUMP
        if (jump == ButtonStates.Down)
        {
            if (prevJump == ButtonStates.Pressed || prevJump == ButtonStates.Held)
            {
                jump = ButtonStates.Held;
            }
            else
            {
                jump = ButtonStates.Pressed;
            }
        }
        else
        {
            if (prevJump == ButtonStates.Pressed || prevJump == ButtonStates.Held)
            {
                jump = ButtonStates.Released;
            }
        }

        //Finally, store the position of the buttons to be referenced next frame
        prevInteract = interact;
        prevCancel = cancel;
        prevMenu = menu;
        prevJump = jump;

    }   ///Update


}
