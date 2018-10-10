namespace LifeLib

open LifeInterface

module Life =
    type LifeGame() =
        interface ILifeGameInterface with
            member this.Get (x: int, y: int) = true
            member this.Add (x: int, y: int) = ()
            member this.Remove(x: int, y: int) = ()
            member this.Update() = true
            member this.CellCount with get() = 0  
