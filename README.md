# Kessel Sabacc Unity Clone

![Screenshot](./Docs/Art/KesselSabaccRepoBanner.png)

This project contains a Unity implementation of Kessel Sabacc, the card game featured in [Star Wars: Outlaws](https://en.wikipedia.org/wiki/Star_Wars_Outlaws). Kessel Sabacc is one of my favorite activities to play in the game. I often went out of my way to play NPCs in Sabacc rather than completing the actual mission at hand.

## Planned Features

- [x] Standard Kessel Sabacc Rules
- [ ] Extensible AI Controllers
- [ ] Shift Tokens

## How to Play

A build of the game is available to play on: [itch.io](https://shijbey.itch.io/kessel-sabacc).

Alternatively, you can download the code and build the game for the desktop.

## Rules

Kessel Sabacc is played with 2-4 players competing to be the last player with chips. The game is played using two decks of cards, shift tokens, and a collection of chips. Kessel Sabacc cards have two suits, blood (red) and sand (orange). Players take turns spending chips to draw cards in the hope of attaining the hand with the lowest score. At the end of each round, players are taxed chips based on the score of their hand. The last player with remaining chips wins the game.

### Game Setup

The game starts by shuffling the Blood and Sand decks and dealing hands to players. Players are also given a starting number of chips (usually 4-8).

### Turns and Gameplay

Sabacc is played across multiple rounds. Each round, the blood and sand decks are shuffled, any discarded cards are added back to their respective draw decks, and players are dealt new hands. Each round is divided into 3 turns. During their turn, a player may play a shift token, draw a card, or stand.

### Drawing Cards and Standing

Drawing a card costs one chip. This is called investing a chip. If a player chooses to draw a card, they can choose the top card from either the decks or the face-up discard piles. Then, they must discard either the card they drew or the card in their hand from the matching suit. Players will always end their turns with at least one card of each suit.

If a player chooses not to draw a card, then they "stand" for this turn. Choosing to stand does not forfeit remaining turns during a round. Players may choose to draw or stand during future turns.

### End of Round

At the end of the round, players reveal their cards. If a player's hand included one or more Impostor cards, random values will be assigned to the cards based on a dice roll. Players roll two six-sided dice and must select one of the two values to assign to the Impostor card.

### Scoring Hands

Players strive to decrease the difference between the values of the two cards in their hands. The closer the difference is to zero, the stronger the hand. If a player has two cards of the same value, they have a _sabacc_ hand. If a player has two Sylop cards in their hand, they have a _pure sabacc_ hand. If the player has one Sylop card in their hand, this is called a _Sylop sabacc_ hand. Pure sabacc is the strongest hand in the game, followed by Sylop sabacc and sabacc. If two players have sabacc hands, the player with the lower-valued cards has the better hand (e.g., two 3s beats two 4s).

### Chip Penalties (Imperial tax) and Ties

The player who wins the round is refunded all chips invested this round from drawing cards.

Players who do not win the round are taxed an amount of chips equal to the difference between their card values. Players who lost while holding any sabacc hand are taxed 1 chip. All taxed chips are lost for the remainder of the game.

In the case of a tie, all tied players are refunded their invested chips for this turn.

If more than one player has chips remaining, a new round starts with the next eligible player to the left of the player who started the last round.

## Legal Disclaimer

Star Wars and its associated IP are the property of Lucasfilm Ltd.
