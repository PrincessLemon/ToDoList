# ToDoList 🗒️
Sooo here's my todo-list! I've experimented a fair bit. This project includes, for example, Encapsulation — I know it isn't needed for this project, but it was a fun little thing anywayyyy. I used encapsulation by keeping the task collection private inside the TaskManager and exposing functionality through public methods instead of letting other parts of the program modify the list directly. (yes that was a lot of words)

I also explored when to use static and what changes when a class stops being static. For example, I initially used a static UI helper class for console-related logic and later experimented with making it non-static to understand how state and instances behave differently. This helped me better understand when static is appropriate and when it is not.

I've also been using LINQ a lot here since it is a big part of the project: sorting tasks by date or project, filtering tasks by project… yeah, you get it :)

I have Input Handling and Validation — multiple layers of user input validation. This is to prevent empty task titles, ensure dates are valid, and again, like before, prevent dates from the past. I also added a fun thing for testing purposes where it accepts the input "today" and "tomorrow" and converts it into real DateTime values. I also got a bit tired of having to use hyphens(I had to google that word, it is not called minus in this case xD) when inputting dates, so I implemented support for a more compact date format like yyyyMMdd. I've also made sure that it supports Back (B) navigation across menus.

Another fun thing I've learned is File Persistence! How to save and load data using text files so that the list stays saved. It reads lines from a file and splits data into fields. This was really fun and useful!

This time I added a UI (as much as possible with a console app lol). Plus, I added fun and friendly UI helper methods like ResetRainbow. 🌈 

The program is also broken into multiple functional parts: Main Menu, Show Tasks Menu, Add Task, etc. I had a plan going into the project to structure it "properly", and it is also easy to maintain that way.

And theeeen let's talk about my extra "flairs" 🌟, because who doesn't like those! One of the really fun things I did was making the app very colorful! ❤️🧡💛💚💙💜 :D I added a rainbow-coloured menu, and numbers are printed in cycling colours using a predefined ConsoleColor[] palette. I've also added “animated” Christmas snow under the ASCII banner — it uses Random, and no two runs are ever the same(yey)! It uses Thread.Sleep for animation, and I think I created a really nice seasonal theme. ☃️🎄

And maybe the most noticeable thing is probably the ASCII Art Banner. I know — it is very large and pretty! It is colorized per character, overkill but sooo cool!

A progress bar is also added to show how many tasks are completed vs. total. This is to motivate the user to get the tasks done!

The UI logic had to be moved into a separate UI class because it took up a big part of my main class, and I wanted the code to look more clean and structured.

Also, I dealt with warnings like nullable properties and understand why they appear. I hope this project is to your liking — it is a bit 👾 old school 💾 with the banner and stuff, but fun! :)
