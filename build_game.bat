set mypath=%cd%
@echo %mypath%
git checkout main
git pull
"C:\Program Files\Unity\Hub\Editor\2022.3.31f1\Editor\Unity.exe" -quit -batchmode -logFile build_game.log -projectPath %mypath% -executeMethod GameBuilder.Build
