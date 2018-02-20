#!/usr/bin/env groovy

node('master') {
         def location = "RelativityAgent1\\RelativityAgent.sln"
		 def utilities
        
        stage('Stage Checkout') {
                checkout([$class: 'GitSCM', 
                branches: [[name: '*/master']], 
                doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], 
                userRemoteConfigs: [[url: 'https://github.com/relativitydev/agent-testable-code-fest2017.git']]])
        }
		stage('load utilites')
		{
				utilites = load 'S:/SourceCode/Fest2017/PowerShellScripts/Utilities.groovy'
		}
		
        stage('Stage build'){
                fileExists location
				bat 'S:/nuget.exe restore RelativityAgent1\\RelativityAgent.sln'
               // bat "\"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln"
			   utilites.build_solution(location)
        }
		stage('Stage Test'){
		
					bat 'S:/NUnit-2.6.4/bin/nunit-console.exe RelativityAgent1\\AgentUnitTests\\bin\\Debug\\AgentUnitTests.dll'
		 
       }
}
	   
	   
