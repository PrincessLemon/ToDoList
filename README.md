# ToDoList 🗒️
Sooo here’s my todo-list! I’ve experimented quite a bit. This project includes, for example, encapsulation — I know it isn’t strictly needed for this project, but it was a fun little thing anywayyyy. I used encapsulation by keeping the task collection private inside the TaskManager and exposing functionality through public methods instead of letting other parts of the program modify the list directly. (Yes, that was a lot of words.)

I also explored when to use static and what changes when a class stops being static. For example, I initially used a static UI helper class for console-related logic and later experimented with making it non-static to understand how state and instances behave differently. This helped me better understand when static is appropriate and when it is not.

I’ve also been using LINQ a lot here, since it is a big part of the project: sorting tasks by date or project, filtering tasks by project… yeah, you get it 🙂

I implemented input handling and validation, with multiple layers of user input validation. This prevents empty task titles, ensures dates are valid, and prevents dates from being set in the past. I also added a fun feature for testing purposes where the app accepts the input "today" and "tomorrow" and converts them into real DateTime values. I also got a bit tired of having to use hyphens (I had to google that word — it is not called minus in this case xD) when inputting dates, so I added support for a more compact date format like yyyyMMdd. The app also supports Back (B) navigation across menus.

Another fun thing I’ve learned is file persistence! How to save and load data using text files so that the list stays saved between runs. The program reads lines from a file and splits the data into fields. This was really fun and useful! I realised a lot of people used JSON but I've opted out on that for this project, since is a console assignment, I just felt it was more fitting. It is less code as well and since this program already is a bit "code heavy" this was better.

This time I also added a UI (as much as possible with a console app lol). I created fun UI helper methods like ResetRainbow. 🌈 (I love a lot of colours if you can't tell)

The program is broken into multiple functional parts: Main Menu, Show Tasks Menu, Add Task, etc. I had a plan going into the project to structure it “properly”, and it is also easy to maintain that way.

And theeeen let’s talk about my extra flairs 🌟, because who doesn’t like those! One of the really fun things I did was making the app very colorful! ❤️🧡💛💚💙💜 I added a rainbow-coloured menu where numbers are printed in cycling colours using a predefined ConsoleColor[] palette. I’ve also added animated Christmas snow under the ASCII banner — it uses Random, and no two runs are ever the same (yey)! It uses Thread.Sleep for animation, and I think it creates a really nice seasonal theme. ☃️🎄

And maybe the most noticeable thing is the ASCII art banner. I know — it is very large and pretty! It is colorized per character, which is absolutely overkill but sooo cool!

A progress bar is also included to show how many tasks are completed vs. total. This is meant to motivate the user to actually get things done.

The UI logic was moved into a separate UI class because it took up a big part of the main class, and I wanted the code to look cleaner and more structured.

I also dealt with warnings such as nullable reference types and now understand why they appear. I hope this project is to your liking — it’s a bit 👾 old school 💾 with the banner and all that, but definitely fun! 🙂
