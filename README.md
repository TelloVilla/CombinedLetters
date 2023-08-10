# Letter Combiner Console App

## Description

An application that will go though an Input folder and match together admission and scholarship letters based on student ids and combining them into a single file to save paper

## Test Generator Command

Creates date folders in the input folder for Admissions and Scholarships across 30 days. Generates random student ids and creates admission files and scholarship files based on it. A corresponding scholarship is created every other time so only have of admissions have a matching scholarship letter on a date.

## Installation and Running

1. Install [.Net 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
1. `git clone https://github.com/TelloVilla/CombinedLetters.git`
1. `cd` to main directory
1. `dotnet build`
1. `dotnet run -gentest 10` to make test directories and 10 files per day
1. `dotnet run` to run program normally
1. `dotnet run -clear` to clear test directories and files




## Commands
```
// Generate needed directories for testing and number of files per date directory
-gentest <# of files>

// Clear test files and directories
-clear
```