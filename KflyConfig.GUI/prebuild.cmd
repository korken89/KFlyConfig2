cd %1
git describe --tags HEAD > %2
git diff --shortstat >> %2
