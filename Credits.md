# Credits
I couldn't have done this conversion and update for h3net
as easily without the following:

* [H3](https://gihub.com/uber/h3) - The actual source code that I've foolishly
  converted to C#.  I actually use h3net for another project on mobile, which
  is why h3net exists, and partly why I haven't updated this in a while.
  
  My first iteration of _that_ project used generated data sets from
  [DGGRID](https://www.discreteglobalgrids.org/) which was not something I
  enjoyed since the project uses real time location data, and generating the
  hexagons in a 100 km^2 area polygon was at a resolution of ~100 m^2 was an
  unpleasant task at the time.
  
  Not to mention the unit tests!  Jumping the gun from 3.1.1 to 3.7.1, along
  with an architecture change led to a scary amount of code to examine.
  Without having some idea what to expect, I would have probably given up and
  limped along with my initial 3.1.1 implementation.

* [H3Explorer](https://h3explorer.com/) - The visual debugger to make sure the
  new code worked right or figure out where the unit tests were blowing up.
  An excellent way to see that modifying Algos.kRingDistances to an extension
  method on H3Index and not converting the supporting functions correctly, as
  well, caused it to create a wandering serpent for k values greater than 1 is
  fantastic when you can _see_ how it's messing up.

* [browserling](https://www.browserling.com/tools) -
  Having to deal with lists of numbers sometimes in hex, sometimes in decimal,
  and all in 64 bit unsigned integers.  Copy/paste, click a button, copy and
  compare against what you're expecting.  I mostly used
  [decimal to hex](https://www.browserling.com/tools/dec-to-hex) but there's a
  lot of other tools there.

* The usual suspects, as I need music to code.
  * Melissa Etheridge
  * Electric Light Orchestra
  * Ben Folds
  * Billy Joel
  * Meat Loaf
  * Lin-Manuel Miranda
  * Simon and Garfunkel  
  * Many musicals, including the frequently played:
    * Avenue Q
    * Beetlejuice
    * The Book Of Mormon
    * Dear Evan Hansen
    * Hadestown
    * Hamilton
    * Heathers
    * In The Heights
    * Jersey Boys
    * Mean Girls
    * Waitress
  
* As always, my family for accepting that I'm going to spend a few
  hours poking at a keyboard for days on end, and it's not even going
  to make cool pictures or be a video game.

* Mandy Oei, for reminding me that I need to do things before it's too late.
