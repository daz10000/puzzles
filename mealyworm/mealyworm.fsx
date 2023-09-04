#r "nuget:Dmx.Amyris.Bio"
type State = { X : float ; Y : float ; Hops : int}
type Outcome = Soup | Floor
type Result = { Outcome : Outcome ; Hops : int }
let bowlX = 30.0
let bowlY = 40.0
let bowlR = 10.0
let tableX = 100.0
let tableY = 100.0
//let hopMean = 0.0
//let hopStdDev = 15.0
let hopMean = 10.0
let hopStdDev = 2.0

let rng = System.Random()
let inBowl x y =
    let dx = x - bowlX
    let dy = y - bowlY
    dx*dx + dy*dy < bowlR*bowlR

let offTable x y =
    x < 0.0 || x > tableX || y < 0.0 || y > tableY

let rec hop (s:State) =
    if inBowl s.X s.Y then {Outcome = Soup ; Hops = s.Hops}
    elif offTable s.X s.Y then {Outcome = Floor ; Hops = s.Hops}
    else
        let hopDistance = Amyris.Bio.math_stat.getNorm hopMean hopStdDev
        match rng.Next() % 4 with
        | 0 -> // North
            hop { s with Y = s.Y - hopDistance ; Hops = s.Hops + 1 }
        | 1 -> // South
            hop { s with Y = s.Y + hopDistance ; Hops = s.Hops + 1 }
        | 2 -> // East
            hop { s with X = s.X + hopDistance ; Hops = s.Hops + 1 }
        | 3 -> // West
            hop { s with X = s.X - hopDistance ; Hops = s.Hops + 1 }

let hop100k = Array.init 100000 (fun _ -> hop { X = tableX * rng.NextDouble() ; Y = tableY * rng.NextDouble() ; Hops = 0 })
let soupRuns,floorRuns =
    hop100k |> Array.partition (fun x -> x.Outcome = Soup)

let soupPercent = float soupRuns.Length / float hop100k.Length * 100.0
printfn $"Soup: {soupRuns.Length} ({soupPercent}%%) Floor: {floorRuns.Length} ({100.0 - soupPercent}%%) "
printfn $"Avg Soup Hops: {soupRuns |> Array.averageBy (fun x -> float x.Hops)}"
printfn $"Avg Floor Hops: {floorRuns |> Array.averageBy (fun x -> float x.Hops)}"
