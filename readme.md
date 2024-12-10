# KnightKnight API
Objective of this project is to create a full stack "simple" crud app using dotnet in visual studio code / docker.
Mostly for learning, but also for being able to keep track of games played, victors, and project probabilities based off this data.

## Here's some guidance for how to use this

DTOs have request / responses for the API portion of this app. Use them!

This is running swagger in development -- when you add a new endpoint, make sure to add in an associated exmple in the Configuration/SwaggerExamples.cs portion of the code

Run `dotnet watch --release-profile watch` to spin it up in localhost:5000 and have it auto go to /swagger