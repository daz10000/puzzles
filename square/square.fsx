
(*
From:
Greg Egan
@gregeganSF@mathstodon.xyz

Here’s a fun puzzle I heard from @octonion 

Pick two points uniformly at random in a square. What is the probability that the line that contains both points intersects two opposite edges of the square, rather than two adjacent edges?
*)

open System

let mutable oppositeEdges = 0
let mutable adjacentEdges = 0

let nTrials = 10000000

for i in 1 .. nTrials do
    let x1 = Random().NextDouble()
    let y1 = Random().NextDouble()
    let x2 = Random().NextDouble()
    let y2 = Random().NextDouble()

    let slope = (y2 - y1) / (x2 - x1)
    // got back to y at x=0
    let intercept = y1 - slope * x1

    let yAtX0 = intercept
    // move forward 1.0 to rhs intercept
    let yAtX1 = slope + intercept
    (*
      ^ 0,1 .............o 1,1
      |                  .
      |       x2,y2      .
      |                  .
      |                  .
      |   x1, y1         .
      |                  .
      |                  .
      o-------------------> 1,0
    *)


    if  (yAtX0 < 0.0 (* bottom *) && yAtX1 > 1.0 (* top *)) ||  // bottom to top positive slope
        (yAtX0 (* top *)> 1.0 && yAtX1 (*bottom*) < 0.0)  || // top to bottom negative slope
        (yAtX0 > 0.0 && yAtX0 <=1.0 && (yAtX1 >=0 && yAtX1 <= 1.0)) then
        oppositeEdges <- oppositeEdges + 1
    else
        adjacentEdges <- adjacentEdges + 1


printfn "Opposite edges: %d" oppositeEdges
printfn "Adjacent edges: %d" adjacentEdges
printfn "Probability of opposite edges: %f" (float oppositeEdges / float nTrials)