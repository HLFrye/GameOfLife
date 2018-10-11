namespace LifeLib

open LifeInterface

module Life =
    type LifeGame() =
        let mutable points = List.empty<(int * int)>

        interface ILifeGameInterface with
            member this.Get (x: int, y: int) = 
                points
                |> List.contains (x, y)
            member this.Add (x: int, y: int) =
                let updatedList = 
                    (x, y) :: points
                points <- updatedList
            member this.Remove(x: int, y: int) = 
                let filteredList = 
                    points
                    |> List.except [(x, y)]
                points <- filteredList
            member this.Update() = true
            member this.CellCount with get() = 
                points
                |> List.length

