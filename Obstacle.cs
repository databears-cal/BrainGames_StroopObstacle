using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Description: Inteface for every Obstacle to inherit from.
 * Author: Revekka Kostoeva
 * Updated: October 2nd, 2019
 */
public interface Obstacle
{
    /* Initializes all obstacle assets and starts obstacle */
    void startGame();

    /* Returns the int corresponding to the current state of the game.
     * -1 - hasn't started
     * 0 - ongoing
     * 1 - user won
     * 2 - user lost
    */
    int gameState();

    /* Destroys allgameobjects associated with obstacle and ends game */
    void endGame();
}
