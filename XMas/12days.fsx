["First"; "Second"; "Third"; "Fourth"; "Fifth"; "Sixth"; "Seventh"; 
    "Eighth"; "Ninth"; "Tenth"; "Eleventh"; "Twelfth"]
|> List.mapi (fun day dayName ->
    """a Partridge in a Pear Tree
    two Turtle Doves
    three French Hens
    four Calling Birds
    five Gold Rings
    six Geese a Laying
    seven Swans a Swimming
    eight Maids a Milking
    nine Ladies Dancing
    ten Lords a Leaping
    eleven Pipers Piping
    twelve Drummers Drumming""".Split('\n') 
    |> Array.map (fun x -> x.Trim())
    |> Array.toList
    |> List.mapi (fun i gift -> (i, gift))
    |> List.take (day + 1)
    |> List.rev
    |> List.fold (fun acc (giftIdx, giftName) ->
        acc@[ match giftIdx, day with
                | 0, 0 -> sprintf "%s." giftName
                | 0, _ -> sprintf "and %s." giftName
                | _, _ -> sprintf "%s," giftName ]
    ) [$"\nOn the {dayName} day of Christmas my true love gave to me:"]
) |> List.concat |> String.concat "\n" |> System.Console.WriteLine