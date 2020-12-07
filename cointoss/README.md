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
    moreHeadsFirst50       50.504    18.94  1000000    Are there more heads in first 50? If yes, H for first 50 then tails for second 50

    4mer                   50.23143  17.00  1000000    Are there 4 consecutive H, if yes, bet all H, otherwise avoid strings of 4 H
    5mer                   50.86667  19.88  1000000    Are there 5 consecutive H, if yes, bet all H, otherwise avoid strings of 5 H
    6mer                   51.13263  32.45  1000000    Are there 6 consecutive H, if yes, bet all H, otherwise avoid strings of 6 H
    7mer                   50.94310  41.14  1000000    Are there 7 consecutive H, if yes, bet all H, otherwise avoid strings of 7 H
    8mer                   50.62342  50.60  1000000    Are there 8 consecutive H, if yes, bet all H, otherwise avoid strings of 8 H
    9mer                   50.38017  57.12  1000000    Are there 9 consecutive H, if yes, bet all H, otherwise avoid strings of 9 H
    10mer                  50.20701  71.63  1000000    Are there 10 consecutive H, if yes, bet all H, otherwise avoid strings of 10 H
    11mer                  50.12595  73.88  1000000    Are there 11 consecutive H, if yes, bet all H, otherwise avoid strings of 11 H

    longestSequenceHeads   52.488    19.09  1000000    Is longest sequence heads - if yes play heads else play tails
    HTH>HTT                52.80190  20.86  1000000    Are head to head transitions > head to tail?
    HTH>HTT_v2             52.82495  21.07  1000000    Are head to head transitions > head to tail? different null strategy
    MoreHThanTFirst99      53.985    11.45  1000000    More heads than tails first 99
    MoreHTT                53.979    11.18  1000000    More heads than tails

    
    
### More resolution (trials) on two top strategies
    Strategy               Winnings    Time   Trials     Description
    =================================================================================================
    MoreHTT                53.98294    88.90  10000000   More heads than tails
    MoreHThanTFirst99      53.98149    104.76 10000000   More heads than tails first 99


### Analytical R solution for MoreHTT

Gives 53.979 (courtesy Finn VK)

```
outcome_probabilities <- 0:100 %>% sapply(function(n) dbinom(n, 100, 0.5))
odds_more_tails_or_even <- outcome_probabilities[1:51] %>% sum ## stupid 1-indexing
odds_more_heads <- outcome_probabilities[52:101] %>% sum
more_tails_or_even_outcomes_weighted_odds <- 0:50 * outcome_probabilities[1:51]
more_tails_or_even_EV <- (more_tails_or_even_outcomes_weighted_odds %>% sum) / odds_more_tails_or_even
more_heads_outcomes_weighted_odds <- 51:100 * outcome_probabilities[52:101]
more_heads_EV <- (more_heads_outcomes_weighted_odds %>% sum) / odds_more_heads
(100 - more_tails_or_even_EV) * odds_more_tails_or_even + more_heads_EV * odds_more_heads
```
