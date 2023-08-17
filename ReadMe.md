# MyQ Unattended Coding Test 

- version 2.5
- instructions document in \doc\ folder
- examples of input/output files in \doc\test\ folder
- code in \src\ folder
- unit tests in \tests\ folder

## Getting Started

### Application

- ***VisualStudio*** (or other IDE)

1. Unpack archive with solution items
2. Open solution (CleaningRobot.sln) in VS or other IDE
3. Open cleaning_robot project ( in \src folder)
    - open and rewrite ___"commandLineArgs"___ parameter in properties/launchSettings.json
    - copy input file (with the same name as added above) to the path \src\cleaning_robot\bin\Debug\net7.0\ 
    - set cleaning_robot project as default if isn't already and run by pressing F5 button or clicking the Start (or its alternative) button 
4. Check the same path (ending with net7.0) where two new files should appear
    - if any of files already exists every application run overwrites it
    - json file defined like second parameter in point 2 with result
    - txt log file with same name like result json

- ***Command line*** / ***Powershell***

1. Unpack archive with solution items
2. Navigate command line to \src\cleaning_robot\ and run build command
```
dotnet build
```
3. Navige command line to path \src\cleaning_robot\bin\Debug\net7.0\ and copy input json file in this folder
4. Run application with command below (name of json files are illustrative). Input json must have the same name like file copied in step 3
```
cleaning_robot input.json result.json
```
Run with powershell should be slightly different
```
.\cleaning_robot test1.json test1_result.json
```
5. Check the same path (ending with net7.0) two new files should appear. If any of files already exists every application run overwrites it
    - json file defined like second parameter in point 2 with result
    - txt log file with same name like result json


**In both scenarios is possible to run application in Release configuration. All what changes is Debun to Release in all paths.** Type of scenarion depends on project settings.

### Run tests
1. Unpack archive with solution items
2. Open solution (CleaningRobot.sln) in VS or other IDE
3. Open Test explorer
    - in VS in Test\TestExplorer menu
4. Run selected test or all of them
5. Check results     