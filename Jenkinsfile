#!/usr/bin/env groovy

node('master') {
         def location = "Relativity Agent1\\RelativityAgent.sln"
 
        stage('Stage Checkout') {
                checkout([$class: 'GitSCM', 
                branches: [[name: '*/master']], 
                doGenerateSubmoduleConfigurations: false, extensions: [], submoduleCfg: [], 
                userRemoteConfigs: [[url: 'https://github.com/relativitydev/agent-testable-code-fest2017.git']]])
        }
        stage('Stage build'){
                fileExists location
				bat 'S:/nuget.exe restore Relativity Agent1\\Relativity Agent1\\RelativityAgent1.csproj'
                bat "\"C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe\" Relativity Agent1\\RelativityAgent.sln"
        }
		stage('Test'){

         env.NODE_ENV = "test"
         print "Environment will be : ${env.NODE_ENV}"

       }
    
}