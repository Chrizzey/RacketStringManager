# RacketStringManager
This is an app to manage strining jobs of rackets. For each job, the following data is stored:

- The name of the player
- The type of racket to be strung
- The type of string to use
- The tension of the string
- The date on which the job was received
- A flag if the racket has been strung
- A flag if the job has been paid
- Additional comments regarding the job (e.g. if the racket is cracked)

## The App
The app starts up with a main view, wich contains a button to create a new job. Underneath is a list of jobs. The list can either display all jobs or just the pending and/or unpaid jobs. All jobs are sorted by the start date, with the most recent jobs on the top of the list.

When a new job is created, a list of past jobs will be shown. This will contain information about the last time(s) the racket has gotten a new string, including the date the racket was strung as well as the used string type and tension.

## Dev Topics
Display a list of available Rackets/Strings: https://docs.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups

## SQLite DB manipulation
To access the SQLite DB on the phone/emulator, follow these steps:

1. Open ADB command prompt
1. enter `adb root`
1. enter `adb shell`
1. use `cd` to navigate to `data/data/com.companyname.racketstringmanager/files/`
1. use `sqlite3 RacketStringManager.data.db` to gain access to the DB
1. use standard SQL queries to manipulate the DB. Tables are:
    - JobEntity
    - PlayerEntity
    - RacketEntity
    - StringEntity
