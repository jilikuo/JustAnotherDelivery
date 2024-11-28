set mypath=%cd%
@echo %mypath%
git checkout main
git pull
rmdir /s bin
mkdir bin
"C:\Program Files\Unity\Hub\Editor\2022.3.31f1\Editor\Unity.exe" -quit -batchmode -logFile build_game.log -projectPath %mypath% -executeMethod GameBuilder.Build
for /d %%X in (bin\*) do "c:\Program Files\7-Zip\7z.exe" a -tzip "%%X.zip" "%%X\"
