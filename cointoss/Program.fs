(*
@thienan496
I have just flipped 100 fair coins. Before I  I start revealing them to you one by one, you can 
ask me one yes/no question. Then, before I reveal each coin, you can make a Head/Tail guess: 
each correct guess gives you 1$. What's your question and strategy? #probability #maths

https://twitter.com/thienan496/status/1335534138959400961?s=20

*)

open System

let rng = Random()

let inline tossOne() = rng.Next()&&&1

type Player<'Q> = { Name : string ; Question:int [] -> 'Q ; Turn : 'Q->int[]->int->int ; Description : string}

type Outcome<'Q> = { Player : Player<'Q> ; Winnings : float ; Trials : int ; Time : float}

/// Number of coins
/// 
let N = 100
// Generate a set of N coins
let toss() =
    Array.init N (fun _ -> tossOne())

// One round with a player
let playOne<'Q> (player : Player<'Q>) =
    let tosses = toss()
    let q = player.Question tosses

    let answers = [| for i in 0..N-1 -> player.Turn q tosses i |]

    let score = Array.foldBack2 (fun a b count -> if a=b then count+1 else count) tosses answers 0
    score |> float

/// count rounds with a player, average results reported
let playN<'Q> count (player:Player<'Q>) =
    let began = DateTime.Now
    let result = [| for i in 1..count -> playOne player|] |> Array.average
    let ended = DateTime.Now
    { Player = player ; Winnings = result ; Trials = count ; Time = (ended-began).TotalSeconds}

/// Make a nice string from coin array
let coinString(coins:int []) = String.Join("",coins |> Array.map string)

// ============= PLAYERS ==============================================
let naive =
    { Name = "Naive" ; Question = (fun _ -> ()) ; Turn = (fun _ _ _ -> rng.Next()&&&1) ; Description = "Guess randomly, no question"}

let cheater =
    { Name = "Cheater (test)" ; Question = (fun _ -> ()) ; Turn = (fun _ prevTosses i -> prevTosses.[i]) ; Description = "Cheats - looks at correct answer (test)"}

type FirstCoin = int
/// 50.5
let first =
    {   Name = "First"  
        Question = (fun coins -> coins.[0] :FirstCoin) 
        Turn = fun firstCoin _prevTosses i -> if i=0 then firstCoin else tossOne()
        Description = "Ask for coin 0"
    }


type FirstSecondSame = { Same : bool ; Choice : int }

/// 50.5
let firstSecondSame =
    {   Name = "FirstSecondSame"  
        Question = (fun coins -> { Same = coins.[0]=coins.[1] ; Choice = tossOne() } ) 
        Turn = fun answers prevTosses i -> 
                    if i=0 then answers.Choice 
                    elif i=1 then 
                        match prevTosses.[0]=answers.Choice,answers.Same with
                        | true,true -> answers.Choice
                        | true,false -> answers.Choice^^^1
                        | false,true -> prevTosses.[0]
                        | false,false -> prevTosses.[0]^^^1
                    else tossOne()
        Description = "Are first and second same"
    }

type MoreHeadsThanTails = bool
let moreHTT =
    {   Name = "MoreHTT"  
        Question = (fun coins -> 
                        let heads,tails = coins |> Array.partition (fun i -> i&&&1=1)
                        heads.Length > tails.Length : MoreHeadsThanTails) 
        Turn = fun moreHTT prevTosses i -> 
                    if moreHTT then 1 else 0
        Description = "More heads than tails"
    }
type MoreHeadsThanTailsFirst99 = bool
let moreHTTFirst99 =
    {   Name = "MoreHThanTFirst99"  
        Question = (fun coins -> 
                        let heads,tails = coins.[..98] |> Array.partition (fun i -> i&&&1=1)
                        heads.Length > tails.Length : MoreHeadsThanTailsFirst99) 
        Turn = fun moreHTT prevTosses i -> 
                    //if i < 99 then
                        if moreHTT then 1 else 0
                    //else
                    //   tossOne()
        Description = "More heads than tails first 99"
    }

type LongestSequenceHeads = bool
let longestSequenceHeads =
    {   Name = "longestSequenceHeads"  
        Question = (fun coins -> 
                        let coinsString = coinString coins
                        let tails = coinsString.Split([|'1'|],StringSplitOptions.RemoveEmptyEntries) |> Array.maxBy (fun substring -> substring.Length)
                        let heads = coinsString.Split([|'0'|],StringSplitOptions.RemoveEmptyEntries) |> Array.maxBy (fun substring -> substring.Length)
                        heads.Length > tails.Length: LongestSequenceHeads
        )
        Turn = fun longestSequenceHeads prevTosses i -> 
                    if longestSequenceHeads then 1 else 0
        Description = "Is longest sequence heads - if yes play heads else play tails"
    }

type SevenMer = bool
let sevenMer =
    {   Name = "sevenMer"  
        Question = (fun coins -> 
                        let coinsString = coinString coins
                        coinsString.Contains("1111111") : SevenMer
        )
        Turn = fun hasMer prevTosses i -> 
                if hasMer then 1
                elif i>5 && ([| for j in i-6..i-1 -> prevTosses.[j]=1 |] |> Array.forall (id) ) then 0 
                else tossOne()
        Description = "Are there 7 consecutive H, if yes, bet all H, otherwise avoid strings of 7 H"
    }

type MoreHeadsFirst50 = bool
let moreHeadsFirst50 =
    {   Name = "moreHeadsFirst50"  
        Question = (fun coins -> 
                        let headsFirst50 = coins.[0..49] |> Array.countBy (fun coin -> coin = 1)
                        let headsSecond50 = coins.[50..] |> Array.countBy (fun coin -> coin = 1)
                        headsFirst50>headsSecond50 : MoreHeadsFirst50
        )
        Turn = fun moreFirst50 _prevTosses i -> 
                match i<50,moreFirst50 with
                | true,true -> 1
                | true,false -> 0
                | false,true -> 0
                | false,false -> 1
        Description = "Are there more heads in first 50? If yes, H for first 50 then tails for second 50"
    }


// MAIN ===============
[<EntryPoint>]
let main argv =

    printfn "Strategy               Winnings    Time   Trials     Description"
    printfn "================================================================================================="
    let report result =
        printfn "%-20s   %5.5f    %5.2f  %-10d %s" result.Player.Name result.Winnings result.Time result.Trials result.Player.Description

    let t = 100000 // can drop this by 10x and stil get pretty good approximations

    // playN t cheater |> report //  100.0 (test perfect knowledge)
    playN t naive |> report // 50.0
    playN t first |> report // 50.5
    playN t firstSecondSame |> report // 50.5
    playN t moreHTT |> report // 53.97
    playN t moreHTTFirst99 |> report //53.97
    playN t longestSequenceHeads |> report // 52.5
    playN t sevenMer |> report //  50.942
    playN t moreHeadsFirst50 |> report //  50.506

    0 