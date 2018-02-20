#!/usr/bin/env groovy


def build_solution() 
         {
            bat "\"C:/Program Files (x86)/MSBuild/14.0/bin/MSBuild.exe\" RelativityAgent1\\RelativityAgent.sln /p:Configuration=Release /p:Platform=x64"
        }



