@echo off 
echo ---Deploying site
REM ---Deploy the wwwroot folder in repository to default target (wwwroot)
xcopy %DEPLOYMENT_SOURCE%\repository\webapp\* %DEPLOYMENT_TARGET%/Y /s
