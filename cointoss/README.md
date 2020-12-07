# Coin toss problem 

```
@thienan496
I have just flipped 100 fair coins. Before I  I start revealing them to you one by one, you can 
ask me one yes/no question. Then, before I reveal each coin, you can make a Head/Tail guess: 
each correct guess gives you 1$. What's your question and strategy? #probability #maths
```
[https://twitter.com/thienan496/status/1335534138959400961?s=20](https://twitter.com/thienan496/status/1335534138959400961?s=20)


### Comparison of players

    Strategy               Winnings  Time   Trials     Description
    =================================================================================================
    Naive                  50.003    13.10  1000000    Guess randomly, no question
    First                  50.498    12.47  1000000    Ask for coin 0
    FirstSecondSame        50.503    13.00  1000000    Are first and second same
    MoreHTT                53.979    11.18  1000000    More heads than tails
    MoreHThanTFirst99      53.985    11.45  1000000    More heads than tails first 99
    longestSequenceHeads   52.488    19.09  1000000    Is longest sequence heads - if yes play heads else play tails
    sevenMer               50.944    51.66  1000000    Are there 7 consecutive H, if yes, bet all H, otherwise avoid strings of 7 H
    moreHeadsFirst50       50.504    18.94  1000000    Are there more heads in first 50? If yes, H for first 50 then tails for second 50
    
    
### More resolution (trials) on two top strategies
    Strategy               Winnings    Time   Trials     Description
    =================================================================================================
    MoreHTT                53.98294    88.90  10000000   More heads than tails
    MoreHThanTFirst99      53.98149    104.76 10000000   More heads than tails first 99
