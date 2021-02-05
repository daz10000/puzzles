open System
let data =
    """
    CLAWCWCC
    CLAWLCLL
    LWACAAAA
    ACLWWLWL
    WACWACAW
    CWACLAWC
    ALWLAWCL
    CALACLAW
    """.Split([|'\n'|]) 
        |> Array.choose(fun line -> 
                        match line.Trim() with
                        | "" -> None
                        | x -> Some (x.ToCharArray())
        )
let deltas = [|for xd in -1..1 do
                for yd in -1..1 do
                    if xd<>0 || yd<>0 then yield {|XD = xd ; YD= yd|}
                |]

assert(deltas.Length = 8)
let dim = 8
let word = "CLAW"
let wordLength = word.Length
for x in 0..dim-1 do
    for y in 0..dim-1 do
        for direction in deltas do
            if [|0..wordLength-1|] 
                |> Array.forall (fun step ->
                                    let x' = x+step*direction.XD
                                    let y' = y+step*direction.YD
                                    x'>=0 && x'<dim && y'>=0 && y'<dim && data.[y'].[x'] = word.[step]
                                )
            then
                printfn "Position %d %d, xd=%d yd=%d" x y direction.XD direction.YD