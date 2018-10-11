namespace LifeLib

open LifeInterface

// Births: Each dead cell adjacent to exactly three live neighbors will become live in the next generation.
// Death by isolation: Each live cell with one or fewer live neighbors will die in the next generation.
// Death by overcrowding: Each live cell with four or more live neighbors will die in the next generation.
// Survival: Each live cell with either two or three live neighbors will remain alive for the next generation.

// Definition: a neighbor is any live cell in the 8 space "neighborhood", including diagonals

module Life =

    let private numLiveNeighbors points (x, y) = 
         [ (-1, -1);
           ( 0, -1);
           ( 1, -1);
           (-1,  0);
           ( 1,  0);
           (-1,  1);
           ( 0,  1);
           ( 1,  1) ]
        |> Seq.map (fun (neighborX, neighborY) -> (x + neighborX, y + neighborY))
        |> Seq.filter (fun pt -> Seq.contains pt points)
        |> Seq.length

    let private applyRules points =
        let mutable survivors = List.empty<(int * int)>
        let mutable adjacentDeadCells = Map.empty<(int * int), int>
        for point in points do
            let (liveNeighborCount, deadNeighbors) = numLiveNeighbors points point
            match liveNeighborCount with
            | 2
            | 3 -> survivors <- point :: survivors
            | _ -> ()

            match (adjacentDeadCells |> Map.containsKey point) with
            | false -> adjacentDeadCells <- Map.add point 1 adjacentDeadCells
            | true -> 
                let adjacentCount = 
                    adjacentDeadCells
                    |> Map.find point
                    |> (+) 1
                adjacentDeadCells <- 
                    adjacentDeadCells
                    |> Map.remove point
                    |> Map.add point adjacentCount

        let births = 
            adjacentDeadCells
            |> Map.filter (fun _ count -> count = 3)
            |> Map.toSeq
            |> Seq.map (fun (pt, _) -> pt)
            |> Seq.toList

        survivors 
        |> List.append births        


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

