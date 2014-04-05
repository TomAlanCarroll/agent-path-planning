agent-path-planning
====================
This program simulates agent path planning using A* and Q-Learning in a 2D grid.

# Demo Video
<a href="http://www.youtube.com/watch?feature=player_embedded&v=LuxOfLlQ530" target="_blank"><img src="http://img.youtube.com/vi/LuxOfLlQ530/0.jpg" alt="Watch a demo on YouTube" width="240" height="180" /></a>

# How to Run
1. Verify the .NET framework version 4.5 or higher is installed.
2. Run in either "Debug" or "Release" mode in Visual Studio.
3. Click the button "Load Grid File…" and select a well-formed CSV file. For the purposes of this assignment, a well-formed CSV file will have exactly 10 lines with each line containing 9 commas. Quotation marks should not be used in a file. The symbol definitions are:

  | Symbol | Meaning                     |
  |:------:|:---------------------------:|
  | 0      | An obstacle in the 2D grid. |
  | 1      | The agent in the 2D grid.   |
  | 2      | The reward in the 2D grid.  |
  Here is an example of a well-formed grid map CSV file with various obstacles. The agent is in the top left corner and the reward is in the bottom right corner:
  ```
  1,,,,,,,,0,
  ,0,,0,,,,,,
  ,,,0,,0,,0,0,0
  ,,,0,,,,,,0
  ,,0,,,,,,,0
  ,,,,,0,,,,0
  ,,,0,,0,,0,,
  ,0,,0,,0,,0,,
  ,,,0,,0,,0,,
  ,,,0,,,,0,,2
  ```
4. Select either "Q-Learning" or "A\*" and click "Start". The default algorithm is A\*.
5. For A*, the best path is found and will be automatically displayed after the search. For Q-Learning, the Q-Table is trained. After training, click on a cell to have the agent navigate the grid from the clicked cell to the reward using the max exploration policy.
