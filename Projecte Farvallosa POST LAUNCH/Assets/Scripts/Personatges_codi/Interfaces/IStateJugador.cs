using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IStateJugador
    {
        //void StateUpdate(Controlador_Jugador cj);
        void StateUpdate(DetectiuStateManager cj);
        //IStateJugador CanviaState(Controlador_Jugador cj);
    }
}

/*public interface IStateJugador
{
    void Maneja(Controlador_Jugador cj);
    IStateJugador CanviaState(Controlador_Jugador cj);
}*/
