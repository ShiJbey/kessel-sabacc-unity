---
title: Recreating Kessel Sabacc in Unity
date: 03-26-2025
---

# Recreating Kessel Sabacc in Unity

Earlier this year, I purchased Star Wars: Outlaws, a recent expansion of the Star Wars video game universe that follows the misadventures of Kay Vess and her companion Nix. Aside from spamming all the Crimson Dawn missions (because they have the best faction clothes), my favorite part of the game has been playing Kessel Sabacc.
s
Kessel Sabacc is a card game somewhat akin to Black Jack where the player tries their luck at gaining the best valued hand against their opponents. The rules feel intimidating at first, but in reality the game is very easy to pick-up.

So, inspired by my new favorite card game and the want to procrastinate on dissertation writing, I decided to try and recreate the Kessel Sabacc as a multiplayer game in Unity.

**Disclaimers**: Most of the designs and art are adapted directly from Star Wars: Outlaws. I do not own the copyright to any of the source materials. Thank you to the folks at Ubisoft and Massive Entertainment for making such a fun game.

## Designing the UI

The first thing I did when making this game was sketch out the user interface. I'm generally more drawn to designing game systems and code architecture, but it was important for me to ensure the UI received just as much love. The design I decided to go with is a simplified combination of the original Sabacc UX and the UI from Ubisoft's Uno game on PS5.

I consider this game as having two sets of UI components: out-of-match and in-match UI. The out-of-match UI includes the main menu and all UI component responsible for helping the player configure the game, start/find matches, and adjust settings. The in-match UI needs to communicate the current state of the game (from a single player's perspective) and update accordingly across all players when players perform actions.

I also started considering game pad support during this time. I think that it is important to support players navigating game UI without a mouse and keyboard. This is something that web browsers usually handle well by default. For this game, I wanted to support using a gamepad to play. Thus, I needed to add selection highlights and potentially animations to make it clear to the player what UI element they have selected. Additionally, I considered placing button hints on screen to help with navigation and keyboard/button shortcuts.

## Designing the Cards and Shift Tokens

The visual design for the cards maintains most of the original design from Star Wars: Outlaws. The main design difference is swap the symbols on the cards to make them easier to read. The original cards are kind of hard to read. The numbers at the top are not clear unless you search them beforehand or have played multiple games to learn them. So, the first thing I wanted to do was bring them a little more down to earth by pulling elements from Uno. This is evident in the large numbers/symbols centered on the cards. Players should be able to quickly evaluate their hands without any guess work.

Shift tokens remain consisten between the source material and this adaptation. 

## Multiplayer Server Architecture




