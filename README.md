## Conway's Game of Life (built on Gui.cs)

This is a (work in progress) implementation of [Conway's Game of Life](https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life), written using dotnet core and the excellent [Gui.cs](https://github.com/migueldeicaza/gui.cs) library by [Miguel de Icaza](https://github.com/migueldeicaza)

## Why?

I'd been looking for an excuse to play with Gui.cs.  My first computer had a lot of console based games and applications, including an implementation of Conway's Game of Life, and I thought it would be fun to create something similar.  

In addition to wanting to do something with Gui.cs, I had been thinking a lot about creating an implementation of Life after a recent discussion with some of my teammates about different ways to implement the game.  I'd wanted to try an implementation that wasn't set on a fixed board size, and so creating this gave me the opportunity to scratch that itch as well.

## Progress
[x] Initial setup with Gui.cs
[x] Create game board widget
[x] Animate the game board
[x] Controls to start and stop animation
[x] Ability to move cursor around game board to edit it
[ ] Ability to toggle cells on and off
[ ] Add widgets to display stats and health
[ ] Add widgets to control viewport
[ ] Add proper game of life processing
[ ] Ability to add preset entities to board