

// Actually easier to fold cube in reverse order
let fullSnake =[3;3;3;3;2;2;2;3;3;2;2;3;2;3;2;2;3] |> List.rev

// ensure we entered it correctly
assert(List.sum fullSnake = 43)

/// legal folding directions
type Direction=
    |UP
    |DOWN
    |LEFT
    |RIGHT
    |FORWARD
    |BACKWARD

/// coordinate system, z is up, 0,0,0 is bottom left front side
type Position={x:int;y:int;z:int}

/// basic directions and coordinate shifts
let move(p:Position) (d:Direction)=
    match d with 
    |UP-> {p with z=p.z+1}
    |DOWN->{p with z=p.z-1}
    |RIGHT->{p with x=p.x+1}
    |LEFT->{p with x=p.x-1}
    |FORWARD->{p with y=p.y+1}
    |BACKWARD->{p with y=p.y-1}

/// logger with depth awareness for indeting
let log n s = 
    printfn "%-4d %s%s" n (String.replicate n "  ") s
/// have we moved outside the legal 3x3x3 cube?
let outOfBounds (p:Position) = p.x<0 || p.x>2 || p.y<0 || p.y>2 || p.z<0||p.z>2

/// Try to move from p in position d n steps
/// return Some new position or None
let rec tryMoveN (used:bool[,,]) p d n =
    if n = 0 then Some p else 
        let newPos = move p d
        if outOfBounds newPos then None
        elif used.[newPos.x,newPos.y,newPos.z] then None
        else
            tryMoveN used newPos d (n-1)

/// Fill in a move we just made
let rec fillMoveN depth (used:bool[,,]) p d n =
    if n = 0 then ()
    else
        let newPos = move p d
        sprintf "filling x=%d y=%d z=%d" newPos.x newPos.y newPos.z |> log depth
        used.[newPos.x,newPos.y,newPos.z] <- true
        fillMoveN depth used newPos d (n-1)

/// untrace a move we made
let rec unFillMoveN depth (used:bool[,,]) p d n =
    if n = 0 then ()
    else
        let newPos = move p d
        sprintf "unfilling x=%d y=%d z=%d" newPos.x newPos.y newPos.z |> log depth
        used.[newPos.x,newPos.y,newPos.z] <- false
        unFillMoveN depth used newPos d (n-1)

/// how can we move next given a current direction
let legalMoves (d:Direction)=
    match d with 
    |UP
    |DOWN->[LEFT;RIGHT;FORWARD;BACKWARD]
    |LEFT
    |RIGHT->[UP;DOWN;FORWARD;BACKWARD]
    |FORWARD
    |BACKWARD->[LEFT;RIGHT;UP;DOWN]

/// main search routine.  Given an array tracking fillwed positions, current position and direction.
/// remaining parts of the snake we want to lay out and historical moves as path in LIFO order
let rec search depth (filled:bool [,,]) (p:Position) (d:Direction) (snake:int list) (path:Direction list) =
    match snake with
    | [] -> // no snake left! we are done (we decide to detect this one step earlier, so not used)
        failwithf "unreachable"
    | hd::tl ->
        // Take next snake segment and consider laying it down in this direction
        // A segment 3 long moves 2,  a segment 2 long moves 1 etc, since we are standing on first position
        sprintf "considering moving from x=%d y=%d z=%d in d=%A" p.x p.y p.z d |> log depth
        match tryMoveN filled p d (hd-1) with
        | Some newPosition when outOfBounds newPosition ->
            sprintf "out of bounds" |> log depth
            () // nothing to do
        | None ->
            () // can't move that direction
            sprintf "no legal move" |> log depth
        | Some legalNewPosition ->
            sprintf "moving to x=%d y=%d z=%d" legalNewPosition.x legalNewPosition.y legalNewPosition.z  |> log depth
            fillMoveN depth filled p d (hd-1)
            sprintf "filling move" |> log depth
            if List.isEmpty tl then
                printfn "Done!"
                printfn "Directions: %A" (List.rev path)
                exit 1
            for newDirection in legalMoves d do
                sprintf "trying direction %A" newDirection |> log depth
                search (depth+1) filled legalNewPosition newDirection tl (newDirection::path)
                sprintf "returning from direction %A" newDirection |> log depth
            sprintf "unfilling move" |> log depth
            unFillMoveN depth filled p d (hd-1)



[<EntryPoint>]
let main argv =
    printfn "Hi - starting!"
    printfn "The time is %s" (System.DateTime.Now.ToLongTimeString())

    let filled = Array3D.init 3 3 3 (fun _ _ _ -> false)
    for x in 0..2 do
        for z in 0..2 do
            for d in [FORWARD; LEFT;RIGHT;UP;DOWN] do
                sprintf "Starting in position x=%d y=%d z=%d direction=%A" x 0 z d |> log 0
                filled.[x,0,z]<-true
                search 0 filled {x=x;y=0;z=0} d fullSnake [d]
                filled.[x,0,z]<-false
    printfn "Final array=" 
    for x in 0..2 do
        for y in 0..2 do
            for z in 0..2 do
                printf "%A" (filled.[x,y,z])

    0 // return an integer exit code
