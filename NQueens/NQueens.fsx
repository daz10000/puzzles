

let startTime = System.DateTime.Now
let N = 15
printfn "N = %d" N
let colOccupied = Array.create N false
let rowOccupied = Array.create N false
let diag1Occupied = Array.create (2 * N - 1) false
let diag2Occupied = Array.create (2 * N - 1) false
type Pos = { row: int; col: int }

/// For some rows, the queen positions are fixed - a solution must use these
let fixedPositions = Array.create N -1

fixedPositions.[0] <- 0
fixedPositions.[1] <- 2
fixedPositions.[2] <- 4

let queenPositions = Array.create N { row = -1; col = -1 }
let printBoard () =
    for row in 0 .. N - 1 do
        for col in 0 .. N - 1 do
            if queenPositions.[row].col = col then
                printf "Q "
            else
                printf ". "
        printfn ""
    printfn ""

let isValid row col =
    not colOccupied.[col] && not rowOccupied.[row] && not diag1Occupied.[row + col] && not diag2Occupied.[row - col + N - 1]

/// Solve for the nth queen / row
let rec solve n =
    if n = N then // solution found - ran off bottom of the board
        printfn "%A" queenPositions
        printBoard ()
        let elapsed = (System.DateTime.Now - startTime).TotalSeconds
        printfn $"Elapsed time: {elapsed} seconds" 
        exit 0
    else
        if fixedPositions.[n] <> -1 then
            // This row has a fixed position
            let col = fixedPositions.[n]
            if isValid n col then
                colOccupied.[col] <- true
                rowOccupied.[n] <- true
                diag1Occupied.[n + col] <- true
                diag2Occupied.[n - col + N - 1] <- true
                queenPositions.[n] <- { row = n; col = col }
                solve (n + 1)
            // otherwise return
        else
            // For this row, try every column
            for col in 0 .. N - 1 do
                if not colOccupied.[col] && not rowOccupied.[n] && not diag1Occupied.[n + col] && not diag2Occupied.[n - col + N - 1] then
                    queenPositions.[n] <- { row = n; col = col }
                    colOccupied.[col] <- true
                    rowOccupied.[n] <- true
                    diag1Occupied.[n + col] <- true
                    diag2Occupied.[n - col + N - 1] <- true
                    solve (n + 1)
                    colOccupied.[col] <- false
                    rowOccupied.[n] <- false
                    diag1Occupied.[n + col] <- false
                    diag2Occupied.[n - col + N - 1] <- false

solve 0
printfn "No solution :( "