
open System

(*
Now that it is summer and prime scuba diving season, John at Palamedes Dive Company is trying to get a handle on their dive schedule. Using only the clues that follow, match each customer to their guide, dive date and location.

Mrs Ferrell won't dive  on Jul 16th
Mrs Blake wants to see Manta Bay

Mr Ayres will dive 2 days before customer going out with Muriel

Diver going out on Jul 17th isn't going to Manta Bay

The customer going out with Nadine will dive 1 day before the customer
headed to Porita Reef

Mr Ayres, the diver going out on July 14th, the diver going out with 
Sylvia and the diver going out on July 13th are all different divers

Neither Ms Chang nor the diver going out with Muriel is the diver headed to Rowe island
Ms Chang will dive 1 day after the diver going out with Lynn
Mrs Ferrell will dive 1 day before the customer headed to Queen's bench 

Jul13
Jul14
Jul15
Jul16
Jul17


Mr Ayres
Mrs Blake
Ms Chang
Mr Drake
Mrs Ferrll


Lynn
Muriel
Nadine
Sylvia
Vicki


Manta Bay
Nemo's ridge
Portia Reef
Queen's Bench
Rowe Island


*)

type Dates = Jul13 | Jul14| Jul15 | Jul16 | Jul17
type Instructors = Lynn | Muriel | Nadine | Sylvia | Vicki
type Locations = MantaBay|NemosRidge|PortiaReef|QueensBench|RoweIsland
type Customer =MrAyres|MrsBlake|MsChang|MrDrake|MrsFerrell

let format<'T> (x:'T []) = 
    let spaced = x |> Array.map (fun i -> i.ToString() |> sprintf "%-15s" )
    printfn "%s " (String.Join(",",spaced))

let shuffle<'T> (items:'T[]) (thenRun :'T []->unit)=
    let swap a b =
        let t = items.[a]
        items.[a]<-items.[b]
        items.[b]<-t

    let rec shuffleAt position =
        if position = items.Length then
            thenRun items
        else
            for i in position..items.Length-1 do
                swap position i
                shuffleAt (position+1)
                swap position i // swap back
    shuffleAt 0

let test() =
    shuffle [| 1;2;3|] (fun items -> printfn "%s" (String.Join(",",items)))

let get<'T when 'T:equality> (item:'T) (arr:'T[]) = Array.findIndex (fun x -> x=item ) arr

let generate() =
    [| Jul13 ; Jul14 ; Jul15 ; Jul16 ; Jul17|]  |> // no need to shuffle dates these are arbitrary
        (fun dates -> 
            shuffle [| Lynn ; Muriel ; Nadine ; Sylvia ; Vicki |] 
                (fun instructor ->
                    shuffle [| MantaBay;NemosRidge;PortiaReef;QueensBench;RoweIsland |]
                        (fun location ->
                            shuffle [| MrAyres;MrsBlake;MsChang;MrDrake;MrsFerrell |] 
                                (fun customer ->
                                    // Acceptable solution?
                                    let thirteenth = dates |> get Jul13
                                    let fourteenth = dates |> get Jul14
                                    let sixteenth = dates |> get Jul16
                                    let seventeenth = dates |> get Jul17

                                    let ayres= customer |> get MrAyres
                                    let blake = customer |> get MrsBlake
                                    let chang = customer |> get MsChang
                                    let ferrel = customer |> get MrsFerrell

                                    let manta = location |> get MantaBay
                                    let portia = location |> get PortiaReef
                                    let queen = location |> get QueensBench
                                    let rowe = location |> get RoweIsland

                                    let lynn = instructor |> get Lynn
                                    let muriel= instructor |> get Muriel
                                    let nadine = instructor |> get Nadine
                                    let sylvia = instructor |> get Sylvia

                                    if 
                                    // Mrs Ferrell won't dive  on Jul 16th
                                        ferrel <> sixteenth &&
                                    // Mrs Blake wants to see Manta Bay
                                        blake = manta  &&
                                    // Mr Ayres will dive 2 days before customer going out with Muriel
                                        ayres= muriel-2 &&
                                    // Diver going out on Jul 17th isn't going to Manta Bay
                                        location.[seventeenth] <> MantaBay &&
                                    // The customer going out with Nadine will dive 1 day before the customer
                                    // headed to Porita Reef
                                        nadine = portia-1 &&
                                    // Mr Ayres, the diver going out on July 14th, the diver going out with 
                                    // Sylvia and the diver going out on July 13th are all different divers
                                        ayres<> fourteenth &&
                                        ayres<> thirteenth &&
                                        ayres<> sylvia &&
                                        sylvia <> thirteenth &&
                                        sylvia <> fourteenth &&
                                    // Neither Ms Chang nor the diver going out with Muriel is the diver headed to Rowe island
                                        chang <> rowe &&
                                        muriel<> rowe &&
                                        muriel<> chang &&
                                    // Ms Chang will dive 1 day after the diver going out with Lynn
                                        lynn+1=chang &&
                                    // Mrs Ferrell will dive 1 day before the customer headed to Queen's bench 
                                        ferrel=queen-1
                                    then
                                        // acceptable
                                        printfn "---------------"
                                        format dates
                                        format instructor
                                        format location
                                        format customer
                                )
                        )
                )
        )

[<EntryPoint>]
let main argv =
    generate()
    0 // return an integer exit code