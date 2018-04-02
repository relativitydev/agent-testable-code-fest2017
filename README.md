


INTRODUCTION
The Developer Experience team has come up with a set of scripts and instructions to help you set up a Continuous Integration (CI) pipeline to build and test your application against an On-Prem or Relativity One instance. You can use these build scripts, combine it with a CI tool to create full blown Continuous delivery system. One key feature that is included with these scripts is the ability of the scripts to run integration tests against a TestVM locally.
STACK OF TOOLS
*  GitHub
* PowerShell
* Git
* PSake
* Nunit framework -3.6.0
* Nuget (lastest) or nugget 3.3.0 for Visual Studio 2015
* Microsoft Visual Studio 2015 or 2017

PRE-SETUP
In order to get setup, lets talk about the contents 
1. Navigate to Github and log into your github account.
2. Clone Agent Testable Code to get the base development scripts. Checkout the master branch
3. Once you have the repository cloned, navigate to the developmentScripts folder. You should have the following scripts and folders in there
a. Buildtools
i. pacakges.config
b. Build.ps1
c. defaultBuildTest.ps1


BUILDTOOLS FOLDER
Build tools folder consists of pacakages.config that lists all the nugget packages we want to restore while compiling the project. The following tools get installed upon nugget restore. This particular nugget restore gets called in the ‘build.ps1’ PowerShell script and restores all the tools we will utilize for the project. Here is a screenshot of what your buildtools folder should look like after a restore



BUILD POWERSHELL SCRIPT
The build PowerShell sets up all the variables for directories, the solution file and restores nugget. It also calls the defaultBuildTest powershell script.
DEFAULTBUILTTEST POWERSHELL SCRIPT
This PowerShell script will compile your actual code and run your unit tests and integration tests. This script is written in psake. Psake is a build automation tool written in PowerShell. Using this framework, you can utilize your existing command line knowledge in Tasks. Tasks are just function calls and we can have dependencies on other tasks in your build script.
Note:
Compile your code in visual studio before configuring or running any of these build scripts. This will ensure that your build has no errors and anything that fails is probably due to the build scripts not being configured correctly.
INSTRUCTIONS TO SETUP A REPOSITORY
In order to have this CI pipeline set up for your repository, follow the following presetup steps to configure it:
1. Go to the buld.ps1 and update the following variables 
a. ‘SOLUTION_PATH’ field and the child path for that solution path.
‘SOLUTION_PATH’ is the path to the actual solution from the Base directory. If the base directory is the base of the directory, then the path should take you for example to ‘Source/Codeproject/Code.sln’ 
2. Save and close the build.ps1
3. Open the defaultBuildTest.ps1 and update the following:
a. $solution – location of the ‘.sln’ solution
b. $testDir – location of the integration test folder
c. $configSource – location of the ‘ap.config’ file you need the build script pointed to
d. $configDestination – location of the integration test config assembly
e. $testAssembly – location of the integration test assebmly
4. Find the name of the Integration Test Class and update it here.
Replace the yellow highlighted part with your class name :
exec { & $nunit_exe $testAssembly --result="$test_logs\IntegrationTest.xml;format=nunit2" } -errorMessage "Integration tests failed!


POWERSHELL COMMANDS TO RUN	
* Once setup is complete, run the build by using the following command. This will compile the build, run the unit tests and run the integration tests
o .\build.ps1
* If you just want to just run the unit tests run the following command
o .\build.ps1 UnitTest
* If you just want to run the integration tests run the following command
o .\build.ps1 IntegrationTest
* To clean up all the artifacts run the following command. We recommend running this everytime you make a code change or change a nugget package you install for the build to compile
o .\build.ps1 Clean



