open System
open System.Net.Http
let bias = 0.49
let trials = 1

let rng = System.Random()

let rec game trial cash probWinning =
    printfn $"trial={trial} cash={cash} probWinning={probWinning}"
    if cash = 0 then trial
    elif rng.NextDouble() <= probWinning then
        printfn "   win"
        game (trial+1) (cash+1) probWinning
    else
        printfn "   lose"
        game (trial+1) (cash-1) probWinning

[<EntryPoint>]
let main argv =

    let outcomes = [| for i in 1..trials -> {| Alice = game 1 100 bias ; Bob = game 1 100 (1.0-bias) |}|]
    let aliceAvgGames = outcomes |> Array.averageBy (fun x -> float x.Alice)
    let bobAvgGames = outcomes |> Array.averageBy (fun x -> float x.Bob)
    printfn $"Alice avg {aliceAvgGames} Bob avg {bobAvgGames}"
    0 // return an integer exit code