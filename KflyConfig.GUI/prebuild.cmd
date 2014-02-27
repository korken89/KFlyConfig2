cd %1
git describe HEAD > %2
git diff --shortstat >> %2
